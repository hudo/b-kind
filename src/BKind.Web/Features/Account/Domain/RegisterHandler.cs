using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Account.Domain
{
    public class RegisterHandler : IAsyncRequestHandler<ProfileInputModel, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public RegisterHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response> Handle(ProfileInputModel message)
        {
            var response = new Response();

            var existing = await _mediator.Send(new GetOneQuery<User>(x => x.Username.ToLower() == message.Username.ToLower()));

            if(existing != null)
                response.AddError("username", $"Username '{message.Username}' already in use");

            var existingNick = await _mediator.Send(new GetOneQuery<User>(x => x.Nickname.ToLower() == message.Nick.ToLower()));

            if (existingNick != null)
                response.AddError("nick", $"Nickname '{message.Nick}' already in use");

            if(!response.HasErrors)
            {
                var salt = Guid.NewGuid().ToString();

                var user = new User
                {
                    Username = message.Username,
                    Nickname = message.Nick,
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
                    await _unitOfWork.CommitAsync();
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