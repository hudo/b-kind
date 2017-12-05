using System.Threading.Tasks;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.QueryHandlers
{
    public class GetStoryQueryHandler : IAsyncRequestHandler<GetStoryQuery, Story>
    {
        private readonly DbContext _db;

        public GetStoryQueryHandler(DbContext db)
        {
            _db = db;
        }
        
        public async Task<Story> Handle(GetStoryQuery message)
        {
            return await _db.Set<Story>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == message.Slug.ToLower());
        }
    }
}