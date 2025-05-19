using Microsoft.AspNetCore.Mvc;
using EspnBackend.DTO;
using EspnBackend.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class NFLGamesController : ControllerBase
{
    private readonly EspnDbContext _context;

    public NFLGamesController(EspnDbContext context)
    {
        _context = context;
    }

    // Get all games
    [HttpGet]
    public async Task<ActionResult<List<NFLGameDto>>> GetAllGames()
    {
        var games = await _context.Database.SqlQueryRaw<NFLGameDto>(@"
            SELECT 
                g.Id,
                DATE_FORMAT(g.GameDate, '%Y-%m-%d') AS GameDate,
                ht.Name AS HomeTeamName,
                at.Name AS AwayTeamName,
                g.HomeScore,
                g.AwayScore,
                s.Year
            FROM NFLGames g
            JOIN Teams ht ON g.HomeTeamId = ht.Id
            JOIN Teams at ON g.AwayTeamId = at.Id
            JOIN Seasons s ON g.SeasonId = s.Id
        ").ToListAsync();

        return Ok(games);
    }

    // Sorted games
    [HttpGet("sorted")]
    public async Task<ActionResult<List<NFLGameDto>>> GetSortedGames([FromQuery] string sortBy = "gamedate", [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "gamedate", "g.GameDate" },
            { "hometeamname", "ht.Name" },
            { "awayteamname", "at.Name" },
            { "homescore", "g.HomeScore" },
            { "awayscore", "g.AwayScore" },
            { "seasonyear", "s.Year" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                g.Id,
                DATE_FORMAT(g.GameDate, '%Y-%m-%d') AS GameDate,
                ht.Name AS HomeTeamName,
                at.Name AS AwayTeamName,
                g.HomeScore,
                g.AwayScore,
                s.Year
            FROM NFLGames g
            JOIN Teams ht ON g.HomeTeamId = ht.Id
            JOIN Teams at ON g.AwayTeamId = at.Id
            JOIN Seasons s ON g.SeasonId = s.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<NFLGameDto>(sql).ToListAsync();
        return Ok(result);
    }

    // Filter games by HomeTeamName, AwayTeamName, or SeasonYear
    [HttpGet("filter")]
    public async Task<ActionResult<List<NFLGameDto>>> FilterGames(
        [FromQuery] string? hometeam,
        [FromQuery] string? awayteam,
        [FromQuery] int? year,
        [FromQuery] string? sortBy = "gamedate",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "gamedate", "g.GameDate" },
            { "hometeamname", "ht.Name" },
            { "awayteamname", "at.Name" },
            { "homescore", "g.HomeScore" },
            { "awayscore", "g.AwayScore" },
            { "seasonyear", "s.Year" }
        };

        sortBy = sortBy?.ToLower() ?? "gamedate";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrWhiteSpace(hometeam))
        {
            filters.Add("ht.Name = @hometeam");
            parameters.Add(new MySqlParameter("@hometeam", hometeam));
        }

        if (!string.IsNullOrWhiteSpace(awayteam))
        {
            filters.Add("at.Name = @awayteam");
            parameters.Add(new MySqlParameter("@awayteam", awayteam));
        }

        if (year.HasValue)
        {
            filters.Add("s.Year = @seasonyear");
            parameters.Add(new MySqlParameter("@seasonyear", year.Value));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                g.Id,
                DATE_FORMAT(g.GameDate, '%Y-%m-%d') AS GameDate,
                ht.Name AS HomeTeamName,
                at.Name AS AwayTeamName,
                g.HomeScore,
                g.AwayScore,
                s.Year
            FROM NFLGames g
            JOIN Teams ht ON g.HomeTeamId = ht.Id
            JOIN Teams at ON g.AwayTeamId = at.Id
            JOIN Seasons s ON g.SeasonId = s.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<NFLGameDto>(sql, parameters.ToArray()).ToListAsync();
        return Ok(result);
    }
}