using System;
using System.Linq;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BKind.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabase _db;

        public HomeController(IDatabase db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                Title = "Welcome to Be Kind",
                DisplayMode = StoriesDisplayMode.FeaturedList
            };

            return View(model);
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