using System.Threading.Tasks;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BKind.Web.Core.StandardQueries;
using Story = BKind.Web.Model.Story;

namespace BKind.Web.Features.Stories
{
    public class StoriesViewComponent : ViewComponent
    {
        IMediator _mediator;

        public StoriesViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(StoriesDisplayMode mode)
        {
            var stories = await _mediator.Send(new GetAllQuery<Story>(new PagedOptions<Story>(orderBy: x => x.Modified, ascending: false)));   
            return View(stories);
        }
    }
}