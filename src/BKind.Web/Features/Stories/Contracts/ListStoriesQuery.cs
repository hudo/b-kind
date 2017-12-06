using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ListStoriesQuery : IRequest<List<StoryProjection>>
    {
        public ListStoriesQuery()
        {
            Paging = new PagedOptions<Story>(1, 10, s => s.Created, false);
        }

        public User UserWithRoles { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StorySlug { get; set; }
        public bool? Pinned { get; set; }
        public bool IncludeTags { get; set; }
        public PagedOptions<Story> Paging { get; set; }
    }
}
