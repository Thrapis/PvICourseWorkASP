using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Page;

namespace VideoHosting.Controllers
{
    public class AccountController : Controller
    {
        [AcceptVerbs("Get","Post")]
        public ActionResult SignInWindow()
        {
            return PartialView("SignInWindow", new LoginModel());
        }

        [ValidateAntiForgeryToken, AcceptVerbs("Post")]
        public ActionResult SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var connection = MainOracleConnection.GetConnection();
                using (AccountContext accountContext = new AccountContext(connection))
                {
                    Account user = accountContext.SignIn(model.Email, model.Password);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.ToJson(), true);
                        return PartialView("SignedInWindow");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is no account with this email and password");
                        return PartialView("SignInWindow", new LoginModel());
                    }
                }
            }
            return PartialView("SignInWindow", new LoginModel());
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult CreateAccountWindow()
        {
            return PartialView("CreateAccountWindow", new RegisterModel());
        }

        [ValidateAntiForgeryToken, AcceptVerbs("Post")]
        public ActionResult CreateAccount(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var connection = MainOracleConnection.GetConnection();
                using (AccountContext accountContext = new AccountContext(connection))
                {
                    int created = accountContext.CreateAccount(model.Email, model.Login, model.Password);
                    if (created > 0)
                    {
                        return PartialView("AccountCreatedWindow");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Creation account error");
                        return PartialView("CreateAccountWindow", new RegisterModel());
                    }
                }
            }
            return PartialView("CreateAccountWindow", new RegisterModel());
        }


        [AcceptVerbs("Get")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}