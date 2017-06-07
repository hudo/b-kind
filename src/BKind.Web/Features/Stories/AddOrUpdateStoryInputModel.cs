using BKind.Web.Core;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Stories
{
    public class AddOrUpdateStoryInputModel : ViewModelBase, IRequest<Response<Story>>, IUserIdentifier
    {
        public AddOrUpdateStoryInputModel()
        {
            this.Title = "Create or edit story";        
        }

        public int? StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}