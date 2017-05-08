using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Features.Stories
{
    public class ListStoriesQuery : IRequest<List<StoryProjection>>
    {
        public bool IncludeUnpublished { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int MaxStories { get; set; } = 10;
        

        public class Handler : IAsyncRequestHandler<ListStoriesQuery, List<StoryProjection>>
        {
            private readonly DbContext _db;

            public Handler(DbContext db)
            {
                _db = db;
            }

            public async Task<List<StoryProjection>> Handle(ListStoriesQuery message)
            {
                IQueryable<Story> query = _db.Set<Story>().AsNoTracking();

                if (!message.IncludeUnpublished)
                    query = query.Where(x => x.Status == Status.Published);

                if (message.AuthorId.HasValue)
                    query = query.Where(x => x.AuthorId == message.AuthorId.Value);

                if (message.FromDate.HasValue)
                    query = query.Where(x => x.Created > message.FromDate.Value);

                if (message.ToDate.HasValue)
                    query = query.Where(x => x.Created < message.ToDate.Value);

                return await query.OrderByDescending(x => x.Created)
                    .Skip(0).Take(message.MaxStories)
                    .Select(x => new StoryProjection
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        Created = x.Created,
                        AuthorId = x.AuthorId,
                        AuthorName = x.Author.FirstName + " " + x.Author.LastName,
                        ThumbsUp = x.ThumbsUp
                    }).ToListAsync();
            }
        }
    }
}