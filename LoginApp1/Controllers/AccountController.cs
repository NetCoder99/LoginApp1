﻿using LoginApp1.Classes.Account;
using LoginApp1.DataConnections;
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
using WebApp2.Models.Account;

namespace LoginApp1.Controllers
{
    public class AccountController : Controller
    {
        // -------------------------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginCreds model = new LoginCreds();
            model.UserName = "Petey_Cruiser2@nowhere.com";

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginCreds login)
        {
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
            return View("Login");
        }

        //// -------------------------------------------------------------------------------
        //[HttpGet]
        //public ActionResult CreateAccount()
        //{
        //    AppUser tmp_user = new AppUser();
        //    tmp_user.Email = "Petey_Cruiser@nowhere.com";
        //    tmp_user.Password = "CruiserPAss3";
        //    tmp_user.PasswordConfirm = "CruiserPAss3";
        //    tmp_user.FirstName = "Petey";
        //    tmp_user.LastName = "Cruiser";
        //    tmp_user.PhoneNumber = "333.555.2465";

        //    ViewBag.backLink = "Login";
        //    return View(tmp_user);
        //}

        [HttpGet]
        public ActionResult CreateAccount(int id = -1)
        {
            int userId = int.Parse(id.ToString());
            dynamic jsonMessage = null;
            List<AppUser> appUsers = null;
            using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
            { appUsers = userManager.Users.Where(w => w.UserId == userId).ToList(); }

            if (appUsers.Count() == 1)
            {
                ViewBag.backLink = "ManageAccounts";
                return View(appUsers[0]);
            }
            
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
            System.Threading.Thread.Sleep(100);
            ViewBag.backLink = "Login";
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                model.PasswordHash = HashFunctions.HashPassword(model.Password);

                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
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
            return View(model);
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
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }

                }
                    //model.UserName = model.Email;
                    //model.PasswordHash = HashFunctions.HashPassword(model.Password);

                    //var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                    //var authManager = HttpContext.GetOwinContext().Authentication;
                    //var ident = userManager.Create(model);
                    //if (ident.Errors.Count() > 0)
                    //{
                    //    if (((string[])ident.Errors)[0].Contains("is already taken"))
                    //    {
                    //        dynamic errorMessage = new { param1 = "UserName", param2 = "Account already exists." };
                    //        HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    //        return Json(errorMessage, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                string lcldeleteMsg = "Account was deleted: ";// + user_dtls.Email;
                ViewBag.deleteMsg = "Account was deleted";
                return RedirectToAction("ManageAccounts", new { deleteMsg = lcldeleteMsg });

            }
            return RedirectToAction("ManageAccounts", new { errorMessage = "Invalid Model state" });
            //using (UserAccountDB userAccountDB = new UserAccountDB())
            //{
            //    userAccountDB.Configuration.ValidateOnSaveEnabled = false;
            //    UserAccount user_dtls = userAccountDB.UserDetails.Where(w => w.UserDetailId == ID).First();
            //    UserAccountDeleted user_dltd = new UserAccountDeleted(user_dtls);
            //    user_dltd.DeleteDate = DateTime.Now;
            //    userAccountDB.UserDeletes.Add(user_dltd);
            //    userAccountDB.SaveChanges();
            //    userAccountDB.UserDetails.Remove(user_dtls);
            //    userAccountDB.SaveChanges();
            //    string lcldeleteMsg = "Account was deleted: " + user_dtls.Email;
            //    ViewBag.deleteMsg = "Account was deleted";
            //    return RedirectToAction("ManageAccounts", new { deleteMsg = lcldeleteMsg });
            //}
        }
    }
}
