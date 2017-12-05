using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class GetStoryQuery : IRequest<Story>
    {
        public GetStoryQuery(string slug)
        {
            Slug = slug;
        }

        public string Slug { get;  }
    }
}