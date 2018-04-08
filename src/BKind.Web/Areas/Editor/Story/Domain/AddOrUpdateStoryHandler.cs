using System;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Areas.Editor.Story.Models;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;
using BKind.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Areas.Editor.Story.Domain
{
    public class AddOrUpdateStoryHandler : IAsyncRequestHandler<AddOrUpdateStoryInputModel, Response<Model.Story>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;
        private readonly ILogger<AddOrUpdateStoryHandler> _logger;

        public AddOrUpdateStoryHandler(IUnitOfWork unitOfWork, 
            IMediator mediator, 
            IStorageService storageService,
            ILogger<AddOrUpdateStoryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _storageService = storageService;
            _logger = logger;
        }

        public async Task<Response<Model.Story>> Handle(AddOrUpdateStoryInputModel message)
        {
            var response = new Response<Model.Story>();

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

                await _unitOfWork.CommitAsync();
            }
            else author = user.GetRole<Author>();

            Model.Story story;
            if (message.StoryId.HasValue)
            {
                story = await GetAndUpdateAsync(message);

                if (!user.Is<Administrator>())
                    story.Status = Status.Draft;
            }
            else
            {
                story = author.CreateNewStory(message.StoryTitle, message.Content);
            }

            story.Modified = DateTime.UtcNow;

            if (message.StoryId.HasValue)
                _unitOfWork.Update(story);
            else
                _unitOfWork.Add(story);

            try
            {
                await _unitOfWork.CommitAsync();
                
                await UpdateTags(message, story);

                response.Result = story;

                var fileName = $"{story.Id}-{message.Image?.FileName}";

                if(await TryUploadPhoto(message.Image, fileName, response))
                {
                    if (!string.IsNullOrEmpty(story.Photo))
                        await _storageService.DeleteAsync(story.Photo, ContentType.Story);

                    story.Photo = fileName;

                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception e)
            {
                response.AddError("", e.Unwrap().Message);
                _logger.LogError("Error saving story: " + e.Unwrap().Message, e);
            }

            return response;
        }

        private async Task<bool> TryUploadPhoto(IFormFile formFile, string fileName, Response response)
        {
            try
            {
                if(formFile == null || formFile.Length == 0)
                    return false;

                await _storageService.UploadAsync(formFile.OpenReadStream(), fileName, ContentType.Story);

                return true;
            }
            catch(Exception e)
            {
                response.AddError("Image", $"Couldn't upload image: {e.Unwrap().Message}");
                _logger.LogError("Error uploading photo: " + e.Unwrap().Message, e);
            }

            return false;
        }

        public async Task DeletePhoto(string fileName)
        {

        }

        private async Task UpdateTags(AddOrUpdateStoryInputModel message, Model.Story story)
        {
            var storyTags = await _mediator.Send(new GetAllQuery<StoryTags>(x => x.StoryId == story.Id));

            foreach (var tag in storyTags)
            {
                _unitOfWork.Delete(tag);
            }

            await _unitOfWork.CommitAsync();
            
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

                    await _unitOfWork.CommitAsync();
                }

                _unitOfWork.Add(new StoryTags { TagId = tagDb.Id, StoryId = story.Id });
            }

            await _unitOfWork.CommitAsync();
        }

        private async Task<Model.Story> GetAndUpdateAsync(AddOrUpdateStoryInputModel message)
        {
            var story = await _mediator.Send(new GetByIdQuery<Model.Story>(message.StoryId.Value));
            story.Title = message.StoryTitle;
            story.Content = message.Content;
            return story;
        }
    }
}