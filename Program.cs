using EspnBackend.Data;
using EspnBackend.Models;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Database;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EspnBackend.Security;

var builder = WebApplication.CreateBuilder(args);

// Set the port
builder.WebHost.UseUrls("http://localhost:5149");

// Add services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EspnDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

builder.Services.AddSession();

var app = builder.Build();

// Use middleware
app.UseCors();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Serve static files from wwwroot/Frontend/public
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Frontend", "public")),
    RequestPath = ""
});

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Players}/{action=Index}/{id?}");

// ✅ Scoped block for seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EspnDbContext>();

    // Seed AdminUser
    if (!context.AdminUsers.Any())
    {
        var password = "yuki24";
        using var sha256 = SHA256.Create();
        var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

        context.AdminUsers.Add(new AdminUser
        {
            Username = "admin",
            PasswordHash = hash,
        });
        context.SaveChanges();
        Console.WriteLine("Admin user seeded.");
    }

    // ✅ Custom raw SQL seeder
    try
    {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var seeder = new DatabaseSeeder(configuration.GetConnectionString("DefaultConnection"));
        seeder.Seed();
        Console.WriteLine("Custom database seed executed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding error: {ex.Message}");
    }
}

app.Run();