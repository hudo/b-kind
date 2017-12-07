using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Commands
{
    public class DeleteStoryCommand : IRequest<Response>
    {
        public DeleteStoryCommand(string slug, User userWithRoles)
        {
            Slug = slug;
            UserWithRoles = userWithRoles;
        }

        public string Slug { get; set; }

        public User UserWithRoles { get; set; }
    }
}