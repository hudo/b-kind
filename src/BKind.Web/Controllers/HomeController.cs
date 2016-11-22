using System;
using System.Linq;
using BKind.Web.Infrastructure;
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
                //FeaturedStories = _database.Stories.ToList()
            };

            return View(model);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}