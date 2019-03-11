using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.AspNet.Identity;
using LoginApp1.DataConnections;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LoginApp1.Classes.Account
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new UserAccountDB());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            RoleStore<AppRole> role_store = new RoleStore<AppRole>(new UserAccountDB());
            app.CreatePerOwinContext<RoleManager<AppRole>>((options, context) =>
                new RoleManager<AppRole>(
                role_store));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

    }
}
