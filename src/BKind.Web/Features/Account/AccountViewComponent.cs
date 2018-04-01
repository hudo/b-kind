using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Account
{
    public class AccountViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;

            return View("Default", username);
        }
    }
}