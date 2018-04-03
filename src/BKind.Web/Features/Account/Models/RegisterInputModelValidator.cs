﻿using BKind.Web.Features.Account.Contracts;
using FluentValidation;

namespace BKind.Web.Features.Account.Models
{
    public class RegisterInputModelValidator : AbstractValidator<ProfileInputModel>
    {
        public RegisterInputModelValidator()
        {
            RuleFor(x => x.Nick).NotEmpty().MinimumLength(4);
            RuleFor(x => x.Username).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Length(5, 50).WithMessage("Password length must be at least 5 characters");
            RuleFor(x => x.PasswordConfirm).NotEmpty().Equal(x => x.Password).WithMessage("Password does not match the confirm password");
        }
    }
}