using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Pages.Models;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Pages.Domain
{
    public class SavePageHandler : IAsyncRequestHandler<PageEditModel, Response<Page>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public SavePageHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        public async Task<Response<Page>> Handle(PageEditModel model)
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

            var response = new Response<Page>(page);

            var slug = model.Page.Slug.ToLower();
            var existing = await _mediator.Send(new GetAllQuery<Page>(
                x => x.Slug.ToLower() == slug && x.Id != model.Page.Id, 
                new PagedOptions<Page>(pageSize: 1, orderBy:x => x.Id)));

            if (existing.Any())
            {
                response.AddError("Slug", "Slug already in use, please choose another");
                return response;
            }

            page.Title = model.Page.Title;
            page.Slug = slug;
            page.Content = model.Page.Content;
            page.ModifiedBy = model.User.Id;
            page.Modified = DateTimeOffset.Now;

            try
            {
                if (page.Id == 0)
                {
                    _uow.Add(page);
                }
                else
                {
                    _uow.Update(page);
                }

                await _uow.Commit();
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }
    }
}