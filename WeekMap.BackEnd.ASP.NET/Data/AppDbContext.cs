using Microsoft.EntityFrameworkCore;
using WeekMap.Models;

namespace WeekMap.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDefaultWeekMapSettings> UserDefaultWeekMapSettings { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<ActivityCategory> ActivityCategories { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<PlannedWeekMap> PlannedWeekMaps { get; set; }
        public DbSet<PlannedWeekMapActivity> PlannedWeekMapActivities { get; set; }
        public DbSet<RealisedWeekMap> RealisedWeekMaps { get; set; }
        public DbSet<RealisedWeekMapActivity> RealisedWeekMapActivities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // composite keys
            modelBuilder.Entity<PlannedWeekMapActivity>().HasKey(p => new 
            {
                p.PlannedWeekMapID,
                p.ActivityID,
                p.PlannedWeekMapActivityID
            });
            modelBuilder.Entity<RealisedWeekMapActivity>().HasKey(p => new
            {
                p.RealisedWeekMapID,
                p.ActivityID,
                p.RealisedWeekMapActivityID
            });

            //modelBuilder.Entity<RealisedWeekMap>().HasOne(r => r.PlannedWeekMap)
            //.WithMany(p => p.RealisedWeekMaps)
            //.HasForeignKey(r => r.PlannedWeekMapID);

            // constraints:
            // when the HasForeignKey ROW gets deleted, what happens to one in the Entity ROW (look from bottom up)
            // when PlannedWeekMap is deleted, it deletes all the RealisedWeekMap rows with that PlannedWeekMapID (not the other way around!)
            modelBuilder.Entity<RealisedWeekMap>()
                .HasOne(r => r.PlannedWeekMap)
                .WithMany(p => p.RealisedWeekMaps)
                .HasForeignKey(r => r.PlannedWeekMapID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityCategory>()
                .HasOne(ac => ac.User)
                .WithMany(u => u.ActivityCategories)
                .HasForeignKey(ac => ac.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // this constraint can't be set to cascade because of cascading ambiguity
            // (doesn't allow multiple cascading paths that could potentially "overlap" and cause ambiguity during deletion)
            // (Two or more CASCADE constraints could be triggered by deleting the same parent row)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.ActivityCategory)
                .WithMany()
                .HasForeignKey(a => a.ActivityCategoryID)
                .IsRequired(false)   // This tells EF it's OPTIONAL
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlannedWeekMap>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserSettings)
                .WithOne(us => us.User)
                .HasForeignKey<UserSettings>(us => us.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDefaultWeekMapSettings)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDefaultWeekMapSettings>(ud => ud.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannedWeekMapActivity>()
                .HasOne(p => p.PlannedWeekMap)
                .WithMany()
                .HasForeignKey(p => p.PlannedWeekMapID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannedWeekMapActivity>()
                .HasOne(p => p.Activity)
                .WithMany()
                .HasForeignKey(p => p.ActivityID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RealisedWeekMapActivity>()
                .HasOne(r => r.RealisedWeekMap)
                .WithMany()
                .HasForeignKey(r => r.RealisedWeekMapID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RealisedWeekMapActivity>()
                .HasOne(r => r.Activity)
                .WithMany()
                .HasForeignKey(r => r.ActivityID)
                .OnDelete(DeleteBehavior.Cascade);

            // class to database table mapping
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserDefaultWeekMapSettings>().ToTable("UserDefaultWeekMapSettings");
            modelBuilder.Entity<UserSettings>().ToTable("UserSettings");
            modelBuilder.Entity<ActivityCategory>().ToTable("ActivityCategories");
            modelBuilder.Entity<Activity>().ToTable("Activities");
            modelBuilder.Entity<PlannedWeekMap>().ToTable("PlannedWeekMaps");
            modelBuilder.Entity<PlannedWeekMapActivity>().ToTable("PlannedWeekMapActivities");
            modelBuilder.Entity<RealisedWeekMap>().ToTable("RealisedWeekMaps");
            modelBuilder.Entity<RealisedWeekMapActivity>().ToTable("RealisedWeekMapActivities");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
