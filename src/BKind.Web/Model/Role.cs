namespace BKind.Web.Model
{
    public abstract class Role : Entity { }

    public class Visitor : Role { }
    public class Administrator : Role { }
    public class Author : Role { }
    public class Reviewer : Role { }

}