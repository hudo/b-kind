using System;
using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class Story : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Status Status { get; set; }
        public int ThumbsUp { get; set; }
        public DateTime Deleted { get; set; }
    }


    public enum Status
    {
        Draft, Published, Declined
    }

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

    public abstract class Role : Entity { }

    public class Visitor : Role { }
    public class Administrator : Role { }
    public class Author : Role { }
    public class Reviwer : Role { }

    public class Credential : Entity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }

    public abstract class Entity
    {
        public int ID { get; set; }
    }
}