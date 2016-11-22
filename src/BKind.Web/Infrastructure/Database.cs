using System;
using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.Infrastructure
{
    public class Database : IDatabase
    {

        static Database()
        {
            _stories = new List<Story>
            {
                new Story
                {
                    ID = 1,
                    Author = new User
                    {
                        ID = 1,
                        FirstName = "Bob", 
                        LastName = "Rock"
                    },
                    Content = "story content",
                    Created = DateTime.Now.AddDays(-5),
                    Title = "Sample story",
                    Status = Status.Published
                }
            };
        }

        private static readonly List<Story> _stories;

        public IEnumerable<Story> Stories => _stories;
    }

    public interface IDatabase
    {
        IEnumerable<Story> Stories { get; }
    }
}