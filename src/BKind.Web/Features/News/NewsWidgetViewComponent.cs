using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Features.News.Models;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.News
{
    public class NewsWidgetViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public NewsWidgetViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NewsWidgetModel();

            model.Newses = await _mediator.Send(new GetAllQuery<Model.News>(
                new PagedOptions<Model.News>(
                    pageSize: 5,
                    orderBy: x => x.Published, 
                    ascending: false)));
                

            if(User.Identity.IsAuthenticated)
            {
                var user = await _mediator.Send(new GetUserQuery(User.Identity.Name));

                if (user.Result.Is<Administrator>())
                    model.CanAdd = true;
            }

            return View(model);
        }
    }
}