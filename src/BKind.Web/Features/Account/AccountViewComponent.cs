using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Account
{
    public class AccountViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
                return View("Welcome", User.Identity.Name);

            return View("Login");
        }
    }
}