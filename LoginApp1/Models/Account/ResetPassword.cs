using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    [FluentValidation.Attributes.Validator(typeof(ResetPasswordValid))]
    public class ResetPassword
    {
        public ResetPassword() { }
        public ResetPassword(string userName)
        {
            Email = userName;
        }

        [NotMapped]
        public string ProcessStatus { get; set; }

        [StringLength(50)]
        [NotMapped]
        public string TempResetCode { get; set; }

        [StringLength(50)]
        [NotMapped]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [StringLength(50)]
        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [StringLength(50)]
        [NotMapped]
        public string PasswordConfirm { get; set; }

    }
}