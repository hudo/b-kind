namespace BKind.Web.Model
{
    public class Credential : Entity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}