using BKind.Web.Features.Pages.Models;
using FluentValidation;

namespace BKind.Web.Features.Pages
{
    public class PageEditModelValidator : AbstractValidator<PageEditModel>
    {
        public PageEditModelValidator()
        {
            RuleFor(x => x.Page.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Page.Slug).NotEmpty().WithMessage("Slug is required");
        }
    }
}