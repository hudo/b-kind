using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories
{
    public class CreateStoryHandler : IAsyncRequestHandler<CreateStoryInputModel, Response<Story>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateStoryHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<Story>> Handle(CreateStoryInputModel message)
        {
            var response = new Response<Story>();

            var user = await _mediator.Send(new GetOneQuery<User>(x => x.Id == message.UserId));

            if (user == null)
            {
                response.AddError("User", "User not foound");
                return response;
            }

            var story = new Story
            {
                Author = user,
                Title = message.Title,
                Content = message.Content,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            try
            {
                _unitOfWork.Add(story);
                await _unitOfWork.Commit();

                response.Result = story;
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }
    }
}