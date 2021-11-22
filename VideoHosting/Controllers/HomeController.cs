using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

				InfoPackageContext previewContext = new InfoPackageContext(connection);
				IEnumerable<VideoPreviewInfo> previews = previewContext.GetNFirstVideoPreview(12);

				return View(previews);
			}
			return Content("");
		}

		[AuthenticatedUser]
		public ActionResult VideoLoad()
		{
			return View();
		}
	}
}