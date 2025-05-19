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
public class TeamTitlesController : ControllerBase
{
    private readonly EspnDbContext _context;

    public TeamTitlesController(EspnDbContext context)
    {
        _context = context;
    }

    // Get all team titles
    [HttpGet]
    public async Task<ActionResult<List<TeamTitleDTO>>> GetAllTeamTitles()
    {
        var teamTitles = await _context.Database.SqlQueryRaw<TeamTitleDTO>(@"
            SELECT 
                t.Name AS TeamName,
                ti.Name AS TitleName,
                tt.YearWon
            FROM TeamTitles tt
            JOIN Teams t ON tt.TeamId = t.Id
            JOIN Titles ti ON tt.TitleId = ti.Id
        ").ToListAsync();

        return Ok(teamTitles);
    }

    // Sorted team titles
    [HttpGet("sorted")]
    public async Task<ActionResult<List<TeamTitleDTO>>> GetSortedTeamTitles(
        [FromQuery] string sortBy = "teamname", 
        [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "teamname", "t.Name" },
            { "titlename", "ti.Name" },
            { "yearwon", "tt.YearWon" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                t.Name AS TeamName,
                ti.Name AS TitleName,
                tt.YearWon
            FROM TeamTitles tt
            JOIN Teams t ON tt.TeamId = t.Id
            JOIN Titles ti ON tt.TitleId = ti.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<TeamTitleDTO>(sql).ToListAsync();
        return Ok(result);
    }

    // Filter team titles by TeamName, TitleName, or YearWon
    [HttpGet("filter")]
    public async Task<ActionResult<List<TeamTitleDTO>>> FilterTeamTitles(
        [FromQuery] string? teamname,
        [FromQuery] string? titlename,
        [FromQuery] int? yearwon,
        [FromQuery] string? sortBy = "teamname",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "teamname", "t.Name" },
            { "titlename", "ti.Name" },
            { "yearwon", "tt.YearWon" }
        };

        sortBy = sortBy?.ToLower() ?? "teamname";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrWhiteSpace(teamname))
        {
            filters.Add("t.Name LIKE @teamname");
            parameters.Add(new MySqlParameter("@teamname", $"%{teamname}%"));
        }

        if (!string.IsNullOrWhiteSpace(titlename))
        {
            filters.Add("ti.Name LIKE @titlename");
            parameters.Add(new MySqlParameter("@titlename", $"%{titlename}%"));
        }

        if (yearwon.HasValue)
        {
            filters.Add("tt.YearWon = @yearwon");
            parameters.Add(new MySqlParameter("@yearwon", yearwon.Value));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                t.Name AS TeamName,
                ti.Name AS TitleName,
                tt.YearWon
            FROM TeamTitles tt
            JOIN Teams t ON tt.TeamId = t.Id
            JOIN Titles ti ON tt.TitleId = ti.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<TeamTitleDTO>(sql, parameters.ToArray()).ToListAsync();
        return Ok(result);
    }
}