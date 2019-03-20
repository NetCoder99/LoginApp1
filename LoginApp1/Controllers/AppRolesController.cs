using LoginApp1.Classes.Account;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LoginApp1.Controllers
{
    public class AppRolesController : Controller
    {

        // ===============================================================================
        public ActionResult IndexAppRoles()
        {
            using (var roleManager1 = HttpContext.GetOwinContext().Get<AppRoleManager>())
            {
                var t_roles = roleManager1.Roles.ToList();
                AppRoleView model = new AppRoleView();
                model.appRoles = roleManager1.Roles.ToList();
                model.status = "Init";
                return View(model);
            }
        }

        // ===============================================================================
        public ActionResult DeleteAppRole(AppRoleView model)
        {
            using (var role_Manager = HttpContext.GetOwinContext().Get<AppRoleManager>())
            {
                AppRole appRole = role_Manager.FindById(model.key);
                if (appRole != null)
                {
                    role_Manager.Delete(appRole);
                    model.status = "OK";
                    model.message = "Role was deleted.";
                }
                else
                {
                    model.status = "Error";
                    model.message = "Role was not found.";
                }
                model.appRoles = role_Manager.Roles.ToList();
                return PartialView("IndexAppRolesPartial", model);
            }
        }

        // ===============================================================================
        [HttpGet]
        public ActionResult EditAppRole(string id)
        {
            AppRoleView model = new AppRoleView();
            using (var role_Manager = HttpContext.GetOwinContext().Get<AppRoleManager>())
            {
                AppRole appRole = role_Manager.Roles.FirstOrDefault(f => f.Id == id);
                if (appRole != null)
                { model.appRole = appRole; }
                // !!! else - handle not found, when id not null !!!
            }
            return View(model);
        }

        // ===============================================================================
        [HttpPost]
        public ActionResult EditAppRole(AppRoleView model)
        {
            System.Threading.Thread.Sleep(200);
            dynamic jsonMessage;

            if (!ModelState.IsValid)
            {
                Dictionary<string, string> modelErrors = GetModelErrors.GetErrors(ModelState);
            }
            else
            {
                using (var role_Manager = HttpContext.GetOwinContext().Get<AppRoleManager>())
                {
                    AppRole appRole = role_Manager.Roles.FirstOrDefault(f => f.Id == model.appRole.Id);
                    if (appRole == null)
                    {
                        appRole = new AppRole();
                        appRole.Name = model.appRole.Name;
                        appRole.Description = model.appRole.Description;
                        appRole.CreateDate = DateTime.Now;
                        role_Manager.Create(appRole);
                        jsonMessage = new { param1 = "RoleName", param2 = "", param3 = "Role was created." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }

                    appRole.Description = model.appRole.Description;
                    role_Manager.Update(appRole);
                    jsonMessage = new { param1 = "RoleName", param2 = "OK", param3 = "Role was updated." };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
            }
            return View(model);
        }


            //using (var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>())
            //{
            //    List<AppUser> model = userManager.Users.ToList();
            //    return View(model);
            //}


    }
}
