using BKind.Web.Features.News.Models;
using FluentValidation;

namespace BKind.Web.Features.News
{
    public class NewsInputValidator : AbstractValidator<NewsInputModel>
    {
        public NewsInputValidator()
        {
            RuleFor(x => x.News.Title).NotEmpty().WithMessage("News title is required");
            RuleFor(x => x.News.Title).MaximumLength(70).WithMessage("Maximum title length is 70 charachters");

            RuleFor(x => x.News.Content).NotEmpty().WithMessage("News content is required");
        }
    }
}