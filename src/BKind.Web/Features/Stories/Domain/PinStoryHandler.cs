using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Stories.Commands;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Domain
{
    public class PinStoryHandler : IAsyncRequestHandler<PinStoryCommand, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public PinStoryHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }
        
        public async Task<Response> Handle(PinStoryCommand message)
        {
            var response = new Response();
            
            if (!message.UserWithRoles.Is<Administrator>())
                return response.AddError("", "Only admin can pin the story");

            var story = await _mediator.Send(new GetStoryQuery(message.Slug));

            if (story == null)
                return response.AddError("", $"Story with slug {message.Slug} not found");

            story.Pinned = !story.Pinned;
            
            _uow.Update(story);

            await _uow.Commit();

            return response;
        }
    }
}