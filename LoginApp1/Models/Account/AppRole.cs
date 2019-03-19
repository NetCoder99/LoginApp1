using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    [Table("AspNetRoles")]
    [FluentValidation.Attributes.Validator(typeof(AppRoleValid))]
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name)
        {  }

        [Display(Name = "Role Description")]
        [StringLength(50)]
        public string Description { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "User Count")]
        [NotMapped]
        public int UserCount { get; set; }

    }
}