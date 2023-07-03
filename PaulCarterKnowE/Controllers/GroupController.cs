using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaulCarterKnowE.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

[ApiController]
public class GroupController : ControllerBase
{
    private readonly KnowEDbContext _context;

    public GroupController(KnowEDbContext context)
    {
        _context = context;
    }

    [HttpPost("Create Group")]
    public async Task<IActionResult> CreateGroup(string groupName, string client)
    {
        Group group = new Group
        {
            GroupName = groupName,
            Client = client
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(group);
    }


    [HttpPost("Place User In Group")]
    public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
    {
        var group = await _context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.GroupId == groupId);
        var user = await _context.Users.FindAsync(userId);

        if (group == null || user == null)
        {
            return NotFound();
        }

        group.Users.Add(user);
        await _context.SaveChangesAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        return Ok(JsonSerializer.Serialize(group, options));
    }

    [HttpDelete("Remove User By Id From Group By Id")]
    public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId, string password)
    {
        if (password != "DELETE")
        {
            return BadRequest("Invalid password");
        }

        var group = await _context.Groups
            .Include(g => g.Users)
            .FirstOrDefaultAsync(g => g.GroupId == groupId);

        var user = await _context.Users.FindAsync(userId);

        if (group == null || user == null)
        {
            return NotFound();
        }

        group.Users.Remove(user);
        await _context.SaveChangesAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        return Ok(JsonSerializer.Serialize(group, options));
    }

    [HttpDelete("Delete Group")]
    public async Task<IActionResult> DeleteGroup(int groupId, string password)
    {
        if (password != "DELETE")
        {
            return BadRequest("Invalid password");
        }

        var group = await _context.Groups.FindAsync(groupId);

        if (group == null)
        {
            return NotFound();
        }

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return Ok(group);
    }


    [HttpGet("Show Group By Name Search")]
    public async Task<IActionResult> GetGroupByName(string groupName)
    {
        var group = await _context.Groups
            .Include(g => g.Users)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(g => g.GroupName == groupName);

        if (group == null)
        {
            return NotFound();
        }

        var groupWithMembers = new
        {
            Id = group.GroupId,
            Name = group.GroupName,
            Members = group.Users.Select(u => new
            {
                UserName = u.UserName,
                Role = u.Role.RoleName
            }).ToList()
        };

        return Ok(groupWithMembers);
    }



    [HttpGet("Group Info")]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _context.Groups
            .Include(g => g.Users)
            .ThenInclude(u => u.Role)
            .ToListAsync();

        var groupsWithMembers = groups.Select(g => new
        {
            Id = g.GroupId,
            Name = g.GroupName,
            Members = g.Users.Select(u => new
            {
                UserName = u.UserName,
                Role = u.Role.RoleName
            }).ToList()
        }).ToList();

        return Ok(groupsWithMembers);
    }


}
