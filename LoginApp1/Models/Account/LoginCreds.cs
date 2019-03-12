using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    [FluentValidation.Attributes.Validator(typeof(LoginCredsValid))]
    public class LoginCreds
    {
        public int LoginCredsId  { get; set; }
        public int LoginAttempts { get; set; }

        [Index(IsUnique = true)]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }


    }
}