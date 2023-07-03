using PaulCarterKnowE.Models;

public class SampleData
{
    public static void Create(KnowEDbContext database)
    {

        if (database.Users.Count() == 0)
        {
            var roles = new List<Role>
    {
        new Role { RoleName = "Manager" },
        new Role { RoleName = "Web Designer" },
        new Role { RoleName = "Programmer" },
        new Role { RoleName = "Project Manager" }
    };

            var groups = new List<Group>
    {
        new Group { GroupName = "Development Team", Client = "ABC Company", Users = new List<User>() },
        new Group { GroupName = "Design Team", Client = "XYZ Agency", Users = new List<User>() },
        new Group { GroupName = "Testing Team", Client = "123 Corporation", Users = new List<User>() },
        new Group { GroupName = "Support Team", Client = "Tech Solutions", Users = new List<User>() }
    };

            var users = new List<User>
    {
        new User { UserName = "John Smith", Role = roles[0], Groups = new List<Group> { groups[0], groups[3] } },
        new User { UserName = "Emily Johnson", Role = roles[1], Groups = new List<Group> { groups[1], groups[2] } },
        new User { UserName = "David Lee", Role = roles[2], Groups = new List<Group> { groups[0] } },
        new User { UserName = "Jessica Brown", Role = roles[2], Groups = new List<Group> { groups[2] } },
        new User { UserName = "Jennifer Davis", Role = roles[3], Groups = new List<Group> { groups[3] } },
        new User { UserName = "Michael Wilson", Role = roles[2], Groups = new List<Group> { groups[0] } },
        new User { UserName = "Sophia Thompson", Role = roles[1], Groups = new List<Group> { groups[1] } },
        new User { UserName = "Matthew Davis", Role = roles[2], Groups = new List<Group> { groups[2] } },
        new User { UserName = "Olivia Johnson", Role = roles[1], Groups = new List<Group> { groups[2] } },
        new User { UserName = "Daniel Smith", Role = roles[2], Groups = new List<Group> { groups[3] } }
    };

            database.Roles.AddRange(roles);
            database.Groups.AddRange(groups);
            database.Users.AddRange(users);

            database.SaveChanges();
        }

    }
}

