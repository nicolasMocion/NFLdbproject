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
public class CoachTeamHistoryController : ControllerBase
{
    private readonly EspnDbContext _context;

    public CoachTeamHistoryController(EspnDbContext context)
    {
        _context = context;
    }

    // Get all coach-team histories
    [HttpGet]
    public async Task<ActionResult<List<CoachTeamHistoryDTO>>> GetAllCoachTeamHistories()
    {
        var histories = await _context.Database.SqlQueryRaw<CoachTeamHistoryDTO>(@"
            SELECT 
                c.Name AS CoachName,
                t.Name AS TeamName,
                DATE_FORMAT(cth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(cth.EndDate, '%Y-%m-%d') AS EndDate
            FROM CoachTeamHistory cth
            JOIN Coaches c ON cth.CoachId = c.Id
            JOIN Teams t ON cth.TeamId = t.Id
        ").ToListAsync();

        return Ok(histories);
    }

    // Sorted coach-team histories
    [HttpGet("sorted")]
    public async Task<ActionResult<List<CoachTeamHistoryDTO>>> GetSortedCoachTeamHistories(
        [FromQuery] string sortBy = "coachname", 
        [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "coachname", "c.Name" },
            { "teamname", "t.Name" },
            { "startdate", "cth.StartDate" },
            { "enddate", "cth.EndDate" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                c.Name AS CoachName,
                t.Name AS TeamName,
                DATE_FORMAT(cth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(cth.EndDate, '%Y-%m-%d') AS EndDate
            FROM CoachTeamHistory cth
            JOIN Coaches c ON cth.CoachId = c.Id
            JOIN Teams t ON cth.TeamId = t.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<CoachTeamHistoryDTO>(sql).ToListAsync();
        return Ok(result);
    }

    // Filter coach-team histories by CoachName, TeamName, or date range
    [HttpGet("filter")]
    public async Task<ActionResult<List<CoachTeamHistoryDTO>>> FilterCoachTeamHistories(
        [FromQuery] string? coachname,
        [FromQuery] string? teamname,
        [FromQuery] string? startdate,
        [FromQuery] string? enddate,
        [FromQuery] string? sortBy = "coachname",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "coachname", "c.Name" },
            { "teamname", "t.Name" },
            { "startdate", "cth.StartDate" },
            { "enddate", "cth.EndDate" }
        };

        sortBy = sortBy?.ToLower() ?? "coachname";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrWhiteSpace(coachname))
        {
            filters.Add("c.Name = @coachname");
            parameters.Add(new MySqlParameter("@coachname", coachname));
        }

        if (!string.IsNullOrWhiteSpace(teamname))
        {
            filters.Add("t.Name = @teamname");
            parameters.Add(new MySqlParameter("@teamname", teamname));
        }

        if (!string.IsNullOrWhiteSpace(startdate))
        {
            filters.Add("cth.StartDate >= @startdate");
            parameters.Add(new MySqlParameter("@startdate", startdate));
        }

        if (!string.IsNullOrWhiteSpace(enddate))
        {
            filters.Add("(cth.EndDate IS NULL OR cth.EndDate <= @enddate)");
            parameters.Add(new MySqlParameter("@enddate", enddate));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                c.Name AS CoachName,
                t.Name AS TeamName,
                DATE_FORMAT(cth.StartDate, '%Y-%m-%d') AS StartDate,
                DATE_FORMAT(cth.EndDate, '%Y-%m-%d') AS EndDate
            FROM CoachTeamHistory cth
            JOIN Coaches c ON cth.CoachId = c.Id
            JOIN Teams t ON cth.TeamId = t.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<CoachTeamHistoryDTO>(sql, parameters.ToArray()).ToListAsync();
        return Ok(result);
    }
}