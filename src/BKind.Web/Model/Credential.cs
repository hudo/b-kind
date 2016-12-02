namespace BKind.Web.Model
{
    public class Credential : Entity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}