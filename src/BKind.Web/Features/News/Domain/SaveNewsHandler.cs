using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.News.Models;
using BKind.Web.Infrastructure.Helpers;
using MediatR;

namespace BKind.Web.Features.News.Domain
{
    public class SaveNewsHandler : IAsyncRequestHandler<NewsInputModel, Response<Model.News>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public SaveNewsHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Response<Model.News>> Handle(NewsInputModel message)
        {
            var response = new Response<Model.News>();

            Model.News news = null;

            news = message.News.Id > 0
                ? await _mediator.Send(new GetByIdQuery<Model.News>(message.News.Id))
                : new Model.News();

            news.Published = DateTime.UtcNow;
            news.Title = message.News.Title;
            news.Content = message.News.Content;
            news.Link = message.News.Link;
            news.Slug = message.News.Title.GenerateSlug();

            if(news.Id > 0)
                _uow.Update(news);
            else
                _uow.Add(news);


            try
            {
                await _uow.Commit();

                response.Result = news;
            }
            catch(Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }
    }
}