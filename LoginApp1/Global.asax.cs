using LoginApp1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LoginApp1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            var error = Server.GetLastError();

            Server.ClearError();
            httpContext.Response.Clear();
            httpContext.ClearError();
            HttpContext.Current.ClearError();

            HttpContext.Current.Session.Add("gbl_exception", error);
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            using (var controller = new ErrorController())
            {  ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));  }

        }
    }
}
