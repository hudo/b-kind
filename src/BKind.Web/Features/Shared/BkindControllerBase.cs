using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BKind.Web.Features.Shared
{
    public abstract class BkindControllerBase : Controller
    {
        protected IMediator _mediator;

        private User _loggedUser;

        protected const string _ErrorKey = "__errors";
        protected const string _InfoKey = "__info";


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

        protected async Task<User> GetLoggedUserAsync()
        {
            if(!this.User.Identity.IsAuthenticated)
                return null;

            if (_loggedUser != null) return _loggedUser;

            var user = await _mediator.Send(new GetUserQuery(User.Identity.Name));

            _loggedUser = user.Result;

            return _loggedUser;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var model = (context.Result as ViewResult)?.Model as ViewModelBase;

            if (model != null)
            {
                model.Title = "Welcome to B-Kind!";
                model.Description = "SEO stuff";
            }
        }
    }
}