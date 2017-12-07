using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Commands
{
    public class ThumbsUpCommand : IRequest<Response>
    {
        public ThumbsUpCommand(User loggedUser, string slug)
        {
            LoggedUser = loggedUser;
            Slug = slug;
        }

        public User LoggedUser { get; }
        public string Slug { get; }
    }
}