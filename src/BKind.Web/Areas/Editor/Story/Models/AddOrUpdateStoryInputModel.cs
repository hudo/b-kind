using BKind.Web.Core;
using BKind.Web.Features.Account.Models;
using BKind.Web.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BKind.Web.Areas.Editor.Story.Models
{
    public class AddOrUpdateStoryInputModel : ViewModelBase, IRequest<Response<Model.Story>>, IUserIdentifier
    {
        public AddOrUpdateStoryInputModel()
        {
            this.Title = "Create or edit story";        
        }

        public int? StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public IFormFile Image { get; set; }

        public string ExistingImage { get; set; }
        

        public int UserId { get; set; }

        public static AddOrUpdateStoryInputModel From(Model.Story story)
        {
            return new AddOrUpdateStoryInputModel
            {
                StoryId = story.Id,
                StoryTitle = story.Title,
                Content = story.Content
            };
        }
    }
}