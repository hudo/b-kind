using System.Linq;
using FluentValidation;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class AddOrUpdateStoryModelValidator : AbstractValidator<AddOrUpdateStoryInputModel>
    {
        private readonly string[] _allowedTypes = new[] { "image/jpeg", "image/png" };

        public AddOrUpdateStoryModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("Content should not be empty.");

            RuleFor(x => x.StoryTitle).Length(0, 70)
                .WithMessage("Title is to long. Keep the title under 70 characters");

            RuleFor(x => x.StoryTitle).NotEmpty()
                .When(x => x.Content?.Length > 200)
                .WithMessage("Story is longer than 200 characters, please use title!");

            RuleFor(x => x.Image).Custom((file, ctx) =>
            {
                if(file == null) return; 

                if(!_allowedTypes.Contains(file.ContentType))
                {
                    ctx.AddFailure("Image", $"JPG and PNG files supported. You uploaded {file.ContentType}");
                }
            });

        }
    }
}