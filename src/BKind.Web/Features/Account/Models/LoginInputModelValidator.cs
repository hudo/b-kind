using BKind.Web.Features.Account.Contracts;
using FluentValidation;

namespace BKind.Web.Features.Account.Models
{
    public class LoginInputModelValidator : AbstractValidator<LoginInputModel>
    {
        public LoginInputModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}