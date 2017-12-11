using System;
using System.Threading.Tasks;
using BKind.Web.Features.Stories.Queries;
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
            if (string.IsNullOrEmpty(message.Slug))
                throw new ArgumentNullException("Slug","Story slug missing");
            return await _db.Set<Story>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == message.Slug.ToLower());
        }
    }
}