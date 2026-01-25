using WeekMap.Data;
using Microsoft.EntityFrameworkCore;
using WeekMap.Services.ActivityCategory;
using WeekMap.Services.ActivityTemplate;
using WeekMap.Services.User;
using WeekMap.Services.UserSettings;
using WeekMap.Services.UserDefaultWeekMapSettings;
using WeekMap.Services.WeekMap;
using WeekMap.Services.WeekMapActivity;

namespace WeekMap
{
    public partial class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(options => { options.AddPolicy("AllowLocalhost3000", 
                policy => policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()); });
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
            builder.Services.AddScoped<IUserDefaultWeekMapSettingsService, UserDefaultWeekMapSettingsService>();
            builder.Services.AddScoped<IActivityCategoryService, ActivityCategoryService>();
            builder.Services.AddScoped<IActivityTemplateService, ActivityTemplateService>();
            builder.Services.AddScoped<IWeekMapService, WeekMapService>();
            builder.Services.AddScoped<IWeekMapActivityService, WeekMapActivityService>();

            var app = builder.Build();

            app.UseCors("AllowLocalhost3000");

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                //context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}/");

            app.Run();
        }
    }
}

