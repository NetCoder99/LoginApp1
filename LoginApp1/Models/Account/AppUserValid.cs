using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class AppUserValid : AbstractValidator<AppUser>
    {
        public AppUserValid()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adress is required.")
                .EmailAddress().WithMessage("Email address format was not valid.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
            //.Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,15})").WithMessage("Password does not meet complexity requirements.");
            //.Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,15})").WithMessage("Password does not meet complexity requirements</br />One upper case, One lower case and a number, 8 to 15 characters long.");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Password Confirmation is required.")
                .Equal(x=>x.Password).WithMessage("Passwords do not match.");


        }

    }
}