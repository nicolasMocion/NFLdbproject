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
public class PlayerStatsController : ControllerBase
{
    private readonly EspnDbContext _context;

    public PlayerStatsController(EspnDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlayerStatsDto>>> GetAllPlayerStats()
    {
        var players = await _context.Database.SqlQueryRaw<PlayerStatsDto>(@"
            SELECT 
                ps.Id,
                p.Name AS PlayerName,
                ps.Yards,
                ps.Touchdowns,
                ps.Interceptions
            FROM PlayerStats ps
            JOIN Players p ON ps.PlayerId = p.Id
        ").ToListAsync();

        return Ok(players);
    }

    [HttpGet("sorted")]
    public async Task<ActionResult<List<PlayerStatsDto>>> GetSortedPlayerStats([FromQuery] string sortBy = "playername", [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "playername", "p.Name" },
            { "yards", "ps.Yards" },
            { "touchdowns", "ps.Touchdowns" },
            { "interceptions", "ps.Interceptions" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                ps.Id,
                p.Name AS PlayerName,
                ps.Yards,
                ps.Touchdowns,
                ps.Interceptions
            FROM PlayerStats ps
            JOIN Players p ON ps.PlayerId = p.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<PlayerStatsDto>(sql).ToListAsync();
        return Ok(result);
    }

    [HttpGet("filter")]
public async Task<ActionResult<List<PlayerStatsDto>>> FilterPlayerStats(
    [FromQuery] string? yards,
    [FromQuery] string? touchdowns,
    [FromQuery] string? interceptions,
    [FromQuery] string? playername,
    [FromQuery] string? sortBy = "playername",
    [FromQuery] string? order = "asc")
{
    var allowedColumns = new Dictionary<string, string>
    {
        { "playername", "p.Name" },
        { "yards", "ps.Yards" },
        { "touchdowns", "ps.Touchdowns" },
        { "interceptions", "ps.Interceptions" }
    };

    sortBy = sortBy?.ToLower() ?? "playername";
    order = order?.ToLower() == "desc" ? "DESC" : "ASC";

    if (!allowedColumns.ContainsKey(sortBy))
        return BadRequest("Invalid sort field");

    var filters = new List<string>();
    var parameters = new List<MySqlParameter>();

    if (!string.IsNullOrWhiteSpace(playername))
    {
        filters.Add("p.Name = @playername");
        parameters.Add(new MySqlParameter("@playername", playername));
    }
    if (!string.IsNullOrWhiteSpace(yards))
    {
        filters.Add("ps.Yards = @yards");
        parameters.Add(new MySqlParameter("@yards", yards));
    }
    if (!string.IsNullOrWhiteSpace(touchdowns))
    {
        filters.Add("ps.Touchdowns = @touchdowns");
        parameters.Add(new MySqlParameter("@touchdowns", touchdowns));
    }
    if (!string.IsNullOrWhiteSpace(interceptions))
    {
        filters.Add("ps.Interceptions = @interceptions");
        parameters.Add(new MySqlParameter("@interceptions", interceptions));
    }

    var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

    var sql = $@"
        SELECT 
            ps.Id,
            p.Name AS PlayerName,
            ps.Yards,
            ps.Touchdowns,
            ps.Interceptions
        FROM PlayerStats ps
        JOIN Players p ON ps.PlayerId = p.Id
        {whereClause}
        ORDER BY {allowedColumns[sortBy]} {order}
    ";

    var result = await _context.Database.SqlQueryRaw<PlayerStatsDto>(sql, parameters.ToArray()).ToListAsync();
    return Ok(result);
}


    

    [HttpGet("{field}")]
    public async Task<IActionResult> GetDistinctStatValues(string field)
    {
        switch (field.ToLower())
        {
            case "yards":
                return Ok(await _context.PlayerStats.Select(p => p.Yards).Distinct().ToListAsync());
            case "touchdowns":
                return Ok(await _context.PlayerStats.Select(p => p.Touchdowns).Distinct().ToListAsync());
            case "interceptions":
                return Ok(await _context.PlayerStats.Select(p => p.Interceptions).Distinct().ToListAsync());
            default:
                return BadRequest("Invalid filter field");
        }
    }
}