using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<CalendarTask> CalendarTasks { get; set; }
        public DbSet<RepetitiveTask> RepetitiveTasks { get; set; }
        public DbSet<NoDeadlineTask> NoDeadlineTasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // like the unique constraint in the SSMS database
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Table-per-type inheritance
            modelBuilder.Entity<CalendarTask>().ToTable("CalendarTasks");
            modelBuilder.Entity<RepetitiveTask>().ToTable("RepetitiveTasks");
            modelBuilder.Entity<NoDeadlineTask>().ToTable("NoDeadlineTasks");

            base.OnModelCreating(modelBuilder);
        }
    }
}
