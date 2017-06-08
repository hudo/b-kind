using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Domain
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

            var story = await _mediator.Send(new GetByIdQuery<Story>(message.StoryId));

            if (story == null) return response.AddError("", "Story not found");

            if ((message.NewStatus == Status.Published || message.NewStatus == Status.Declined) &&
                (!message.User.Is<Reviewer>() || !message.User.Is<Administrator>()))
                return response.AddError("", "Permission needed");

            story.Status = message.NewStatus;

            _unitOfWork.Update(story);

            try
            {
                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }
    }
}