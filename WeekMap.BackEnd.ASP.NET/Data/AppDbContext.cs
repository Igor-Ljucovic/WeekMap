using Microsoft.EntityFrameworkCore;
using Models = WeekMap.Models;
using WeekMap.Models;

namespace WeekMap.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDefaultWeekMapSettings> UserDefaultWeekMapSettings { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<ActivityCategory> ActivityCategories { get; set; }
        public DbSet<ActivityTemplate> ActivityTemplates { get; set; }
        public DbSet<Models.WeekMap> WeekMaps { get; set; }
        public DbSet<WeekMapActivity> WeekMapActivities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            //modelBuilder.Entity<RealisedWeekMap>().HasOne(r => r.PlannedWeekMap)
            //.WithMany(p => p.RealisedWeekMaps)
            //.HasForeignKey(r => r.WeekMapID);

            // constraints:
            // when the HasForeignKey ROW gets deleted, what happens to one in the Entity ROW (look from bottom up)
            // when PlannedWeekMap is deleted, it deletes all the RealisedWeekMap rows with that WeekMapID (not the other way around!)

            modelBuilder.Entity<ActivityCategory>()
                .HasOne(ac => ac.User)
                .WithMany(u => u.ActivityCategories)
                .HasForeignKey(ac => ac.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // this constraint can't be set to cascade because of cascading ambiguity
            // (doesn't allow multiple cascading paths that could potentially "overlap" and cause ambiguity during deletion)
            // (Two or more CASCADE constraints could be triggered by deleting the same parent row)
            modelBuilder.Entity<ActivityTemplate>()
                .HasOne(a => a.User)
                .WithMany(u => u.ActivityTemplates)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityTemplate>()
                .HasOne(a => a.ActivityCategory)
                .WithMany()
                .HasForeignKey(a => a.ActivityCategoryID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Models.WeekMap>()
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

            modelBuilder.Entity<WeekMapActivity>()
                .HasOne(p => p.WeekMap)
                .WithMany()
                .HasForeignKey(p => p.WeekMapID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeekMapActivity>()
                .HasOne(p => p.ActivityTemplate)
                .WithMany()
                .HasForeignKey(p => p.ActivityTemplateID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeekMapActivity>()
                .HasKey(p => new { p.WeekMapActivityID} );

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserDefaultWeekMapSettings>().ToTable("UserDefaultWeekMapSettings");
            modelBuilder.Entity<UserSettings>().ToTable("UserSettings");
            modelBuilder.Entity<ActivityCategory>().ToTable("ActivityCategories");
            modelBuilder.Entity<ActivityTemplate>().ToTable("ActivityTemplates");
            modelBuilder.Entity<Models.WeekMap>().ToTable("WeekMaps");
            modelBuilder.Entity<WeekMapActivity>().ToTable("WeekMapActivities");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
