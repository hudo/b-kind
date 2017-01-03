using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BKind.Web.Features.Account;
using BKind.Web.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegisterInputModel = BKind.Web.ViewModels.Account.RegisterInputModel;

namespace BKind.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IDatabase _db;
        private readonly IMediator _mediator;

        public AccountController(IDatabase db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public IActionResult Login()
        {
            //await HttpContext.Authentication.SignInAsync("AuthScheme",
            //    new ClaimsPrincipal(new ClaimsIdentity("hudo")));

            var model = new LoginInputModel();
            return View(model);
        }

        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            var response = await _mediator.SendAsync(inputModel);

            if(!response.Result)
            {
                MapToModelState(response);
                return View(inputModel);
            }

            return RedirectToAction("Login");
        }

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

        public async Task<IActionResult> Signout()
        {
            await HttpContext.Authentication.SignOutAsync("AuthScheme");
            return Redirect("/home/index");
        }
    }
}