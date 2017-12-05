using BKind.Web.Features.Stories.Contracts;
using FluentValidation;

namespace BKind.Web.Features.Stories
{
    public class CreateStoryModelValidator : AbstractValidator<AddOrUpdateStoryInputModel>
    {
        public CreateStoryModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            
            RuleFor(x => x.StoryTitle).NotEmpty()
                .When(x => x.Content.Length > 200)
                .WithMessage("Story is longer than 200 characters, please use title!");
        }
    }
}