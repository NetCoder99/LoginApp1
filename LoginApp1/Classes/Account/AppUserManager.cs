using System.Security.Claims;
using System.Threading.Tasks;
using LoginApp1.DataConnections;
using LoginApp1.Models;
using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LoginApp1.Classes.Account
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(UserStore<AppUser> store) : base(store){ }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<UserAccountDB>()));
            return manager;
        }
    }
}