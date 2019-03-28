using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.AspNet.Identity;
using LoginApp1.DataConnections;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using LoginApp1.Classes.Account;

namespace LoginApp1
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new UserAccountDB());
            app.CreatePerOwinContext(() => new UserRoleDB());

            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

    }
}
