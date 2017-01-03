using System.Threading.Tasks;
using BKind.Web.Infrastructure;
using BKind.Web.Infrastructure.Persistance;
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
            response.AddMessage("username", "User doesn't exists", ResponseMessageType.Error);
            
            return Task.FromResult(response);
        }
    }
}