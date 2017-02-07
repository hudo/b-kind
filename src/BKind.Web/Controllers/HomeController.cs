using System.Reflection;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.Infrastructure.Persistance.StandardHandlers;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp;
using StructureMap.TypeRules;

namespace BKind.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabase _db;
        private readonly IMediator _mediator;

        public HomeController(IDatabase db, IMediator mediator)
        {
            var type1 = typeof(IAsyncRequestHandler<,>);
            var type = typeof(GetOneHandler<>);

            _db = db;
            _mediator = mediator;

            var story = _mediator.Send(new GetOneQuery<Story>(1)).Result;

            var stories = mediator.Send(new GetAllQuery<Story>(x => x.Id == 1, new PagedOptions<Story>(0, 10, x => x.Id)));
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