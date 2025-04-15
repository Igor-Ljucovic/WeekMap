using WebApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ? REGISTER AppDbContext properly
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// refreshing instead of having to restart the app to see changes
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); // optional but useful

// register CORS policy
builder.Services.AddCors(options => { options.AddPolicy("AllowReactApp", policy => policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()); });

var app = builder.Build();
// ENABLE CORS BEFORE routing/middleware
app.UseCors("AllowReactApp");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/");

app.Run();
