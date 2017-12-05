using System;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Domain
{
    public class AddOrUpdateStoryHandler : IAsyncRequestHandler<AddOrUpdateStoryInputModel, Response<Story>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AddOrUpdateStoryHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<Story>> Handle(AddOrUpdateStoryInputModel message)
        {
            var response = new Response<Story>();

            var user = await _mediator.Send(new GetOneQuery<User>(x => x.Id == message.UserId, u => u.Roles));

            if (user == null)
            {
                response.AddError("User", "User not foound");
                return response;
            }

            Author author;

            if (!user.Is<Author>())
            {
                author = new Author();
                user.Roles.Add(author);

                _unitOfWork.Update(user);

                await _unitOfWork.Commit();
            }
            else author = user.GetRole<Author>();

            var story = message.StoryId.HasValue
                ? await GetAndUpdateAsync(message)
                : author.CreateNewStory(message.StoryTitle, message.Content);

            story.Modified = DateTime.UtcNow;

            if (message.StoryId.HasValue)
                _unitOfWork.Update(story);
            else
                _unitOfWork.Add(story);

            try
            {
                await _unitOfWork.Commit();
                
                await UpdateTags(message, story);

                response.Result = story;
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
            }

            return response;
        }

        private async Task UpdateTags(AddOrUpdateStoryInputModel message, Story story)
        {
            var storyTags = await _mediator.Send(new GetAllQuery<StoryTags>(x => x.StoryId == story.Id));

            foreach (var tag in storyTags)
            {
                _unitOfWork.Delete(tag);
            }

            await _unitOfWork.Commit();
            
            if(string.IsNullOrEmpty(message.Tags)) return;

            var uiTags = message.Tags
                .Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLower())
                .ToArray();

            var existingTags = await _mediator.Send(new GetAllQuery<Tag>(x => uiTags.Contains(x.Title)));

            foreach (var uiTag in uiTags)
            {
                var tagDb = existingTags.FirstOrDefault(x => x.Title == uiTag);

                if (tagDb == null)
                {
                    tagDb = new Tag { Title = uiTag };
                    
                    _unitOfWork.Add(tagDb);

                    await _unitOfWork.Commit();
                }

                _unitOfWork.Add(new StoryTags { TagId = tagDb.Id, StoryId = story.Id });
            }

            await _unitOfWork.Commit();
        }

        private async Task<Story> GetAndUpdateAsync(AddOrUpdateStoryInputModel message)
        {
            var story = await _mediator.Send(new GetByIdQuery<Story>(message.StoryId.Value));
            story.Title = message.StoryTitle;
            story.Content = message.Content;
            return story;
        }
    }
}