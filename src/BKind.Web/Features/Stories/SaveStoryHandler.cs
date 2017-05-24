using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories
{
    public class SaveStoryHandler : IAsyncRequestHandler<SaveStoryInputModel, Response<Story>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public SaveStoryHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<Story>> Handle(SaveStoryInputModel message)
        {
            var response = new Response<Story>();

            var user = await _mediator.Send(new GetOneQuery<User>(x => x.Id == message.UserId, u => u.Roles));

            if (user == null)
            {
                response.AddError("User", "User not foound");
                return response;
            }

            var story = message.StoryId.HasValue
                ? await _mediator.Send(new GetByIdQuery<Story>(message.StoryId.Value))
                : new Story
                {
                    AuthorId = user.Id,
                    Title = message.StoryTitle,
                    Content = message.Content,
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow
                };

            if (user.Is<Visitor>())
            {
                user.Roles.Add(new Author());
                _unitOfWork.Update(user);
            }

            try
            {
                if(message.StoryId.HasValue)
                    _unitOfWork.Update(story);
                else
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