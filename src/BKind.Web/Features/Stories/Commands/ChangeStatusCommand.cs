using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Commands
{
    public class ChangeStatusCommand : IRequest<Response>
    {
        public ChangeStatusCommand(string slug, User user, Status newStatus)
        {
            Slug = slug;
            User = user;
            NewStatus = newStatus;
        }

        public string Slug { get; set; }
        public User User { get; set; }
        public Status NewStatus { get; set; }
    }
}