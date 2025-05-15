using WeekMap.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WeekMap
{
    public partial class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // ? REGISTER AppDbContext properly
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor(); // optional but useful

            // register CORS policy
            builder.Services.AddCors(options => { options.AddPolicy("AllowLocalhost3000", policy => policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()); });
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            var app = builder.Build();
            // ENABLE CORS BEFORE routing/middleware
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

