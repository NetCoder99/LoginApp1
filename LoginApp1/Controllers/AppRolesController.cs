using LoginApp1.Classes.Account;
using LoginApp1.DataConnections;
using LoginApp1.Models;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        public ActionResult IndexAppRoles()
        {
            using (var roleManager1 = HttpContext.GetOwinContext().Get<AppRoleManager>())
            {
                var t_roles = roleManager1.Roles.ToList();
                AppRoleView model = new AppRoleView();
                model.appRoles = roleManager1.Roles.ToList();
                return View(model);
            }
        }

        public ActionResult DeleteAppRole(AppRoleView model)
        {
            dynamic jsonMessage;
            using (var role_Manager = HttpContext.GetOwinContext().Get<AppRoleManager>())
            {
                AppRole appRole = role_Manager.FindById(model.key);
                if (appRole != null)
                { role_Manager.Delete(appRole); }
                
                List<AppRole> t_roles = role_Manager.Roles.ToList();
                return PartialView("IndexAppRolesPartial", t_roles);
            }
        }


        [HttpGet]
        public ActionResult CreateAppRole()
        {
            AppRole model = new AppRole();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateAppRole(AppRole model)
        {
            System.Threading.Thread.Sleep(200);
            dynamic jsonMessage;

            if (!ModelState.IsValid)
            {
                Dictionary<string, string> modelErrors = GetModelErrors.GetErrors(ModelState);
            }
            else
            {
                var role_Manager = HttpContext.GetOwinContext().Get<AppRoleManager>();
                if (role_Manager.RoleExists(model.Name))
                {
                    jsonMessage = new { param1 = "RoleName", param2 = "Exists", param3 = "Role already exists." };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.CreateDate = DateTime.Now;
                    role_Manager.Create(model);
                    jsonMessage = new { param1 = "RoleName", param2 = "", param3 = "Role was created." };
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                }
            }
            return View(model);
        }


    }
}
