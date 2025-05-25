using Microsoft.AspNetCore.Mvc;
using EspnBackend.Data;
using EspnBackend.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTeamsController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminTeamsController(EspnDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminTeams/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTeams()
        {
            var teams = new List<object>();
            var sql = @"
                SELECT t.Id, t.Name, s.Name AS Stadium, c.Name AS City
                FROM Teams t
                LEFT JOIN Cities c ON t.CityId = c.Id
                LEFT JOIN Stadiums s ON t.StadiumId = s.Id
            ";

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            teams.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Stadium = reader.GetString(reader.GetOrdinal("Stadium")),
                                City = reader.GetString(reader.GetOrdinal("City"))
                            });
                        }
                    }
                }
            }

            return Ok(teams);
        }

        // POST: api/AdminTeams
        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamDto dto)
        {
            int cityId, stadiumId;

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Get Stadium ID
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Stadiums WHERE Name = @name";
                    var p = cmd.CreateParameter(); p.ParameterName = "@name"; p.Value = dto.Stadium; cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"Stadium '{dto.Stadium}' not found");
                    stadiumId = System.Convert.ToInt32(result);
                }

                // Get City ID
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Cities WHERE Name = @name";
                    var p = cmd.CreateParameter(); p.ParameterName = "@name"; p.Value = dto.City; cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"City '{dto.City}' not found");
                    cityId = System.Convert.ToInt32(result);
                }

                // Insert new team
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Teams (Name, StadiumId, CityId) VALUES (@name, @stadiumId, @cityId)";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@stadiumId"; p2.Value = stadiumId; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@cityId"; p3.Value = cityId; cmd.Parameters.Add(p3);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // PUT: api/AdminTeams/dto/{id}
        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamDto dto)
        {
            bool exists;
            int cityId, stadiumId;

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Check existence
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Teams WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }

                if (!exists)
                    return NotFound();

                // Get Stadium ID
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Stadiums WHERE Name = @name";
                    var p = cmd.CreateParameter(); p.ParameterName = "@name"; p.Value = dto.Stadium; cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"Stadium '{dto.Stadium}' not found");
                    stadiumId = System.Convert.ToInt32(result);
                }

                // Get City ID
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Cities WHERE Name = @name";
                    var p = cmd.CreateParameter(); p.ParameterName = "@name"; p.Value = dto.City; cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"City '{dto.City}' not found");
                    cityId = System.Convert.ToInt32(result);
                }

                // Update team
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Teams SET Name = @name, StadiumId = @stadiumId, CityId = @cityId WHERE Id = @id";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@stadiumId"; p2.Value = stadiumId; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@cityId"; p3.Value = cityId; cmd.Parameters.Add(p3);
                    var p4 = cmd.CreateParameter(); p4.ParameterName = "@id"; p4.Value = id; cmd.Parameters.Add(p4);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // DELETE: api/AdminTeams/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            bool exists;

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Teams WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }

                if (!exists)
                    return NotFound();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Teams WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // GET: api/AdminTeams/dropdown-data
        [HttpGet("dropdown-data")]
        public async Task<IActionResult> GetDropdownData()
        {
            var cities = new List<object>();
            var stadiums = new List<object>();

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Cities
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cities";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cities.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            });
                        }
                    }
                }

                // Stadiums
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Stadiums";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stadiums.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            });
                        }
                    }
                }
            }

            return Ok(new { cities, stadiums });
        }
    }
}