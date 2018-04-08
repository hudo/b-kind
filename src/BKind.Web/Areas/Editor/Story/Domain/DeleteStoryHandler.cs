using System;
using System.Threading.Tasks;
using BKind.Web.Areas.Editor.Story.Models;
using BKind.Web.Core;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Areas.Editor.Story.Domain
{
    public class DeleteStoryHandler : IAsyncRequestHandler<DeleteStoryCommand, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public DeleteStoryHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        public async Task<Response> Handle(DeleteStoryCommand message)
        {
            var response = new Response();

            if (!message.UserWithRoles.Is<Administrator>())
                return response.AddError("user", "Only admin can delete story permanetly");

            var story = await _mediator.Send(new GetStoryQuery(message.Slug));

            if (story == null)
                return response.AddError("story", "Story not found");

            _uow.Delete(story);

            try
            {
                await _uow.CommitAsync();
            }
            catch (Exception e)
            {
                response.AddError("story", $"Error deleting the story: {e.Unwrap().Message}");
            }

            return response;
        }
    }
}