using BKind.Web.Core;
using BKind.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected void MapToModelState(Response response)
        {
            foreach (var message in response.AllMessages)
            {
                ModelState.AddModelError(message.Key, message.Message);
            }
        }
    }
}