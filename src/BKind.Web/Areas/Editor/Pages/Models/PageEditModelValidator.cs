using FluentValidation;

namespace BKind.Web.Areas.Editor.Pages.Models
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