using LoginApp1.DataConnections;
using LoginApp1.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Classes.Account
{
    public class AppUserProcs
    {
        public static List<AppUser> GetUsers(AppRoleView model)
        {
            List<AppUser> rtn_list = new List<AppUser>();

            using (var db_context = new SqlExpIdentity())
            {
                model.appRole = db_context.Roles.Where(w => w.Id == model.appRole.Id).ToList().FirstOrDefault();
                var query = from users in db_context.Users select users;
                string q1 = query.ToString();

                if (!string.IsNullOrEmpty(model.FirstName))
                { query = query.Where(w => w.FirstName.StartsWith(model.FirstName));  }
                string q2 = query.ToString();

                if (!string.IsNullOrEmpty(model.LastName))
                { query = query.Where(w => w.LastName.StartsWith(model.LastName)); }
                string q3 = query.ToString();

                rtn_list = query.ToList();
            }

            return rtn_list;

        }

    }
}