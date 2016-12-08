using BKind.Web.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Register");
        }
    }
}