namespace BKind.Web.Model
{
    public abstract class Role : Entity
    {
        public abstract string Name { get; }
    }

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
    }

    public class Reviewer : Role
    {
        public override string Name => "Reviewer";
    }

}