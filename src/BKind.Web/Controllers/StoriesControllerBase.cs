using BKind.Web.Core;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public abstract class BkindControllerBase : Controller
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