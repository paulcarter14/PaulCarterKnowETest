namespace PaulCarterKnowE.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Client { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
