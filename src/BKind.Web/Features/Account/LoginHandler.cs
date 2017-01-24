using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Infrastructure;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Infrastructure.Persistance;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BKind.Web.Features.Account
{
    public class LoginHandler : IAsyncRequestHandler<LoginInputModel, Response<bool>>
    {
        private readonly IDatabase _db;
        private readonly IHttpContextAccessor _httpContext;

        public LoginHandler(IDatabase db, IHttpContextAccessor httpContext)
        {
            _db = db;
            _httpContext = httpContext;
        }

        public async Task<Response<bool>> Handle(LoginInputModel message)
        {
            var response = new Response<bool>(false);
            var user = _db.Users.FirstOrDefault(x => x.Credential.Username == message.Username);

            if (user != null && user.Credential.PasswordHash == StringHelpers.ComputeHash(message.Password, user.Credential.Salt))
            {
                
                await _httpContext.HttpContext.Authentication.SignInAsync(Application.AuthScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "Reviewer")
                    }, "form")));

                response = new Response<bool>(true);
            }
            else
            {
                response.AddMessage("", "User doesn't exists or wrong password", ResponseMessageType.Error);
            }

            return response;
        }
    }
}