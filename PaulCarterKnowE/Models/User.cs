﻿namespace PaulCarterKnowE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Group> Groups { get; set; }
    }
}
