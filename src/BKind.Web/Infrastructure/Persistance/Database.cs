using System;
using System.Collections.Generic;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Model;

namespace BKind.Web.Infrastructure.Persistance
{
    public class InMemoryFakeDatabase : IDatabase
    {

        static InMemoryFakeDatabase()
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
        }

        private static readonly List<Story> _stories;
        private static readonly List<User> _users;

        public List<Story> Stories => _stories;
        public List<User> Users => _users;
    }

    public interface IDatabase
    {
        List<Story> Stories { get; }
        List<User> Users { get; }
    }
}