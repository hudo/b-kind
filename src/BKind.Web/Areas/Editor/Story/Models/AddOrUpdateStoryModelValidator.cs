using FluentValidation;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class AddOrUpdateStoryModelValidator : AbstractValidator<AddOrUpdateStoryInputModel>
    {
        public AddOrUpdateStoryModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            
            RuleFor(x => x.StoryTitle).NotEmpty()
                .When(x => x.Content?.Length > 200)
                .WithMessage("Story is longer than 200 characters, please use title!");
        }
    }
}