using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaulCarterKnowE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly YourDbContext _context;

    public UserController(YourDbContext context)
    {
        _context = context;
    }

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Groups)
            .ToListAsync();
    }

    // GET: api/user/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Groups)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // GET: api/user/search?term={searchTerm}
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string term)
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Groups)
            .Where(u => u.Name.Contains(term) ||
                        u.Groups.Any(g => g.Name.Contains(term)) ||
                        u.Role.Name.Contains(term))
            .ToListAsync();

        return users;
    }

    // POST: api/user
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/user/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/user/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Add your validation logic here before deleting the user
        // For example, check if the user is allowed to be deleted

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
}
