namespace BKind.Web.Model
{

    public abstract class Entity
    {
        
    }

    public abstract class Identity : Entity
    {
        public int Id { get; set; }
    }
}