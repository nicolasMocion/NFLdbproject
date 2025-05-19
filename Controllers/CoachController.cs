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
public class CoachesController : ControllerBase
{
    private readonly EspnDbContext _context;

    public CoachesController(EspnDbContext context)
    {
        _context = context;
    }

    // Get all coaches
    [HttpGet]
    public async Task<ActionResult<List<CoachDTO>>> GetAllCoaches()
    {
        var coaches = await _context.Database.SqlQueryRaw<CoachDTO>(@"
            SELECT 
                c.Id,
                c.Name,
                c.Role,
                c.Speciality,
                t.Name AS TeamName
            FROM Coaches c
            JOIN Teams t ON c.TeamId = t.Id
        ").ToListAsync();

        return Ok(coaches);
    }

    // Sorted coaches
    [HttpGet("sorted")]
    public async Task<ActionResult<List<CoachDTO>>> GetSortedCoaches([FromQuery] string sortBy = "name", [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "c.Name" },
            { "role", "c.Role" },
            { "speciality", "c.Speciality" },
            { "teamname", "t.Name" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                c.Id,
                c.Name,
                c.Role,
                c.Speciality,
                t.Name AS TeamName
            FROM Coaches c
            JOIN Teams t ON c.TeamId = t.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<CoachDTO>(sql).ToListAsync();
        return Ok(result);
    }

    // Filter coaches by Name, Role, Speciality, or TeamName
    [HttpGet("filter")]
    public async Task<ActionResult<List<CoachDTO>>> FilterCoaches(
        [FromQuery] string? name,
        [FromQuery] string? role,
        [FromQuery] string? speciality,
        [FromQuery] string? teamname,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "c.Name" },
            { "role", "c.Role" },
            { "speciality", "c.Speciality" },
            { "teamname", "t.Name" }
        };

        sortBy = sortBy?.ToLower() ?? "name";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            filters.Add("c.Name LIKE @name");
            parameters.Add(new MySqlParameter("@name", $"%{name}%"));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            filters.Add("c.Role = @role");
            parameters.Add(new MySqlParameter("@role", role));
        }

        if (!string.IsNullOrWhiteSpace(speciality))
        {
            filters.Add("c.Speciality = @speciality");
            parameters.Add(new MySqlParameter("@speciality", speciality));
        }

        if (!string.IsNullOrWhiteSpace(teamname))
        {
            filters.Add("t.Name = @teamname");
            parameters.Add(new MySqlParameter("@teamname", teamname));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : "";

        var sql = $@"
            SELECT 
                c.Id,
                c.Name,
                c.Role,
                c.Speciality,
                t.Name AS TeamName
            FROM Coaches c
            JOIN Teams t ON c.TeamId = t.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var result = await _context.Database.SqlQueryRaw<CoachDTO>(sql, parameters.ToArray()).ToListAsync();
        return Ok(result);
    }
}