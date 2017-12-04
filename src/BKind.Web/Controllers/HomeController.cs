using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Controllers
{
    public class HomeController : BkindControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                Title = "Welcome to Be Kind",
                CanWriteStory = User.Identity.IsAuthenticated
            };

            return View(model);
        }

        public IActionResult Test()
        {
            return Json("hello!");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var model = (context.Result as ViewResult)?.Model as ViewModelBase;

            if (model != null)
            {
                model.Title = "Welcome to B-Kind!";
                model.Description = "SEO stuff";
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}