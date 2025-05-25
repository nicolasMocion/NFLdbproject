using Microsoft.AspNetCore.Mvc;
using EspnBackend.Models; // Adjust if your models are elsewhere
using EspnBackend.Data;
using EspnBackend.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class AdminGamesController : ControllerBase
{
    private readonly EspnDbContext _context;

    public AdminGamesController(EspnDbContext context)
    {
        _context = context;
    }

    // GET: api/AdminGames
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NFLGame>>> GetGames()
    {
        return await _context.NFLGames
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .Include(g => g.Season)
            .ToListAsync();
    }

    // GET: api/AdminGames/5
    [HttpGet("{id}")]
    public async Task<ActionResult<NFLGame>> GetGame(int id)
    {
        var game = await _context.NFLGames
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .Include(g => g.Season)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return NotFound();

        return game;
    }

    // POST: api/AdminGames
    [HttpPost("dto")]
    public async Task<ActionResult<NFLGame>> CreateGame(AdminGameEditDto dto)
    {
        var game = new NFLGame
        {
            GameDate = dto.GameDate,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            HomeScore = dto.HomeScore,
            AwayScore = dto.AwayScore,
            SeasonId = dto.SeasonId
        };

        _context.NFLGames.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
    }

    [HttpPut("dto/{id}")]
public async Task<IActionResult> UpdateGame(int id, AdminGameEditDto dto)
{
    var existingGame = await _context.NFLGames.FindAsync(id);
    if (existingGame == null)
        return NotFound();

    existingGame.GameDate = dto.GameDate;
    existingGame.HomeTeamId = dto.HomeTeamId;
    existingGame.AwayTeamId = dto.AwayTeamId;
    existingGame.HomeScore = dto.HomeScore;
    existingGame.AwayScore = dto.AwayScore;
    existingGame.SeasonId = dto.SeasonId;

    await _context.SaveChangesAsync();
    return NoContent();
}

    // DELETE: api/AdminGames/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _context.NFLGames.FindAsync(id);
        if (game == null)
            return NotFound();

        _context.NFLGames.Remove(game);
        await _context.SaveChangesAsync();
        return NoContent();
    }

//GET ALL
    [HttpGet("all")]
public async Task<ActionResult<IEnumerable<AdminGameDto>>> GetAllGames()
{
    return await _context.NFLGames
        .Include(g => g.HomeTeam)
        .Include(g => g.AwayTeam)
        .Include(g => g.Season)
        .Select(g => new AdminGameDto
        {
            Id = g.Id,
            GameDate = g.GameDate,
            HomeTeamName = g.HomeTeam.Name,
            AwayTeamName = g.AwayTeam.Name,
            HomeScore = g.HomeScore,
            AwayScore = g.AwayScore,
            SeasonYear = g.Season.Year
        })
        .ToListAsync();
}


    // GET: api/AdminGames/{id} (specific DTO version)
    [HttpGet("dto/{id}")]
    public async Task<ActionResult<AdminGameDto>> GetGameDto(int id)
    {
        var game = await _context.NFLGames
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .Include(g => g.Season)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return NotFound();

        return new AdminGameDto
        {
            // Map properties from NFLGame to AdminGameDto
            Id = game.Id,
            // Add other properties as needed
        };
    }


    // DELETE: api/AdminGames/dto/{id}
    [HttpDelete("dto/{id}")]
    public async Task<ActionResult> DeleteGameFromDto(int id)
    {
        var game = await _context.NFLGames.FindAsync(id);
        if (game == null)
            return NotFound();

        _context.NFLGames.Remove(game);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/AdminGames/dropdown-data
    [HttpGet("dropdown-data")]
    public async Task<ActionResult> GetDropdownData()
    {
        var teams = await _context.Teams
            .Select(t => new AdminTeamDto { Id = t.Id, Name = t.Name })
            .ToListAsync();

        var seasons = await _context.Seasons
            .Select(s => new AdminSeasonDto { Id = s.Id, Year = s.Year })
            .ToListAsync();

        return Ok(new { teams, seasons });
    }
}