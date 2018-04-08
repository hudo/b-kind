using System.Linq;
using FluentValidation;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class AddOrUpdateStoryModelValidator : AbstractValidator<AddOrUpdateStoryInputModel>
    {
        private string[] _allowedTypes = new[] { "image/jpeg", "image/png" };

        public AddOrUpdateStoryModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            
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