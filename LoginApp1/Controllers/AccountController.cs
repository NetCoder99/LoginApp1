using LoginApp1.Classes.Account;
using LoginApp1.DataConnections;
using LoginApp1.Models;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace LoginApp1.Controllers
{
    public class AccountController : Controller
    {
        // ===============================================================================
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.UrlReferrer == null)
            { ViewBag.Header1 = "Please Login to continue."; }
            else
            {
                var urls = HttpUtility.ParseQueryString(Request.UrlReferrer.Query).GetValues("ReturnUrl");
                if (urls != null)
                { ViewBag.Header1 = "Please Login to access that page."; }
                else
                { ViewBag.Header1 = "Please Login to continue."; }
            }

            LoginCreds model = new LoginCreds();
            model.UserName = "Petey_Cruiser4@nowhere.com";
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginCreds login)
        {
            ViewBag.Header1 = "Please Login to continue.";
            dynamic jsonMessage = null;
            if (ModelState.IsValid)
            {
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    AppUser user = userManager.FindByEmail(login.UserName);
                    if (user == null)
                    {
                        jsonMessage = new { param1 = "UserName", param2 = "User Name not found." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }
                    if (!userManager.CheckPassword(user, login.PassWord))
                    {
                        jsonMessage = new { param1 = "Password", param2 = "Invalid password." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }

                    if (user.SecurityStamp == null)
                    { userManager.UpdateSecurityStamp(user.Id); }

                    var ident = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                    var urls = HttpUtility.ParseQueryString(Request.UrlReferrer.Query).GetValues("ReturnUrl");
                    if (urls != null)
                    {
                        jsonMessage = new { url = urls[0].ToString() };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }
                    jsonMessage = new { url = Url.Action("Index", "Home") };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                jsonMessage = new { param1 = "ModelState", param2 = "ModelState was not valid." };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Json(jsonMessage, JsonRequestBehavior.AllowGet);
            }
        }

        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Logout()
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            AuthenticationManager.SignOut();
            System.Web.HttpContext.Current.User = null;
            return RedirectToAction("Login");
        }

        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult CreateAccount(int id = -1)
        {
            AppUser tmp_user = new AppUser();
            tmp_user.Email = "Petey_Cruiser@nowhere.com";
            tmp_user.Password = "pass1";
            tmp_user.PasswordConfirm = "pass1";
            tmp_user.FirstName = "Petey";
            tmp_user.LastName = "Cruiser";
            tmp_user.PhoneNumber = "333.555.2465";
            ViewBag.backLink = "Login";
            return View(tmp_user);
        }

        [HttpPost]
        public ActionResult CreateAccount(AppUser model)
        {
            ViewBag.backLink = "Login";
            model.UserName = model.Email;
            var validator = new AppUserValid();
            var errors = validator.Validate(model);

            if (errors.IsValid)
            {
                model.UserName = model.Email;
                model.PasswordHash = HashFunctions.HashPassword(model.Password);
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var ident = userManager.Create(model);
                    if (ident.Errors.Count() > 0)
                    {
                        if (((string[])ident.Errors)[0].Contains("is already taken"))
                        {
                            dynamic errorMessage = new { param1 = "UserName", param2 = "Account already exists." };
                            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                            return Json(errorMessage, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                //Dictionary<String, String>  err_list = GetModelErrors.GetErrors(ModelState);
            }
            return View(model);
        }
        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult EditAccount(int userId = -1)
        {
            if (userId < 1)
            { return RedirectToAction("CreateAccount"); }

            List<AppUser> appUsers = null;
            using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
            { appUsers = userManager.Users.Where(w => w.UserId == userId).ToList(); }

            if (appUsers.Count() == 1)
            { return View(appUsers[0]); }

            return RedirectToAction("CreateAccount"); 
        }
        [HttpPost]
        public ActionResult EditAccount(AppUser model)
        {
            List<string> include_fields = new List<string>() { "FirstName",
                                                               "LastName",
                                                               "DisplayName",
                                                               "PrefEmail",
                                                               "PrefText",
                                                               "PhoneNumber",
                                                               "LockoutEndDateUtc" };

            model.UserName = model.Email;
            var validator = new AppUserValid();
            var errors = validator.Validate(model);
            if (errors.IsValid)
            {
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
                {
                    AppUser appUser = userManager.FindByEmail(model.Email);
                    model.LockoutEndDateUtc = DateTime.Now;
                    List<ModelUpdates> upd_list = GetModelUpdates.GetUpdates(model, appUser, null, include_fields);
                    userManager.Update(appUser);
                    return View(appUser);
                }
            }
            else
            {
                return View();
            }
        }

        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult ManageAccounts()
        {
            using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
            {
                List<AppUser> model = userManager.Users.ToList();
                return View(model);
            }
        }


        // -------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult DeleteAccount(string Id)
        {
            if (Id == null)
            { return RedirectToAction("ManageAccounts", new { errorMessage = "User id was null." }); }

            System.Threading.Thread.Sleep(100);
            dynamic jsonMessage = null;
            if (ModelState.IsValid)
            {
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
                {
                    int userId = int.Parse(Id);
                    List<AppUser> appUsers = userManager.Users.Where(w => w.UserId == userId).ToList();
                    if (appUsers.Count() == 1)
                    {
                        userManager.Delete(appUsers[0]);
                        jsonMessage = new { param1 = "UserName", param2 = "Account was deleted." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        ViewBag.deleteMsg = "Account was deleted";
                        return RedirectToAction("ManageAccounts", new { deleteMsg = jsonMessage });
                        //return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }

                }
                string lcldeleteMsg = "Account was deleted: ";// + user_dtls.Email;
                ViewBag.deleteMsg = "Account was deleted";
                return RedirectToAction("ManageAccounts", new { deleteMsg = lcldeleteMsg });

            }
            return RedirectToAction("ManageAccounts", new { errorMessage = "Invalid Model state" });
        }

        // ===============================================================================
        [HttpGet]
        public ActionResult ResetPassword(string userName)
        {
            ViewBag.state = "Sending";
            ResetPassword model = new ResetPassword();
            model.Email = userName;
            model.ProcessStatus = "sending";
            return View(model);
        }
        // -------------------------------------------------------------------------------
        [HttpPost]
        public JsonResult ResetPassword(ResetPassword model)
        {
            System.Threading.Thread.Sleep(100);
            dynamic jsonMessage;

            if (model.ProcessStatus == "login")
            {
                string loginUrl = Url.Action("Login", "Account");
                jsonMessage = new { param1 = "gotologin", param2 = loginUrl };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(jsonMessage, JsonRequestBehavior.AllowGet);
            }

            if (String.IsNullOrEmpty(model.TempResetCode))
            {
                jsonMessage = new { param1 = "status", param2 = "waiting" };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(jsonMessage, JsonRequestBehavior.AllowGet);
            }

            if (model.TempResetCode != "test reset")
            {
                jsonMessage = new { param1 = "status", param2 = "invalidcode" };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Json(jsonMessage, JsonRequestBehavior.AllowGet);
            }

            if (model.TempResetCode == "test reset")
            {

                if (String.IsNullOrEmpty(model.Password) && String.IsNullOrEmpty(model.Password))
                {
                    jsonMessage = new { param1 = "status", param2 = "validcode" };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    model.ProcessStatus = "validcode";
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    UpdatePassword(HttpContext, model);
                    jsonMessage = new { param1 = "status", param2 = "passupdated" };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    model.ProcessStatus = "passupdated";
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jsonMessage = new { param1 = "status", param2 = "invalidpass" };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
            }

            jsonMessage = new { param1 = "state", param2 = "uknownstate" };
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            return Json(jsonMessage, JsonRequestBehavior.AllowGet);
        }


        private static void UpdatePassword(HttpContextBase HttpContext, ResetPassword model)
        {
            using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
            {
                AppUser appUser = userManager.FindByEmail(model.Email);
                userManager.PasswordHasher.HashPassword(model.Password);
                appUser.PasswordHash = userManager.PasswordHasher.HashPassword(model.Password);
                userManager.Update(appUser);
            }
        }

    }
}
