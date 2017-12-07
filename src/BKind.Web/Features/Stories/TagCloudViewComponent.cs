using System.Threading.Tasks;
using BKind.Web.Features.Stories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Stories
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public TagCloudViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _mediator.Send(new GetTagCloudQuery());
            return View(tags);
        }
    }
}