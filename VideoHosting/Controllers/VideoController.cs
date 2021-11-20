using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoHosting.Filters;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Database.Entities.Sinthetic;
using VideoHosting.Models.Status;
using VideoHosting.Models.Utils;
using static VideoHosting.Models.Database.Entities.AuthVideoView;

namespace VideoHosting.Controllers
{
	public class VideoController : Controller
	{
		[AcceptVerbs("Get"), AuthenticatedUser]
		public ActionResult Watch()
		{
			string pageId = Request.QueryString.Get("pageId");
			var connection = MainOracleConnection.GetConnection();
			VideoPageContext videoPageContext = new VideoPageContext(connection);
			InfoPackageContext infoPackageContext = new InfoPackageContext(connection);

			int accountId = 0;
			if (User.Identity.IsAuthenticated)
            {
				string jsonAccount = User.Identity.Name;
				Account account = Account.FromJson(jsonAccount);
				accountId = account.Id;
				videoPageContext.AddAuthViewToVideoById(pageId, account.Id);
            }
			else
            {
				string ipAddress = Request.UserHostAddress;
				videoPageContext.AddNonAuthViewToVideoById(pageId, ipAddress);
            }

			VideoPageInfo videoPageInfo = infoPackageContext.GetVideoPageInfo(pageId, accountId);
			IEnumerable<CommentInfo> comments = infoPackageContext.GetCommentsOfVideoPage(pageId);
			ViewBag.Comments = comments;
			int lastCommentId = 0;
			if (comments.Count() > 0)
				lastCommentId = comments.Max(c => c.CommentId);
			ViewBag.LastComment = lastCommentId;

			return View(videoPageInfo);
		}


		[AcceptVerbs("Get")]
		public EmptyResult GetVideoSource()
		{
			string pageId = Request.QueryString.Get("pageId");
			int quality = int.Parse(Request.QueryString.Get("quality"));

			var connection = MainOracleConnection.GetConnection();
			VideoSourceContext context = new VideoSourceContext(connection);
			VideoSource video = context.GetByVideoPageIdAndQuality(pageId, quality);

			Response.ContentType = "video/" + video.Format.Replace(".", "");

			Response.Headers.Add("Content-Length", video.Size.ToString());
			Response.Headers.Add("accept-ranges", "bytes");

			HttpContext.Response.BinaryWrite(video.Data);

			return new EmptyResult();
		}


		[AcceptVerbs("Get")]
		public ActionResult GetThumbnail()
		{
			string pageId = Request.QueryString.Get("pageId");

			var connection = MainOracleConnection.GetConnection();
			VideoThumbnailContext context = new VideoThumbnailContext(connection);
			var preview = context.GetByVideoPageId(pageId);

			Response.ContentType = $"image/{preview.Format.Replace(".", "")}";
			Response.BinaryWrite(preview.Data);
			Response.End();

			return new EmptyResult();
		}

		[Route("commentPost")]
		public ContentResult CommentVideo(string pageId, string commentText)
		{
			if (User.Identity.IsAuthenticated)
			{
				var connection = MainOracleConnection.GetConnection();
				VideoPageContext pageContext = new VideoPageContext(connection);
				Account account = Account.FromJson(User.Identity.Name);
				pageContext.AddComment(new Comment(pageId, account.Id, commentText));
				return Content($"");
			}
			return Content("");
		}

		[Route("ratePost")]
		public ContentResult RateVideo(string pageId, RateType rate)
        {
			if (User.Identity.IsAuthenticated)
            {
				var connection = MainOracleConnection.GetConnection();
				VideoPageContext pageContext = new VideoPageContext(connection);
				Account account = Account.FromJson(User.Identity.Name);
				pageContext.SetRateToVideoById(pageId, account.Id, rate);
				return Content($"{rate.ToString().ToLower()}");
			}
			return Content("");
        }

		[Route("uploadPost")]
		public ContentResult UploadVideo(HttpPostedFileBase videoFile, string videoName)
		{
			if (User.Identity.IsAuthenticated && videoFile != null)
			{
				Guid generatedGuid = GuidHelper.GenerateGuid();
				string stringGuid = GuidHelper.ConvertToString(generatedGuid);

				string directory = Server.MapPath("~/Upload/TempUpload/");
				if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

				string uploadedPath = Server.MapPath("~/Upload/TempUpload/" + stringGuid
					+ FilePathHelper.GetExtension(videoFile.FileName));

				videoFile.SaveAs(uploadedPath);

				VideoStatusContainer statusContainer = VideoStatusContainer.GetInstance();
				statusContainer.AddStatus(new VideoStatus(stringGuid, "Prepared to handle video", "Creating thumbnail", 3));

				string jsonAccount = User.Identity.Name;
				Account account = Account.FromJson(jsonAccount);

				Task.Factory.StartNew(() => HandleVideo(stringGuid, account.Id, videoName, uploadedPath));

				return Content(statusContainer.GetStatus(stringGuid).ToJson());
			}
			return Content("");
		}

