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
    public class ChangeStatusCommandHandler : IAsyncRequestHandler<ChangeStatusCommand, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(ChangeStatusCommand message)
        {
            var response = new Response();

            var story = await _mediator.Send(new GetStoryQuery(message.Slug));

            if (story == null) return response.AddError("", "Story not found");

            if ((message.NewStatus == Status.Published || message.NewStatus == Status.Declined) &&
                !(message.User.Is<Reviewer>() || message.User.Is<Administrator>()))
                return response.AddError("", "Permission needed");

            story.Status = message.NewStatus;

            _unitOfWork.Update(story);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }
    }
}