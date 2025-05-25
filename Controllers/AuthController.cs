using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using EspnBackend.DTO;
using EspnBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EspnBackend.Security;





[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly EspnDbContext _context;

    public AuthController(EspnDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] LoginRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return BadRequest(new { success = false, message = "Username and password are required" });
    }

    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
    if (existingUser != null)
    {
        return BadRequest(new { success = false, message = "Username already exists" });
    }

    var newUser = new User
    {
        Username = request.Username,
        PasswordHash = ComputeHash(request.Password),
        Role = "User"
    };

    _context.Users.Add(newUser);
    await _context.SaveChangesAsync();

    return Ok(new { success = true, message = "User created successfully" });
}

    [HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    // First, try to find user in AdminUsers
    var adminUser = _context.AdminUsers.FirstOrDefault(u => u.Username == request.Username);
    var hash = ComputeHash(request.Password);

    if (adminUser != null && hash == adminUser.PasswordHash)
    {
        HttpContext.Session.SetString("AdminUser", adminUser.Username);
        return Ok(new { success = true, role = "Admin", redirectUrl = "/adminDashboard.html" });
    }

    // Then check regular users
    var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
    if (user == null || user.PasswordHash != hash)
    {
        return Unauthorized(new { success = false, message = "Invalid username or password" });
    }

    HttpContext.Session.SetString("User", user.Username);

    // Log the login
    var loginLog = new UserLoginHistory
    {
        UserId = user.Id,
        LoginTime = DateTime.UtcNow
    };

    _context.UserLoginHistories.Add(loginLog);
    _context.SaveChanges();

    return Ok(new { success = true, role = "User", redirectUrl = "/home.html" });
}

    private string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}