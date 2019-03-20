using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class AspNetUserRoles : IdentityUserRole
    {
        [Display(Name = "User ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index(IsUnique = true)]
        public int UserRoleId { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

    }
}