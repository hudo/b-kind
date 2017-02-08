using System;
using System.Collections.Generic;
using System.Linq;

namespace BKind.Web.Model
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? LastLogin { get; set; }

        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public bool Is<T>() where T:Role
        {
            return Roles.OfType<T>().Any();
        }

        public T GetRole<T>() where T:Role
        {
            return Roles.OfType<T>().FirstOrDefault();
        }
    }
}