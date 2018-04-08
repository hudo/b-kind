using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Models;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Domain
{
    public class IncreaseStoryViewCountHandler : IAsyncRequestHandler<IncreaseStoryViewCountCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public IncreaseStoryViewCountHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response> Handle(IncreaseStoryViewCountCommand message)
        {
            var response = new Response();

            var story = await _mediator.Send(new GetByIdQuery<Story>(message.StoryId));

            if (story == null)
                return response.AddError("", $"Story {message.StoryId} not found");

            story.Views++;

            _unitOfWork.Update(story);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch(Exception e)
            {
                response.AddError("", $"Problem with updating story view count {e.Unwrap().Message}");
            }

            return response;
        }
    }
}