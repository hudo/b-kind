using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public abstract class BkindControllerBase : Controller
    {
        protected IMediator _mediator;

        protected BkindControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected void MapToModelState(Response response)
        {
            foreach (var message in response.AllMessages)
            {
                ModelState.AddModelError(message.Key, message.Message);
            }
        }

        protected async Task<User> LoggedUser()
        {
            if(!this.User.Identity.IsAuthenticated)
                return null;

            var user = await _mediator.Send(new GetOneQuery<User>(x => x.Username == User.Identity.Name));

            return user;
        }
    }
}