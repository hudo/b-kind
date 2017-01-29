using System;
using System.Collections.Generic;
using System.Linq;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;

namespace BKind.Web.Infrastructure.Persistance
{
    public class Database : IDatabase
    {

        static Database()
        {
            _users = new List<User>
            {
                new User
                {
                    ID = 1,
                    Username = "bobrock",
                    FirstName = "Bob",
                    LastName = "Rock",
                    Credential = new Credential
                    {
                        ID = 1,
                        Username = "bobrock",
                        PasswordHash = StringHelpers.ComputeHash("1234", "123"),
                        Salt = "123"
                    }
                }
            };

            _stories = new List<Story>
            {
                new Story
                {
                    ID = 1,
                    Author = _users.First(),
                    Content = "story content",
                    Created = DateTime.Now.AddDays(-5),
                    Title = "Sample story",
                    Status = Status.Published
                }
            };
        }

        private static readonly List<Story> _stories;

        private static readonly List<User> _users;

        public IEnumerable<Story> Stories => _stories;
        public IEnumerable<User> Users => _users;
    }

    public interface IDatabase
    {
        IEnumerable<Story> Stories { get; }
        IEnumerable<User> Users { get; }
    }
}