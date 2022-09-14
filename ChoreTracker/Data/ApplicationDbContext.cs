using ChoreTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChoreTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Chore> Chores { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AssignedChore> AssignedChores { get; set; } = null!;
        public DbSet<CompletedChore> CompletedChores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=HE-MAN;Initial Catalog=ChoreTracker2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

    }
}