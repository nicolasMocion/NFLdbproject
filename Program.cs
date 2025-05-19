using EspnBackend.Data;
using EspnBackend.Models;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Database;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5149");

// Add services to the container

// Enable CORS (adjust origins for production)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add controllers with views (for Razor Pages / MVC)
builder.Services.AddControllersWithViews();

// Configure DbContext with MySQL connection
builder.Services.AddDbContext<EspnDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

var app = builder.Build();

// Use CORS middleware
app.UseCors();

// Use routing middleware
app.UseRouting();

// Serve static files from wwwroot/Frontend/public as root
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Frontend", "public")),
    RequestPath = ""  // Serve at root URL
});

// Authorization middleware (only if you use authentication/authorization)
app.UseAuthorization();

app.MapControllers();

// Map controller routes (important for MVC to work)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Players}/{action=Index}/{id?}");

// Seed the database before app runs
try
{
    var seeder = new DatabaseSeeder(builder.Configuration.GetConnectionString("DefaultConnection"));
    seeder.Seed();
}
catch (Exception ex)
{
    Console.WriteLine($"Seeding error: {ex.Message}");
    // You could log or handle exceptions here if needed
}

// Run the app
app.Run();
