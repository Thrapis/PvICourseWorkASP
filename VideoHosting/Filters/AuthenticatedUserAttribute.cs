using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Database.Entities.Sinthetic;

namespace VideoHosting.Filters
{
    public class AuthenticatedUserAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string jsonAccount = filterContext.HttpContext.User.Identity.Name;
                Account account = Account.FromJson(jsonAccount);

                filterContext.Controller.ViewBag.Login = account.Login;
            }
        }
    }
}