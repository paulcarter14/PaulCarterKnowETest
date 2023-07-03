using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaulCarterKnowE.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaulCarterKnowE.Controllers
{
 
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly KnowEDbContext _context;

        public RoleController(KnowEDbContext context)
        {
            _context = context;
        }

        [HttpPut("Change Roles")] 
        public async Task<IActionResult> UpdateUserRole(int userId, string roleName)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
            {
                role = new Role { RoleName = roleName };
                _context.Roles.Add(role);
            }

            user.Role = role;
            await _context.SaveChangesAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(user, options));

        }
    }
}
