using AgileApp.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileApp
{
    public class AgileDbContext : DbContext
    {
        public DbSet<UserDb>? Users { get; set; }
        public DbSet<TaskDb>? Tasks { get; set; }
        public DbSet<ProjectDb>? Projects { get; set; }
        public DbSet<FileDb>? Files { get; set; }
        public DbSet<Proj_UserDb>? Proj_Users { get; set; }
        public DbSet<MessageDb>? Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=student;Database=AgileDb");
    }
}
