using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BKind.Web.Controllers
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

            var user = await _mediator.Send(new GetOneQuery<User>(x => x.Username == User.Identity.Name, x => x.Roles));

            _loggedUser = user;

            return user;
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