using System.Threading.Tasks;
using BKind.Web.Controllers;
using BKind.Web.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Account
{
    public class AccountController : StoriesControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Login() => View(new LoginInputModel());
     
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(inputModel);

            var response = await _mediator.Send(inputModel);

            if(response.HasErrors)
            {
                MapToModelState(response);
                return View(inputModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View(new RegisterInputModel());
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _mediator.Send(model);

            if(response.HasErrors)
            {
                MapToModelState(response);
                return View(model);
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.Authentication.SignOutAsync(Application.AuthScheme);
            return Redirect("/home/index");
        }
    }
}
