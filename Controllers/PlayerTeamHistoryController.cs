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
public class PlayerTeamHistoryController : ControllerBase
{
    private readonly EspnDbContext _context;

    public PlayerTeamHistoryController(EspnDbContext context)
    {
        _context = context;
    }

    // Get all player team histories
    [HttpGet]
    public async Task<ActionResult<List<PlayerTeamHistoryDTO>>> GetAllPlayerTeamHistory()
    {
        var histories = await _context.Database.SqlQueryRaw<PlayerTeamHistoryDTO>(@"
            SELECT 
                p.Name AS PlayerName,
                t.Name AS TeamName,
                DATE_FORMAT(pth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(pth.EndDate, '%Y-%m-%d') AS EndDate
            FROM PlayerTeamHistory pth
            JOIN Players p ON pth.PlayerId = p.Id
            JOIN Teams t ON pth.TeamId = t.Id
        ").ToListAsync();

        return Ok(histories);
    }

    // Sorted player team histories
    [HttpGet("sorted")]
    public async Task<ActionResult<List<PlayerTeamHistoryDTO>>> GetSortedPlayerTeamHistories([FromQuery] string sortBy = "playername", [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "playername", "p.Name" },
            { "teamname", "t.Name" },
            { "startdate", "pth.StartDate" },
            { "enddate", "pth.EndDate" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                p.Name AS PlayerName,
                t.Name AS TeamName,
                DATE_FORMAT(pth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(pth.EndDate, '%Y-%m-%d') AS EndDate
            FROM PlayerTeamHistory pth
            JOIN Players p ON pth.PlayerId = p.Id
            JOIN Teams t ON pth.TeamId = t.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<PlayerTeamHistoryDTO>(sql).ToListAsync();
        return Ok(result);
    }

    // Filter player team histories by PlayerName, TeamName, or date range
    [HttpGet("filter")]
    public async Task<ActionResult<List<PlayerTeamHistoryDTO>>> FilterPlayerTeamHistories(
        [FromQuery] string? playername,
        [FromQuery] string? teamname,
        [FromQuery] string? startdate,
        [FromQuery] string? enddate,
        [FromQuery] string? sortBy = "playername",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "playername", "p.Name" },
            { "teamname", "t.Name" },
            { "startdate", "pth.StartDate" },
            { "enddate", "pth.EndDate" }
        };

        sortBy = sortBy?.ToLower() ?? "playername";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrWhiteSpace(playername))
        {
            filters.Add("p.Name LIKE @playername");
            parameters.Add(new MySqlParameter("@playername", $"%{playername}%"));
        }

        if (!string.IsNullOrWhiteSpace(teamname))
        {
            filters.Add("t.Name LIKE @teamname");
            parameters.Add(new MySqlParameter("@teamname", $"%{teamname}%"));
        }

        if (!string.IsNullOrWhiteSpace(startdate))
        {
            filters.Add("pth.StartDate >= @startdate");
            parameters.Add(new MySqlParameter("@startdate", startdate));
        }

        if (!string.IsNullOrWhiteSpace(enddate))
        {
            filters.Add("(pth.EndDate IS NULL OR pth.EndDate <= @enddate)");
            parameters.Add(new MySqlParameter("@enddate", enddate));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                p.Name AS PlayerName,
                t.Name AS TeamName,
                DATE_FORMAT(pth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(pth.EndDate, '%Y-%m-%d') AS EndDate
            FROM PlayerTeamHistory pth
            JOIN Players p ON pth.PlayerId = p.Id
            JOIN Teams t ON pth.TeamId = t.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<PlayerTeamHistoryDTO>(sql, parameters.ToArray()).ToListAsync();
        return Ok(result);
    }
}