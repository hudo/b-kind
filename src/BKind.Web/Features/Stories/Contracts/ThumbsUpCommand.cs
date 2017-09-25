using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ThumbsUpCommand : IRequest<Response>
    {
        public ThumbsUpCommand(User loggedUser, int storyId)
        {
            LoggedUser = loggedUser;
            StoryId = storyId;
        }

        public User LoggedUser { get; set; }
        public int StoryId { get; set; }
    }
}