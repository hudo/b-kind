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
        public User UserWithRoles { get; set; }
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

                // admin can see everything
                // reviewer can see everything
                // author can see all his stories and others published stories 
                // visitor/anonymous can see just published stories

                if (message.UserWithRoles != null)
                {
                    if (message.UserWithRoles.Is<Reviewer>())
                        query = query.Where(story => story.Status == Status.Published);
                    else if (message.UserWithRoles.Is<Author>())
                        query = query.Where(story => story.AuthorId == message.UserWithRoles.Id || story.Status == Status.Published);
                    else // visitor
                        query = query.Where(story => story.Status == Status.Published);
                }
                else // anonymous
                    query = query.Where(story => story.Status == Status.Published);
                
                if (message.FromDate.HasValue)
                    query = query.Where(story => story.Created > message.FromDate.Value);

                if (message.ToDate.HasValue)
                    query = query.Where(story => story.Created < message.ToDate.Value);

                return await query.OrderByDescending(story => story.Created)
                    .Skip(0).Take(message.MaxStories)
                    .Select(story => new StoryProjection
                    {
                        Id = story.Id,
                        Title = story.Title,
                        Content = story.Content,
                        Created = story.Created,
                        AuthorId = story.AuthorId,
                        AuthorName = story.Author.User.FirstName + " " + story.Author.User.LastName,
                        ThumbsUp = story.ThumbsUp,
                        Status = story.Status
                    }).ToListAsync();
            }
        }
    }
}