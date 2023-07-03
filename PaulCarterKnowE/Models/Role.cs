namespace PaulCarterKnowE.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual List<User> Users { get; set; }

    }
}
