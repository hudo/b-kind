using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BKind.Web.Features.Account
{
    public class LoginHandler : IAsyncRequestHandler<LoginInputModel, Response>
    {
        private readonly IDatabase _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWork _unitOfWork;

        public LoginHandler(IDatabase db, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _db = db;
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(LoginInputModel message)
        {
            var response = Response.Empty();

            var user = _db.Users.FirstOrDefault(x => x.Credential.Username == message.Username);

            if (user != null && user.Credential.PasswordHash == StringHelpers.ComputeHash(message.Password, user.Credential.Salt))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };

                foreach (var role in user.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));

                await _httpContext.HttpContext.Authentication.SignInAsync(Application.AuthScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "form")));

                try
                {
                    user.LastLogin = DateTime.UtcNow; 
                    _unitOfWork.AddOrAttach(user);

                    await _unitOfWork.Commit();
                }
                catch(Exception e)
                {
                    // log
                    // we will allow user to login and just log why last login date couldn't be saved
                }
            }
            else
            {
                response.AddMessage("", "User doesn't exists or wrong password", ResponseMessageType.Error);
            }

            return response;
        }
    }
}