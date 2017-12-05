using System.Linq;
using BKind.Web.Model;
using BKind.Web.Infrastructure.Helpers;
using System.Collections.Generic;
using System;

namespace BKind.Web.Infrastructure.Persistance
{
    public static class StoriesDbContextExtensions
    {
        public static void EnsureDataSeed(this StoriesDbContext db)
        {
            if (!db.Set<Administrator>().Any())
            {
                var user = new User()
                {
                    FirstName = "Admin",
                    Nickname = "Admin",
                    Username = "admin",
                    PasswordHash = StringHelpers.ComputeHash("12345", "12345"),
                    Roles = new List<Role> { new Administrator() },
                    Salt = "12345",
                    Registered = DateTime.UtcNow
                };

                db.Set<User>().Add(user);

                db.SaveChanges();
            }
        }
    }
}