using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Pages.Models;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Pages.Domain
{
    public class SavePageHandler : IAsyncRequestHandler<PageEditModel, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public SavePageHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        public async Task<Response> Handle(PageEditModel model)
        {
            Page page;
            if (model.Page.Id != 0)
            {
                page = await _mediator.Send(new GetByIdQuery<Page>(model.Page.Id));
            }
            else
            {
                page = new Page();
                page.Published = DateTimeOffset.Now;
                page.CreatedBy = model.User.Id;
            }

            page.Title = model.Page.Title;
            page.Slug = model.Page.Slug;
            page.Content = model.Page.Content;
            page.ModifiedBy = model.User.Id;
            page.Modified = DateTimeOffset.Now;

            return new Response();
        }
    }
}