using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaulCarterKnowE.Models;

namespace PaulCarterKnowE.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly KnowEDbContext dbContext;

        public UserController(KnowEDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //Hämta alla
        [HttpGet("Get All")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Groups)
                .ToListAsync();

            var userDTOs = users.Select(user => new
            {
                UserId = user.UserId,
                UserName = user.UserName,
                RoleTitle = user.Role?.RoleName,
                Groups = user.Groups.Select(group => new
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    Client = group.Client
                }).ToList()
            });

            return Ok(userDTOs);
        }

        //Hämta enskild
        [HttpGet]
        [Route("Get User By Id")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        //Lägg till Úser
        [HttpPost("Add User")]
        public async Task<IActionResult> AddUser(string userName, string roleTitle)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleTitle)
                        ?? new Role { RoleName = roleTitle };

            var user = new User
            {
                UserName = userName,
                Role = role
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            //Hade error med Object Cycle så lade till detta i samråd med ChatGpt Samma när jag försökte ändra en user role
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(user, options));
        }
        // Ändra namn på användare
        [HttpPut]
        [Route("Change Username")]
        public async Task<IActionResult> UpdateUserName([FromRoute] int userId, string userName)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userName;

            await dbContext.SaveChangesAsync();

            return Ok(user);
        }


        [HttpDelete]
        [Route("Delete User")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId, [FromQuery] string password)
        {
            if (password != "DELETE")
            {
                return BadRequest("Invalid password");
            }

            var user = await dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return Ok(user);
        }


        private bool UserExists(int userId)
        {
            return dbContext.Users.Any(u => u.UserId == userId);
        }
    }
}


