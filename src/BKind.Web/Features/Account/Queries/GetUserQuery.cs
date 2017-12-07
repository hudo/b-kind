using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Account.Contracts
{
    public class GetUserQuery : IRequest<Response<User>>
    {
        public GetUserQuery(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}