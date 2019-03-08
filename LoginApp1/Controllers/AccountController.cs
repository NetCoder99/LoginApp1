using LoginApp1.Classes.OWIN;
using LoginApp1.DataConnections;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp2.Models.Account;

namespace LoginApp1.Controllers
{
    public class AccountController : Controller
    {
        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Login()
        {
            LoginCreds model = new LoginCreds();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginCreds login)
        {

            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;

                AppUser user = userManager.Find(login.UserName, login.PassWord);
                if (user == null)
                {
                    dynamic errorMessage = new { param1 = "UserName", param2 = "User Name not found." };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }

                if (user != null)
                {
                    var ident = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //use the instance that has been created. 
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);

                    dynamic successMessage = new { url = Url.Action("Index", "Home") };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(successMessage, JsonRequestBehavior.AllowGet);

                    //return Redirect(login.ReturnUrl ?? Url.Action("Index", "Home"));
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(login);
        }


        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult CreateAccount()
        {
            AppUser tmp_user = new AppUser();
            tmp_user.Email = "Petey_Cruiser@nowhere.com";
            tmp_user.Password = "CruiserPAss3";
            tmp_user.PasswordConfirm = "CruiserPAss3";
            tmp_user.FirstName = "Petey";
            tmp_user.LastName = "Cruiser";
            tmp_user.PhoneNumber = "333.555.2465";

            ViewBag.backLink = "Login";
            return View(tmp_user);
        }
        [HttpPost]
        public ActionResult CreateAccount(AppUser model)
        {
            System.Threading.Thread.Sleep(500);
            ViewBag.backLink = "Login";
            if (ModelState.IsValid)
            {
                using (UserAccountDB userAccountDB = new UserAccountDB())
                {
                    userAccountDB.Configuration.ValidateOnSaveEnabled = false;
                    if (userAccountDB.Users.Where(w => w.Email == model.Email).Count() > 0)
                    {
                        dynamic errorMessage = new { param1 = "UserName", param2 = "Email already exists." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        return Json(errorMessage, JsonRequestBehavior.AllowGet);
                    }


                    model.UserName = model.Email;
                    model.PasswordHash = HashFunctions.HashPassword(model.Password);
                    userAccountDB.Users.Add(model);
                    userAccountDB.SaveChanges();
                    return View(model);
                }
            }
            return View(model);
        }


    }
}
