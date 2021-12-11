using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using VideoHosting.Models.Database.Entities;

namespace VideoHosting.Filters
{
    public class AuthenticatedUserAttribute : Attribute, IActionFilter
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
                (filterContext.Controller as Controller).ViewBag.Login = account.Login;
            }
        }
    }
}