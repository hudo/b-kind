using System;
using System.Collections.Generic;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Stories.Contracts
{
    public class ListStoriesQuery : IRequest<List<StoryProjection>>
    {
        public User UserWithRoles { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int StoryId { get; set; }
        public int MaxStories { get; set; } = 10;

    }
}
