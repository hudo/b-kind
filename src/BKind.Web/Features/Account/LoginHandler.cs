using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Infrastructure;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Infrastructure.Persistance;
using FluentValidation;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class LoginHandler : IAsyncRequestHandler<LoginInputModel, Response<bool>>
    {
        private readonly IDatabase _db;

        public LoginHandler(IDatabase db)
        {
            _db = db;
        }

        public Task<Response<bool>> Handle(LoginInputModel message)
        {
            var response = new Response<bool>(false);
            var user = _db.Users.FirstOrDefault(x => x.Credential.Username == message.Username);

            if (user != null && user.Credential.PasswordHash == StringHelpers.ComputeHash(message.Password, user.Credential.Salt))
            {
                response = new Response<bool>(true);
            }
            else
            {
                response.AddMessage("", "User doesn't exists or wrong password", ResponseMessageType.Error);
            }

            return Task.FromResult(response);
        }
    }
}