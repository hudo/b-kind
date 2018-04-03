using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BKind.Web.Controllers.Account
{
    public class AccountController : BkindControllerBase
    {
        private readonly IMemoryCache _cache;

        public AccountController(IMediator mediator, IMemoryCache cache) : base(mediator)
        {
            _cache = cache;
        }
        
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginInputModel { ReturnUrl = returnUrl });
        }

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

            if (!string.IsNullOrEmpty(inputModel.ReturnUrl))
                return Redirect(inputModel.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View(new ProfileInputModel());
        
        [HttpPost]
        public async Task<IActionResult> Register(ProfileInputModel model)
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _cache.Remove(CacheKeys.UserWithUsername(User.Identity.Name));

            return Redirect("/home/index");
        }
    }
}
