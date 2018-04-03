using FluentValidation;

namespace BKind.Web.Features.News.Models
{
    public class NewsInputValidator : AbstractValidator<NewsInputModel>
    {
        public NewsInputValidator()
        {
            RuleFor(x => x.News.Title).NotEmpty().WithMessage("News title is required");
            RuleFor(x => x.News.Title).MaximumLength(50).WithMessage("Maximum title length is 50 characters");

            RuleFor(x => x.News.Content).NotEmpty().WithMessage("News content is required");
        }
    }
}