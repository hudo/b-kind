using FluentValidation;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class AddOrUpdateStoryModelValidator : AbstractValidator<AddOrUpdateStoryInputModel>
    {
        public AddOrUpdateStoryModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("Content should not be empty.");

            RuleFor(x => x.StoryTitle).Length(0, 70)
                .WithMessage("Title is to long. Keep the title under 70 characters");

            RuleFor(x => x.StoryTitle).NotEmpty()
                .When(x => x.Content?.Length > 200)
                .WithMessage("Story is longer than 200 characters, please use title!");
        }
    }
}