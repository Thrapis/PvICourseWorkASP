using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Page;

namespace VideoHosting.Controllers
{
    public class AccountController : Controller
    {
        [AcceptVerbs("Get","Post")]
        public IActionResult SignInWindow()
        {
            return PartialView("SignInWindow", new LoginModel());
        }

        [ValidateAntiForgeryToken, AcceptVerbs("Post")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var connection = MainOracleConnection.GetConnection();
                using (AccountContext accountContext = new AccountContext(connection))
                {
                    Account user = accountContext.SignIn(model.Email, model.Password);
                    if (user != null)
                    {

                        await Authenticate(user.ToJson());
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
        public IActionResult CreateAccountWindow()
        {
            return PartialView("CreateAccountWindow", new RegisterModel());
        }

        [ValidateAntiForgeryToken, AcceptVerbs("Post")]
        public IActionResult CreateAccount(RegisterModel model)
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

        private async Task Authenticate(string userJson)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userJson)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}