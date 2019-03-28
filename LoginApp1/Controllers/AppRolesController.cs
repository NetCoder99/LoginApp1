using LoginApp1.Classes;
using LoginApp1.Classes.Account;
using LoginApp1.DataConnections;
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
                {
                    model.appRole = appRole;
                    model.appUsers = GetUsersByRole(appRole);
                }
                // !!! else - handle not found, when id not null !!!
            }
            return View(model);
        }

        // ===============================================================================
        [HttpPost]
        public ActionResult EditAppRole(AppRoleView model, string save, string search)
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



        // ===============================================================================
        [HttpGet]
        public ActionResult AddUserRole()
        {
            int roleId = 35; 

            AppRoleView model = new AppRoleView();
            model.status = "Init";

            using (var roleManager1 = HttpContext.GetOwinContext().Get<AppRoleManager>())
            { model.appRole = roleManager1.Roles.Where(w => w.RoleId == roleId).ToList().FirstOrDefault(); }

            using (var userManager = HttpContext.GetOwinContext().Get<AppUserManager>())
            {
                model.appUsers = GetUsersByRole(model.appRole); //userManager.Users.ToList();
                model.srchUsers = userManager.Users.ToList();

                foreach (AppUser srchUser in model.srchUsers.ToList())
                {
                    AppUser testUser = model.appUsers.Where(w => w.Id == srchUser.Id).FirstOrDefault();
                    if (testUser != null)
                    {
                        model.srchUsers.Remove(srchUser);
                    }
                }

            }

            return View(model);
        }

        // ===============================================================================
        [HttpPost]
        public ActionResult AddUserRole(AppRoleView model)
        {
            System.Threading.Thread.Sleep(500);
            dynamic jsonMessage;

            if (!ModelState.IsValid)
            {
                Dictionary<string, string> modelErrors = GetModelErrors.GetErrors(ModelState);
                jsonMessage = new { param1 = "ModelState", param2 ="Error", param3 = modelErrors.ToList()[0].Value };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                return Json(jsonMessage, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (model.status == "AddUser")
                {
                    using (var userManager = HttpContext.GetOwinContext().Get<AppUserManager>())
                    {
                        userManager.AddToRole(model.key, model.appRole.Name);
                        jsonMessage = new { param1 = "AddUserRole", param2 = model.key, param3 = "User was added to role." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                if (model.status == "RemoveUser")
                {
                    using (var userManager = HttpContext.GetOwinContext().Get<AppUserManager>())
                    {
                        userManager.RemoveFromRole(model.key, model.appRole.Name);
                        jsonMessage = new { param1 = "RemoveUserRole", param2 = model.key, param3 = "User was removed from role." };
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(jsonMessage, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.status == "SearchUsers")
                {
                    List<AppUser> tmp_list = AppUserProcs.GetUsers(model);
                    model.srchUsers = tmp_list;
                    jsonMessage = new { param1 = "param1", param2 = "param2", param3 = "param2" };
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonMessage);
                    HttpContext.Response.AddHeader("srchMessage", jsonString);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return PartialView("SrchUsersTable", model);
                }

            }
            return View(model);
        }

        //===============================================================================
        private static List<AppUser> GetUsersByRole(AppRole appRole)
        {
            List<AppUser> rtn_list = new List<AppUser>();
            using (var db_context = new SqlExpIdentity())
            {
                var users1 = appRole.Users.Select(s => s.UserId).ToList();
                rtn_list = db_context.Users.Where(w => users1.Contains(w.Id)).ToList();
            }
            return rtn_list;
        }

    }
}
