using Microsoft.AspNetCore.Mvc;
using EspnBackend.Data;
using EspnBackend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class StadiumsController : ControllerBase
{
    private readonly EspnDbContext _context;

    public StadiumsController(EspnDbContext context)
    {
        _context = context;
    }

    // GET: api/Stadiums
    [HttpGet]
    public async Task<ActionResult<List<StadiumDTO>>> GetStadiums()
    {
        var stadiums = await _context.Database.SqlQueryRaw<StadiumDTO>(@"
            SELECT 
                s.Name,
                s.Capacity,
                c.Name AS CityName
            FROM Stadiums s
            JOIN Cities c ON s.CityId = c.Id
        ").ToListAsync();

        return Ok(stadiums);
    }

    // GET: api/Stadiums/sorted?sortBy=name&order=asc
    [HttpGet("sorted")]
    public async Task<ActionResult<List<StadiumDTO>>> GetSortedStadiums([FromQuery] string sortBy = "name", [FromQuery] string order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "s.Name" },
            { "capacity", "s.Capacity" },
            { "city", "c.Name" }
        };

        sortBy = sortBy.ToLower();
        order = order.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var sql = $@"
            SELECT 
                s.Name,
                s.Capacity,
                c.Name AS CityName
            FROM Stadiums s
            JOIN Cities c ON s.CityId = c.Id
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var stadiums = await _context.Database.SqlQueryRaw<StadiumDTO>(sql).ToListAsync();
        return Ok(stadiums);
    }

    // GET: api/Stadiums/filter?city=Miami&minCapacity=50000
    [HttpGet("filter")]
    public async Task<ActionResult<List<StadiumDTO>>> FilterStadiums(
        [FromQuery] string? city,
        [FromQuery] int? minCapacity,
        [FromQuery] int? maxCapacity,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? order = "asc")
    {
        var allowedColumns = new Dictionary<string, string>
        {
            { "name", "s.Name" },
            { "capacity", "s.Capacity" },
            { "city", "c.Name" }
        };

        sortBy = sortBy?.ToLower() ?? "name";
        order = order?.ToLower() == "desc" ? "DESC" : "ASC";

        if (!allowedColumns.ContainsKey(sortBy))
            return BadRequest("Invalid sort field");

        var filters = new List<string>();
        var parameters = new List<object>();

        if (!string.IsNullOrEmpty(city))
        {
            filters.Add("c.Name = {0}");
            parameters.Add(city);
        }
        if (minCapacity.HasValue)
        {
            filters.Add("s.Capacity >= {1}");
            parameters.Add(minCapacity.Value);
        }
        if (maxCapacity.HasValue)
        {
            filters.Add("s.Capacity <= {2}");
            parameters.Add(maxCapacity.Value);
        }

        string whereClause = filters.Count > 0 ? "WHERE " + string.Join(" AND ", filters) : "";

        var sql = $@"
            SELECT 
                s.Name,
                s.Capacity,
                c.Name AS CityName
            FROM Stadiums s
            JOIN Cities c ON s.CityId = c.Id
            {whereClause}
            ORDER BY {allowedColumns[sortBy]} {order}
        ";

        var stadiums = await _context.Database.SqlQueryRaw<StadiumDTO>(sql, parameters.ToArray()).ToListAsync();
        return Ok(stadiums);
    }

    // GET: api/Stadiums/cities
    [HttpGet("cities")]
    public async Task<IActionResult> GetDistinctCities()
    {
        var cities = await _context.Cities.Select(c => c.Name).Distinct().ToListAsync();
        return Ok(cities);
    }
}