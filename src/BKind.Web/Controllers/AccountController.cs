using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDatabase _db;

        public AccountController(IDatabase db)
        {
            _db = db;
        }

        [Authorize]
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

        //todo

        public async Task<ActionResult> Login()
        {
            //await HttpContext.Authentication.SignInAsync("AuthScheme",
            //    new ClaimsPrincipal(new ClaimsIdentity("hudo")));

            return Redirect("/home/index");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.Authentication.SignOutAsync("AuthScheme");
            return Redirect("/home/index");
        }
    }
}