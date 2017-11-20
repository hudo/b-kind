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
    public class ThumbsUpHandler : IAsyncRequestHandler<ThumbsUpCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ThumbsUpHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response> Handle(ThumbsUpCommand message)
        {
            var response = new Response();

            var story = await _mediator.Send(new GetByIdQuery<Story>(message.StoryId));

            if (story == null)
                return response.AddError("", "Story not found");

            var storyVote = await _mediator.Send(new GetOneQuery<StoryVotes>(x => x.StoryId == message.StoryId && x.UserId == message.LoggedUser.Id));

            if (storyVote != null)
                return response.AddError("", "Can't vote twice");

            story.ThumbsUp++;

            var vote = new StoryVotes { StoryId = message.StoryId, UserId = message.LoggedUser.Id, Voted = DateTime.Now };

            _unitOfWork.Update(story);
            _unitOfWork.Add(vote);

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