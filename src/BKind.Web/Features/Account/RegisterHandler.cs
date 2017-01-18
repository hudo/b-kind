using System.Threading.Tasks;
using BKind.Web.Core;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class RegisterHandler : IAsyncRequestHandler<RegisterInputModel, Response<bool>>
    {
        public Task<Response<bool>> Handle(RegisterInputModel message)
        {
            return Task.FromResult(Response.From(true));
        }
    }
}