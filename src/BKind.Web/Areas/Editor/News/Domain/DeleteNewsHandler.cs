using System;
using System.Threading.Tasks;
using BKind.Web.Areas.Editor.News.Models;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Areas.Editor.News.Domain
{
    public class DeleteNewsHandler : IAsyncRequestHandler<DeleteNewsCommand, Response>
    {
        private readonly IMediator _bus;
        private readonly IUnitOfWork _uow;

        public DeleteNewsHandler(IMediator bus, IUnitOfWork uow)
        {
            _bus = bus;
            _uow = uow;
        }

        public async Task<Response> Handle(DeleteNewsCommand message)
        {
            var response = new Response();

            var news = await _bus.Send(new GetByIdQuery<Model.News>(message.Id));

            if (news == null)
                return response.AddError("news", "News not found");

            if (!message.LoggedUser.Is<Administrator>())
                return response.AddError("user", "Permission denied");

            try
            {
                _uow.Delete(news);
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