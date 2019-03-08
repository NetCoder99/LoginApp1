using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoginApp1.Models.Account
{
    [Table("AppUsers", Schema = "LoginApp1")]
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public override string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50)]
        [NotMapped]
        [Compare(nameof(PasswordConfirm), ErrorMessage = "Passwords don't match.")]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [Required(ErrorMessage = "Password confirmation is required")]
        [StringLength(50)]
        [NotMapped]
        public string PasswordConfirm { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Display(Name = "Notify Email")]
        public bool PrefEmail { get; set; }

        [Display(Name = "Notify Text")]
        public bool PrefText { get; set; }

    }
}