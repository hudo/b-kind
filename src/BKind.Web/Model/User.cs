using System;
using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual IEnumerable<Role> Roles { get; set; }
        public virtual Credential Credential { get; set; }
        public DateTime Registered { get; set; }
        public DateTime LastLogin { get; set; }
    }
}