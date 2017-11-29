using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.QueryHandlers
{
    public class GetTagCloudHandler : IAsyncRequestHandler<GetTagCloudQuery, List<(string tag, int count)>>
    {
        private readonly DbContext _db;

        public GetTagCloudHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<List<(string tag, int count)>> Handle(GetTagCloudQuery message)
        {
            // ef does not support projections to DTO and GROUP BY, so lets just pull everything
            // into memory, there will not be many tags probably

            var storyTags = await _db.Set<StoryTags>().AsNoTracking().Include(x => x.Tag).ToListAsync();

            var tagCloud = storyTags
                .GroupBy(x => x.TagId)
                .Select(x => (tag: x.First().Tag.Title, count: x.Count()))
                .ToList();

            return tagCloud;
        }
    }
}