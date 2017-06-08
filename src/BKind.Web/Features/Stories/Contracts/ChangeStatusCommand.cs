using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ChangeStatusCommand : IRequest<Response>
    {
        public ChangeStatusCommand(int storyId, User user, Status newStatus)
        {
            StoryId = storyId;
            User = user;
            NewStatus = newStatus;
        }

        public int StoryId { get; set; }
        public User User { get; set; }
        public Status NewStatus { get; set; }
    }
}