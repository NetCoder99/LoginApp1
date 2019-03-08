using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp2.Models.Account
{
    public class LoginCreds
    {

        public int LoginCredsId  { get; set; }
        public int LoginAttempts { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }


    }
}