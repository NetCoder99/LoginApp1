using LoginApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginApp1.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string message)
        {
            var session = HttpContext.Session["gbl_exception"];
            Errors err_model = new Errors() { error_message = "That page does not exist or can not be served at this time." };
            return View("Errors", err_model);
        }
        public ActionResult Error404()
        {
            return View("Errors");
        }

    }
}
