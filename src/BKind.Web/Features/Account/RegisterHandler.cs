using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class RegisterHandler : IAsyncRequestHandler<RegisterInputModel, Response>
    {
        private readonly IDatabase _db;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterHandler(IDatabase db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public Task<Response> Handle(RegisterInputModel message)
        {
            var response = new Response();

            if(_db.Users.Any(x => x.Username == message.Username))
                response.AddError("username", $"Username {message.Username} already in use");

            if(!response.HasErrors)
            {
                var salt = Guid.NewGuid().ToString();

                var user = new User
                {
                    Username = message.Username,
                    FirstName = message.Firstname,
                    LastName = message.Lastname,
                    Registered = DateTime.UtcNow,
                    Credential = new Credential
                    {
                        Username = message.Username,
                        PasswordHash = StringHelpers.ComputeHash(message.Password, salt),
                        Salt = salt,
                        IsActive = true
                    },
                    Roles = new List<Role> { new Visitor() }
                };

                try
                {
                    _unitOfWork.AddOrAttach(user);
                    _unitOfWork.Commit();
                }
                catch(Exception e)
                {
                    response.AddError("", e.Message);
                }
            }

            return Task.FromResult(response);
        }
    }
}