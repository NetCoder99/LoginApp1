using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class SearchUsers
    {

        [Display(Name = "Email")]
        [StringLength(50)]
        [NotMapped]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        [NotMapped]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        [NotMapped]
        public string LastName { get; set; }

    }
}