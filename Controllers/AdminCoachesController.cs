using Microsoft.AspNetCore.Mvc;
using EspnBackend.Data;
using EspnBackend.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCoachesController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCoachesController(EspnDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminCoaches/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CoachDTO>>> GetAllCoaches()
        {
            var result = new List<CoachDTO>();
            var sql = @"
                SELECT c.Id, c.Name, c.Role, c.Speciality, t.Name AS TeamName
                FROM Coaches c
                LEFT JOIN Teams t ON c.TeamId = t.Id
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
                            result.Add(new CoachDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                Speciality = reader.IsDBNull(reader.GetOrdinal("Speciality")) ? null : reader.GetString(reader.GetOrdinal("Speciality")),
                                TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName")) ? null : reader.GetString(reader.GetOrdinal("TeamName"))
                            });
                        }
                    }
                }
            }
            return Ok(result);
        }

        // POST: api/AdminCoaches/dto
        [HttpPost("dto")]
        public async Task<IActionResult> CreateCoach(CoachDTO dto)
        {
            int teamId = 0;
            var teamSql = "SELECT Id FROM Teams WHERE Name = @teamName";
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = teamSql;
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@teamName";
                    p.Value = dto.TeamName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(p);

                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest("Team not found");
                    teamId = System.Convert.ToInt32(result);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Coaches (Name, Role, Speciality, TeamId) VALUES (@name, @role, @speciality, @teamId)";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name ?? (object)DBNull.Value; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@role"; p2.Value = dto.Role ?? (object)DBNull.Value; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@speciality"; p3.Value = (object?)dto.Speciality ?? DBNull.Value; cmd.Parameters.Add(p3);
                    var p4 = cmd.CreateParameter(); p4.ParameterName = "@teamId"; p4.Value = teamId; cmd.Parameters.Add(p4);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

        // PUT: api/AdminCoaches/dto/{id}
        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateCoach(int id, CoachDTO dto)
        {
            var exists = false;
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Coaches WHERE Id = @id";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@id";
                    p.Value = id;
                    cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }
                if (!exists)
                    return NotFound();

                int teamId = 0;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Teams WHERE Name = @teamName";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@teamName";
                    p.Value = dto.TeamName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(p);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                        return BadRequest("Team not found");
                    teamId = System.Convert.ToInt32(result);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coaches SET Name = @name, Role = @role, Speciality = @speciality, TeamId = @teamId WHERE Id = @id";
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name ?? (object)DBNull.Value; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@role"; p2.Value = dto.Role ?? (object)DBNull.Value; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@speciality"; p3.Value = (object?)dto.Speciality ?? DBNull.Value; cmd.Parameters.Add(p3);
                    var p4 = cmd.CreateParameter(); p4.ParameterName = "@teamId"; p4.Value = teamId; cmd.Parameters.Add(p4);
                    var p5 = cmd.CreateParameter(); p5.ParameterName = "@id"; p5.Value = id; cmd.Parameters.Add(p5);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

        // DELETE: api/AdminCoaches/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            var exists = false;
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(1) FROM Coaches WHERE Id = @id";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@id";
                    p.Value = id;
                    cmd.Parameters.Add(p);
                    exists = System.Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                }
                if (!exists)
                    return NotFound();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coaches WHERE Id = @id";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@id";
                    p.Value = id;
                    cmd.Parameters.Add(p);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

        // GET: api/AdminCoaches/dropdown-data
        [HttpGet("dropdown-data")]
        public async Task<IActionResult> GetDropdownData()
        {
            var teams = new List<object>();
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Teams";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            teams.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            });
                        }
                    }
                }
            }
            return Ok(new { teams });
        }
    }
}