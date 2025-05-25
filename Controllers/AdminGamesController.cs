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
        // Raw SQL with joins to get all games with their related data
        var sql = @"
            SELECT g.*
            FROM NFLGames g
        ";

        // Here we fetch raw NFLGames, but without joins because we return NFLGame entities only.
        // For related data, you'd need manual joins and mapping to DTOs or navigation property eager load - EF Core can't auto map includes in raw SQL.

        var games = await _context.NFLGames
            .FromSqlRaw(sql)
            .ToListAsync();

        return games;
    }

    // GET: api/AdminGames/5
    [HttpGet("{id}")]
    public async Task<ActionResult<NFLGame>> GetGame(int id)
    {
        var sql = @"SELECT * FROM NFLGames WHERE Id = {0}";
        var game = await _context.NFLGames.FromSqlRaw(sql, id).FirstOrDefaultAsync();

        if (game == null)
            return NotFound();

        return game;
    }

    // POST: api/AdminGames/dto
    [HttpPost("dto")]
    public async Task<ActionResult<NFLGame>> CreateGame(AdminGameEditDto dto)
    {
        var sql = @"
            INSERT INTO NFLGames (GameDate, HomeTeamId, AwayTeamId, HomeScore, AwayScore, SeasonId)
            VALUES ({0}, {1}, {2}, {3}, {4}, {5});
            SELECT LAST_INSERT_ID();";

        var id = await _context.Database.ExecuteSqlRawAsync(
            @"INSERT INTO NFLGames (GameDate, HomeTeamId, AwayTeamId, HomeScore, AwayScore, SeasonId)
              VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
            dto.GameDate, dto.HomeTeamId, dto.AwayTeamId, dto.HomeScore, dto.AwayScore, dto.SeasonId);

        // Unfortunately, ExecuteSqlRawAsync returns number of rows affected, not the inserted ID.
        // For getting inserted ID, you can do:
        // var id = await _context.NFLGames.Select(g => g.Id).MaxAsync(); // unsafe in concurrent env.
        // OR use a raw SQL scalar query:
        var insertedId = await _context.NFLGames
            .FromSqlRaw("SELECT * FROM NFLGames ORDER BY Id DESC LIMIT 1")
            .Select(g => g.Id)
            .FirstAsync();

        var game = await _context.NFLGames.FindAsync(insertedId);

        return CreatedAtAction(nameof(GetGame), new { id = insertedId }, game);
    }

    // PUT: api/AdminGames/dto/{id}
    [HttpPut("dto/{id}")]
    public async Task<IActionResult> UpdateGame(int id, AdminGameEditDto dto)
    {
        var existing = await _context.NFLGames.FindAsync(id);
        if (existing == null)
            return NotFound();

        var sql = @"
            UPDATE NFLGames
            SET GameDate = {0},
                HomeTeamId = {1},
                AwayTeamId = {2},
                HomeScore = {3},
                AwayScore = {4},
                SeasonId = {5}
            WHERE Id = {6}";

        await _context.Database.ExecuteSqlRawAsync(sql,
            dto.GameDate, dto.HomeTeamId, dto.AwayTeamId, dto.HomeScore, dto.AwayScore, dto.SeasonId, id);

        return NoContent();
    }

    // DELETE: api/AdminGames/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var existing = await _context.NFLGames.FindAsync(id);
        if (existing == null)
            return NotFound();

        var sql = @"DELETE FROM NFLGames WHERE Id = {0}";

        await _context.Database.ExecuteSqlRawAsync(sql, id);

        return NoContent();
    }

    // GET ALL with DTO: api/AdminGames/all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AdminGameDto>>> GetAllGames()
    {
        var sql = @"
            SELECT g.Id, g.GameDate, ht.Name AS HomeTeamName, at.Name AS AwayTeamName, 
                   g.HomeScore, g.AwayScore, s.Year AS SeasonYear
            FROM NFLGames g
            INNER JOIN Teams ht ON g.HomeTeamId = ht.Id
            INNER JOIN Teams at ON g.AwayTeamId = at.Id
            INNER JOIN Seasons s ON g.SeasonId = s.Id";

        var gameDtos = await _context.Set<AdminGameDto>()
            .FromSqlRaw(sql)
            .ToListAsync();

        return gameDtos;
    }

    // GET: api/AdminGames/dto/{id}
    [HttpGet("dto/{id}")]
    public async Task<ActionResult<AdminGameDto>> GetGameDto(int id)
    {
        var sql = @"
            SELECT g.Id, g.GameDate, ht.Name AS HomeTeamName, at.Name AS AwayTeamName, 
                   g.HomeScore, g.AwayScore, s.Year AS SeasonYear
            FROM NFLGames g
            INNER JOIN Teams ht ON g.HomeTeamId = ht.Id
            INNER JOIN Teams at ON g.AwayTeamId = at.Id
            INNER JOIN Seasons s ON g.SeasonId = s.Id
            WHERE g.Id = {0}";

        var gameDto = await _context.Set<AdminGameDto>().FromSqlRaw(sql, id).FirstOrDefaultAsync();

        if (gameDto == null)
            return NotFound();

        return gameDto;
    }

    // DELETE: api/AdminGames/dto/{id}
    [HttpDelete("dto/{id}")]
    public async Task<ActionResult> DeleteGameFromDto(int id)
    {
        var existing = await _context.NFLGames.FindAsync(id);
        if (existing == null)
            return NotFound();

        var sql = @"DELETE FROM NFLGames WHERE Id = {0}";
        await _context.Database.ExecuteSqlRawAsync(sql, id);

        return NoContent();
    }

    // GET: api/AdminGames/dropdown-data
    [HttpGet("dropdown-data")]
    public async Task<ActionResult> GetDropdownData()
    {
        var teamsSql = @"SELECT Id, Name FROM Teams";
        var seasonsSql = @"SELECT Id, Year FROM Seasons";

        var teams = await _context.Set<AdminTeamDto>().FromSqlRaw(teamsSql).ToListAsync();
        var seasons = await _context.Set<AdminSeasonDto>().FromSqlRaw(seasonsSql).ToListAsync();

        return Ok(new { teams, seasons });
    }
}