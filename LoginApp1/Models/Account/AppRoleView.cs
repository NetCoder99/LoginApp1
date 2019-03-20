using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class AppRoleView
    {
        public string key { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public AppRole appRole { get; set; }
        public List<AppRole> appRoles { get; set; }

        public SearchUsers searchUsers { get; set; }

        public AppRoleView()
        {
            searchUsers = new SearchUsers();
            //appRoles = new List<AppRole>();
        }
    }
}