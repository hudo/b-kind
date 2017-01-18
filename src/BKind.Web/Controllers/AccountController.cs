using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account;
using BKind.Web.Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Login() => View(new LoginInputModel());
     
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(inputModel);

            var response = await _mediator.SendAsync(inputModel);

            if(!response.Result)
            {
                MapToModelState(response);
                return View(inputModel);
            }

            // todo: move to command handler or application service?
            await HttpContext.Authentication.SignInAsync(Application.AuthScheme,
                new ClaimsPrincipal(
                    new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, inputModel.Username),
                        new Claim(ClaimTypes.Role, "Reviewer"),
                    }, "form")));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View(new RegisterInputModel());
        
        [HttpPost]
        public IActionResult Register(RegisterInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Register");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.Authentication.SignOutAsync(Application.AuthScheme);
            return Redirect("/home/index");
        }
    }
}
