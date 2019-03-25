using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<AppUser> appUsers { get; set; }
        public List<AppUser> srchUsers { get; set; }

        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

    }
}