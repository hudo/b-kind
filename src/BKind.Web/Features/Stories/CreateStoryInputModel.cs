using BKind.Web.Core;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Stories
{
    public class CreateStoryInputModel : ViewModelBase, IRequest<Response<Story>>, IUserIdentifier
    {
        public CreateStoryInputModel()
        {
            this.Title = "Create new story";        
        }

        public string StoryTitle { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}