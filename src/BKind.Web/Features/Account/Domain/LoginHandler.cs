using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BKind.Web.Features.Account.Domain
{
    public class LoginHandler : IAsyncRequestHandler<LoginInputModel, Response>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public LoginHandler(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response> Handle(LoginInputModel message)
        {
            var response = Response.Empty();

            var user = await _mediator.Send(new GetUserQuery(message.Username));

            if (user.HasResult && user.Result.PasswordHash == StringHelpers.ComputeHash(message.Password, user.Result.Salt))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Result.Username) };

                claims.Add(new Claim("isAdmin", user.Result.Is<Administrator>().ToString()));
                claims.Add(new Claim("isReviewer", user.Result.Is<Reviewer>().ToString()));
                claims.Add(new Claim("fullname", $"{user.Result.FirstName} {user.Result.LastName}"));

                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "form")));

                try
                {
                    user.Result.LastLogin = DateTime.UtcNow;

                    _unitOfWork.Update(user.Result);

                    await _unitOfWork.CommitAsync();
                }
                catch(Exception e)
                {
                    response.AddError("", e.Unwrap().Message);
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