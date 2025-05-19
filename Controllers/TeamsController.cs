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
public class TeamsController : ControllerBase
{
    private readonly EspnDbContext _context;

    public TeamsController(EspnDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<TeamDto>>> GetTeamsWithDetails()
    {
        var teams = await _context.Database.SqlQueryRaw<TeamDto>(@"
            SELECT 
                t.Id,
                t.Name,
                ct.Name AS City,
                s.Name AS Stadium
            FROM Teams t
            JOIN Stadiums s ON t.StadiumId = s.Id
            JOIN Cities ct ON t.CityId = ct.Id
        ").ToListAsync();

        return Ok(teams);
    }

    [HttpGet("sorted")]
    public async Task<ActionResult<List<TeamDto>>> GetSortedTeams([FromQuery] string sortBy = "Name", [FromQuery] string order = "asc")
    {
        // SQL injection-safe way of handling ordering with a limited whitelist
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "t.Name" },
            { "city", "ct.city" },
            { "stadium", "s.Name" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                t.Id,
                t.Name,
                ct.Name AS City,
                s.Name AS Stadium
            FROM Teams T
            JOIN Stadiums s ON t.StadiumId = s.Id
            JOIN Cities ct ON t.CityId = ct.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var teams = await _context.Database.SqlQueryRaw<TeamDto>(sql).ToListAsync();
        return Ok(teams);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<TeamDto>>> Filterteams(
        [FromQuery] string? stadium,
        [FromQuery] string? city,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "t.Name" },
            { "city", "ct.Name" },
            { "stadium", "s.Name" }
        };

        sortBy = sortBy?.ToLower() ?? "name";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrEmpty(stadium))
        {
            filters.Add("s.Name = @stadium");
            parameters.Add(new MySqlParameter("@stadium", stadium));
        }

        if (!string.IsNullOrEmpty(city))
        {
            filters.Add("ct.Name = @city");
            parameters.Add(new MySqlParameter("@city", city));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                t.Id,
                t.Name,
                ct.Name AS City,
                s.Name AS Stadium
            FROM Teams t
            JOIN Stadiums s ON t.StadiumId = s.Id
            JOIN Cities ct ON t.CityId = ct.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<TeamDto>(sql, parameters.ToArray()).ToListAsync();

        return Ok(result);
    }

    [HttpGet("{field}")]
    public async Task<IActionResult> GetDistinctValues(string field)
    {
        switch (field.ToLower())
        {
            case "stadium":
                return Ok(await _context.Stadiums.Select(s => s.Name).Distinct().ToListAsync());
            case "city":
                return Ok(await _context.Cities.Select(c => c.Name).Distinct().ToListAsync());
            default:
                return BadRequest("Invalid filter field");
        }
    }


}
