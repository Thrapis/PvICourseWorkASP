using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using VideoHosting.Filters;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Database.Entities.Sinthetic;
using VideoHosting.Models.Utils;

namespace VideoHosting.Controllers
{
	public class HomeController : Controller
	{
		[AuthenticatedUser]
		public ActionResult Index()
		{
			var connection = MainOracleConnection.GetConnection();
			InfoPackageContext previewContext = new InfoPackageContext(connection);

			IEnumerable<VideoPreviewInfo> previews = previewContext.GetNFirstVideoPreview(12);

			if (User.Identity.IsAuthenticated)
            {
				Account account = Account.FromJson(User.Identity.Name);

				IEnumerable<VideoPreviewInfo> featured = previewContext.GetNFirstFeaturedVideoPreview(account.Id, 3);
				ViewBag.Featured = featured;

				SetLastViews(this, account, connection);
			}
				
			return View(previews);
		}


		[AuthenticatedUser]
		public ActionResult MyVideo()
		{
			if (User.Identity.IsAuthenticated)
			{
				var connection = MainOracleConnection.GetConnection();

				string jsonAccount = User.Identity.Name;
				Account account = Account.FromJson(jsonAccount);

				InfoPackageContext infoPackageContext = new InfoPackageContext(connection);
				IEnumerable<VideoEditInfo> editInfos = infoPackageContext.GetVideoEditInfo(account.Id);

				SetLastViews(this, account, connection);
				return View(editInfos);
			}
			return Content("");
		}


		[AuthenticatedUser]
		[OutputCache(Duration = 240, Location = OutputCacheLocation.Any)]
		public ActionResult VideoUpload()
		{
			if (User.Identity.IsAuthenticated)
            {
				SetLastViews(this, Account.FromJson(User.Identity.Name), MainOracleConnection.GetConnection());
				return View();
			}
			else
				return Content("");
		}


		[AuthenticatedUser]
		public ActionResult StatisticsAndAnalyse()
		{
			if (User.Identity.IsAuthenticated)
            {
				Account account = Account.FromJson(User.Identity.Name);
				var connection = MainOracleConnection.GetConnection();
				InfoPackageContext infoPackageContext = new InfoPackageContext(connection);
				IEnumerable<ShortVideoInfo> selects = infoPackageContext.GetShortVideoList(account.Id);

				SetLastViews(this, account, connection);
				return View(selects);
			}

			return Content("");
		}

		private void SetLastViews(Controller controller, Account account, OracleConnection connection)
        {
			InfoPackageContext infoPackageContext = new InfoPackageContext(connection);
			IEnumerable<ShortVideoInfo> lastViews = infoPackageContext.GetLastViews(account.Id);
			controller.ViewBag.LastViews = lastViews;
		}


		[AuthenticatedUser]
		public ActionResult Search(string searchPattern)
		{
			var connection = MainOracleConnection.GetConnection();
			InfoPackageContext previewContext = new InfoPackageContext(connection);

			IEnumerable<VideoPreviewInfo> previews = previewContext.SearchNFirstVideoPreviewLike(searchPattern, 12);
			ViewBag.LastSearch = searchPattern;

			if (User.Identity.IsAuthenticated)
				SetLastViews(this, Account.FromJson(User.Identity.Name), connection);
			return View(previews);
		}
	}
}