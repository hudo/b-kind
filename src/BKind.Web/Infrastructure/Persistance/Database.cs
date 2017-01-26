using System;
using System.Collections.Generic;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;

namespace BKind.Web.Infrastructure.Persistance
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
                },
                new Story
                {
                    ID = 2,
                    Author = new User
                    {
                        ID = 1,
                        FirstName = "Bob",
                        LastName = "Rock"
                    },
                    Content = "story content 2",
                    Created = DateTime.Now.AddDays(-5),
                    Title = "Sample story 2",
                    Status = Status.Published
                }
            };

            _users = new List<User>
            {
                new User
                {
                    ID = 1,
                    FirstName = "Bob",
                    LastName = "Rock",
                    Username = "bobrock",
                    Credential = new Credential
                    {
                        ID = 1,
                        Username = "bobrock",
                        PasswordHash = StringHelpers.ComputeHash("1234", "salt"),
                        Salt = "salt"
                    },
                    Roles = new List<Role> { new Visitor(), new Author() }
                }
            };

            _credentials = new List<Credential> { _users[0].Credential };
        }

        private static readonly List<Story> _stories;
        private static readonly List<User> _users;
        private static readonly List<Credential> _credentials;

        public IEnumerable<Story> Stories => _stories;
        public IEnumerable<User> Users => _users;
        public IEnumerable<Credential> Credentials => _credentials;
    }

    public interface IDatabase
    {
        IEnumerable<Story> Stories { get; }
        IEnumerable<User> Users { get; }
        IEnumerable<Credential> Credentials { get; }
    }
}