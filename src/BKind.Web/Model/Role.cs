using System.Collections.Generic;

namespace BKind.Web.Model
{
    public abstract class Role : Entity
    {
        public abstract string Name { get; }

        public virtual User User { get; set; }
        public int UserId { get; set; }
    }

    // each role can have some additional properties and methods
    // we can then use role domain entity to perform specific action permitted just
    // for that role. 

    public class Visitor : Role
    {
        public override string Name => "Visitor";
    }

    public class Administrator : Role
    {
        public override string Name => "Admin";
    }

    public class Author : Role
    {
        public override string Name => "Author";
        public virtual ICollection<Story> Stories { get; set; }

        public Story CreateNewStory(string title, string content)
        {
            return new Story(title, content, this.Id, Status.Draft);
        }
    }

    public class Reviewer : Role
    {
        public override string Name => "Reviewer";
    }

}