		void HandleVideo(string stringGuid, int accountId, string videoName, string uploadedPath)
        {
			VideoStatusContainer statusContainer = VideoStatusContainer.GetInstance();
			var connection = MainOracleConnection.GetConnection();

			VideoPageContext videoPageContext = new VideoPageContext(connection);
			VideoPage videoPage = new VideoPage(stringGuid, accountId, videoName, DateTime.Now);
			videoPageContext.Insert(videoPage);

			string resolution_480 = "-vf scale=854:480";
			string resolution_720 = "-vf scale=1280:720";
			string resolution_1080 = "-vf scale=1920:1080";
			string additional_settings = " -vcodec libx264 -crf 30 -preset fast";

			string thumbnailPath = Server.MapPath("~/Upload/TempUpload/" + stringGuid + "_th.jpeg");
			string convertedPath_480 = Server.MapPath("~/Upload/TempUpload/" + stringGuid + "_480.mp4");
			string convertedPath_720 = Server.MapPath("~/Upload/TempUpload/" + stringGuid + "_720.mp4");
			string convertedPath_1080 = Server.MapPath("~/Upload/TempUpload/" + stringGuid + "_1080.mp4");

			FFMpegConverter ffMpeg = new FFMpegConverter();
			ConvertSettings settings = new ConvertSettings();

			VideoThumbnailContext videoThumbnailContext = new VideoThumbnailContext(connection);
			ffMpeg.GetVideoThumbnail(uploadedPath, thumbnailPath);
			VideoThumbnail videoThumbnail = new VideoThumbnail(thumbnailPath, stringGuid);
			videoThumbnailContext.Insert(videoThumbnail);
			if (System.IO.File.Exists(thumbnailPath)) System.IO.File.Delete(thumbnailPath);
			string thumbnailPage = videoThumbnail.GetForPage();
			statusContainer.AddStatus(new VideoStatus(stringGuid, "Thumbnail created", "Handling 480 quality", 15, thumbnailPage));

			VideoSourceContext videoSourceContext = new VideoSourceContext(connection);

			settings.CustomOutputArgs = resolution_480 + additional_settings;
			ffMpeg.ConvertMedia(uploadedPath, null, convertedPath_480, Format.mp4, settings);
			VideoSource videoSource_480 = new VideoSource(convertedPath_480, stringGuid, 480);
			videoSourceContext.Insert(videoSource_480);
			if (System.IO.File.Exists(convertedPath_480)) System.IO.File.Delete(convertedPath_480);
			statusContainer.AddStatus(new VideoStatus(stringGuid, "Quality 480 handled", "Handling 720 quality", 44));

			settings.CustomOutputArgs = resolution_720 + additional_settings;
			ffMpeg.ConvertMedia(uploadedPath, null, convertedPath_720, Format.mp4, settings);
			VideoSource videoSource_720 = new VideoSource(convertedPath_720, stringGuid, 720);
			videoSourceContext.Insert(videoSource_720);
			if (System.IO.File.Exists(convertedPath_720)) System.IO.File.Delete(convertedPath_720);
			statusContainer.AddStatus(new VideoStatus(stringGuid, "Quality 720 handled", "Handling 1080 quality", 73));

			settings.CustomOutputArgs = resolution_1080 + additional_settings;
			ffMpeg.ConvertMedia(uploadedPath, null, convertedPath_1080, Format.mp4, settings);
			VideoSource videoSource_1080 = new VideoSource(convertedPath_1080, stringGuid, 1080);
			videoSourceContext.Insert(videoSource_1080);
			if (System.IO.File.Exists(convertedPath_1080)) System.IO.File.Delete(convertedPath_1080);
			statusContainer.AddStatus(new VideoStatus(stringGuid, "Quality 1080 handled", "Сompletion of handling", 99));


			if (System.IO.File.Exists(uploadedPath)) System.IO.File.Delete(uploadedPath);
			statusContainer.AddStatus(new VideoStatus(stringGuid, "Video handled", "", 100));
		}
	}
}