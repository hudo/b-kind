using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class RegisterHandler : IAsyncRequestHandler<RegisterInputModel, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public RegisterHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response> Handle(RegisterInputModel message)
        {
            var response = new Response();

            var existing = await _mediator.Send(new GetOneQuery<User>(x => x.Username == message.Username));

            if(existing != null)
                response.AddError("username", $"Username {message.Username} already in use");

            if(!response.HasErrors)
            {
                var salt = Guid.NewGuid().ToString();

                var user = new User
                {
                    Username = message.Username,
                    FirstName = message.Firstname,
                    LastName = message.Lastname,
                    PasswordHash = StringHelpers.ComputeHash(message.Password, salt),
                    Salt = salt,
                    Registered = DateTime.UtcNow,
                    Roles = new List<Role> { new Visitor() }
                };

                try
                {
                    _unitOfWork.Add(user);
                    await _unitOfWork.Commit();
                }
                catch(Exception e)
                {
                    response.AddError("", e.Message);
                }
            }

            return response;
        }
    }
}