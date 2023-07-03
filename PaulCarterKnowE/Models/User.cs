namespace PaulCarterKnowE.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public virtual List<Group>? Groups { get; set; }
    }
}
