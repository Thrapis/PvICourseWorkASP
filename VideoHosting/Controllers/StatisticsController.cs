using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;

namespace VideoHosting.Controllers
{
    public class StatisticsController : Controller
    {
        public ActionResult GetMianStatistics(string pageId)
        {
            return PartialView("MainStatistics", pageId);
        }

        public ActionResult GetStatistics(string pageId,
                                            string type)
        {
            var connection = MainOracleConnection.GetConnection();
            VideoPageContext pageContext
                = new VideoPageContext(connection);
            VideoAnalyseContext analyseContext
                = new VideoAnalyseContext(connection);
            ViewBag.PageId = pageId;
            ViewBag.VideoName = 
                pageContext.GetById(pageId).VideoName;
            switch (type)
            {
                case "Rate": return PartialView("RateStatistics",
                    analyseContext.GetRateData(pageId));
                case "View": return PartialView("ViewStatistics",
                    analyseContext.GetViewData(pageId));
                case "Comment": return PartialView("CommentStatistics",
                    analyseContext.GetCommentData(pageId));
                default: break;
            }
            return Content("");
        }
    }
}