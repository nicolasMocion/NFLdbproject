using Microsoft.AspNetCore.Mvc;
using EspnBackend.Models;
using EspnBackend.Database; // or your DbContext namespace
using EspnBackend.Data;
using EspnBackend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly EspnDbContext _context;

    public PlayersController(EspnDbContext context)
    {
        _context = context;
    }

[HttpGet]
public async Task<ActionResult<List<object>>> GetPlayersWithDetails()
{
    var players = await _context
        .Database
        .SqlQueryRaw<RawPlayerDto>(@"
            SELECT 
                p.Id,
                p.Name,
                ct.Name AS CollegeName,
                t.Name AS TeamName,
                ct.Name AS HomeTownCityName
            FROM Players p
            JOIN Colleges c ON p.CollegeId = c.Id
            JOIN Teams t ON p.TeamId = t.Id
            JOIN Cities ct ON p.HometownCityId = ct.Id
        ").ToListAsync();

    return Ok(players);
} 


    [HttpGet("sorted")]
    public async Task<ActionResult<List<RawPlayerDto>>> GetSortedPlayers([FromQuery] string sortBy = "Name", [FromQuery] string order = "asc")
    {
        // SQL injection-safe way of handling ordering with a limited whitelist
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "p.Name" },
            { "position", "p.Position" },
            { "college", "c.Name" },
            { "team", "t.Name" },
            { "hometown", "ct.Name" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                p.Id,
                p.Name,
                p.Position,
                c.Name AS CollegeName,
                t.Name AS TeamName,
                ct.Name AS HomeTownCityName
            FROM Players p
            JOIN Colleges c ON p.CollegeId = c.Id
            JOIN Teams t ON p.TeamId = t.Id
            JOIN Cities ct ON p.HomeTownCityId = ct.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var players = await _context.Database.SqlQueryRaw<RawPlayerDto>(sql).ToListAsync();
        return Ok(players);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<RawPlayerDto>>> FilterPlayers(
        [FromQuery] string? team,
        [FromQuery] string? position,
        [FromQuery] string? college,
        [FromQuery] string? hometown,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "p.Name" },
            { "position", "p.Position" },
            { "college", "c.Name" },
            { "team", "t.Name" },
            { "hometown", "ct.Name" }
        };

        sortBy = sortBy?.ToLower() ?? "name";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        // Build WHERE clause dynamically
        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrEmpty(team))
        {
            filters.Add("t.Name = @team");
            parameters.Add(new MySqlParameter("@team", team));
        }

        if (!string.IsNullOrEmpty(position))
        {
            filters.Add("p.Position = @position");
            parameters.Add(new MySqlParameter("@position", position));
        }

        if (!string.IsNullOrEmpty(college))
        {
            filters.Add("c.Name = @college");
            parameters.Add(new MySqlParameter("@college", college));
        }

        if (!string.IsNullOrEmpty(hometown))
        {
            filters.Add("ct.Name = @hometown");
            parameters.Add(new MySqlParameter("@hometown", hometown));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                p.Id,
                p.Name,
                p.Position,
                c.Name AS CollegeName,
                t.Name AS TeamName,
                ct.Name AS HomeTownCityName
            FROM Players p
            JOIN Colleges c ON p.CollegeId = c.Id
            JOIN Teams t ON p.TeamId = t.Id
            JOIN Cities ct ON p.HomeTownCityId = ct.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<RawPlayerDto>(sql, parameters.ToArray()).ToListAsync();

        return Ok(result);
    }


    [HttpGet("{field}")]
    public async Task<IActionResult> GetDistinctValues(string field)
    {
        switch (field.ToLower())
        {
            case "team":
                return Ok(await _context.Teams.Select(t => t.Name).Distinct().ToListAsync());
            case "position":
                return Ok(await _context.Players.Select(p => p.Position).Distinct().ToListAsync());
            case "college":
                return Ok(await _context.Colleges.Select(c => c.Name).Distinct().ToListAsync());
            case "hometown":
                return Ok(await _context.Cities.Select(c => c.Name).Distinct().ToListAsync());
            default:
                return BadRequest("Invalid filter field");
        }
    }


}
