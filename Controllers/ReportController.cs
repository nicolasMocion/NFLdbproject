using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Data;
using EspnBackend.Security;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly EspnDbContext _context;

    public ReportsController(EspnDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersReport()
    {
        var users = await _context.Users
            .Select(u => new {
                Name = u.Username
            })
            .ToListAsync();

        return Ok(users);
    }

    // New login history report
    [HttpGet("login-history")]
public async Task<IActionResult> GetLoginHistory()
{
    var history = await _context.UserLoginHistories
        .Include(h => h.User)
        .Select(h => new
        {
            Username = h.User != null ? h.User.Username : "Unknown",
            LoginTime = h.LoginTime
        })
        .OrderByDescending(h => h.LoginTime)
        .ToListAsync();

    return Ok(history);
}
}