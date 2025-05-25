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
    public class AdminCollegeController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCollegeController(EspnDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminCollege/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllColleges()
        {
            var colleges = new List<object>();
            var sql = @"
                SELECT col.Id, col.Name, city.Name AS CityName
                FROM Colleges col
                LEFT JOIN Cities city ON col.CityId = city.Id
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
                            colleges.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName"))
                            });
                        }
                    }
                }
            }

            return Ok(colleges);
        }

        // POST: api/AdminCollege/dto
        [HttpPost("dto")]
        public async Task<IActionResult> CreateCollege(CollegeDTO dto)
        {
            int cityId = 0;
            var citySql = "SELECT Id FROM Cities WHERE Name = @cityName";

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Get city ID
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = citySql;
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@cityName";
                    p.Value = dto.CityName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(p);

                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"City '{dto.CityName}' not found");

                    cityId = System.Convert.ToInt32(result);
                }

                // Insert new college
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Colleges (Name, CityId) VALUES (@name, @cityId)";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name ?? (object)DBNull.Value; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@cityId"; p2.Value = cityId; cmd.Parameters.Add(p2);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // PUT: api/AdminCollege/dto/{id}
        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateCollege(int id, CollegeDTO dto)
        {
            bool exists;
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Check existence
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Colleges WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }

                if (!exists)
                    return NotFound();

                // Get city ID
                int cityId = 0;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Cities WHERE Name = @cityName";
                    var p = cmd.CreateParameter(); p.ParameterName = "@cityName"; p.Value = dto.CityName ?? (object)DBNull.Value; cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest($"City '{dto.CityName}' not found");

                    cityId = System.Convert.ToInt32(result);
                }

                // Update college
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Colleges SET Name = @name, CityId = @cityId WHERE Id = @id";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name ?? (object)DBNull.Value; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@cityId"; p2.Value = cityId; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@id"; p3.Value = id; cmd.Parameters.Add(p3);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // DELETE: api/AdminCollege/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollege(int id)
        {
            bool exists;
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                // Check existence
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Colleges WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }

                if (!exists)
                    return NotFound();

                // Delete college
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Colleges WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        // GET: api/AdminCollege/dropdown-data
        [HttpGet("dropdown-data")]
        public async Task<IActionResult> GetDropdownData()
        {
            var cities = new List<object>();

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

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
            }

            return Ok(new { cities });
        }
    }
}