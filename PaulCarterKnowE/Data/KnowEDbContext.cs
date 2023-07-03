using Microsoft.EntityFrameworkCore;
using PaulCarterKnowE.Models;

public class KnowEDbContext : DbContext
{
    public KnowEDbContext(DbContextOptions<KnowEDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}