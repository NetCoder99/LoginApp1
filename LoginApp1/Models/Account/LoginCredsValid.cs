using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class LoginCredsValid : AbstractValidator<LoginCreds>
    {
        public LoginCredsValid()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Email or user name is required.")
                .EmailAddress().WithMessage("Email address was not valid.");
            RuleFor(x => x.PassWord)
                .NotEmpty().WithMessage("Password is required.");
                //.Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,15})").WithMessage("Password does not meet complexity requirements</br />At least one upper case, at least one lower case and a number, 8 to 15 characters long.");
        }
    }
}