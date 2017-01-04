using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.ViewComponents
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