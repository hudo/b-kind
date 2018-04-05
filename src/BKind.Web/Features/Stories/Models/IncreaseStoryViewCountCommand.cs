using BKind.Web.Core;
using MediatR;

namespace BKind.Web.Features.Stories.Models
{
    public class IncreaseStoryViewCountCommand : IRequest<Response>
    {
        public IncreaseStoryViewCountCommand(int storyId)
        {
            StoryId = storyId;
        }

        public int StoryId { get; set; }
    }
}
