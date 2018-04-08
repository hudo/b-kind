﻿using System;
using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class Story : Identity
    {
        public Story() { }

        public Story(string title, string content, string slug, int authorId, Status status)
        {
            Title = title;
            Content = content;
            AuthorId = authorId;
            Status = status;
            Slug = slug;

            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }

        public string Slug { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Status Status { get; set; }
        public int ThumbsUp { get; set; }
        public DateTime Deleted { get; set; }
        public int Views { get; set; }

        public string Photo { get; set; }

        public bool Pinned { get; set; }

        public IList<StoryVotes> Votes { get; set; }

        public IList<StoryTags> StoryTags { get; set; }
    }
}