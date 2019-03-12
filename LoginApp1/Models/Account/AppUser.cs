using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using FluentValidation;

namespace LoginApp1.Models.Account
{
    [Table("AppUsers", Schema = "LoginApp1")]
    [FluentValidation.Attributes.Validator(typeof(AppUserValid))]
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            CreateDate = DateTime.Now;
        }

        [Display(Name = "User ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "Email Address")]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public override string Email { get; set; }

        [Display(Name = "Password")]
        [StringLength(50)]
        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [StringLength(50)]
        [NotMapped]
        public string PasswordConfirm { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Display(Name = "Notify Email")]
        public bool PrefEmail { get; set; }

        [Display(Name = "Notify Text")]
        public bool PrefText { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        
    }
}