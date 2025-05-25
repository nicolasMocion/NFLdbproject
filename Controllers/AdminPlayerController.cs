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
    public class AdminPlayersController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminPlayersController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPlayers()
        {
            var players = new List<object>();
            var sql = @"
                SELECT p.Id, p.Name, p.Position, t.Name AS TeamName, c.Name AS CollegeName, ci.Name AS CityName
                FROM Players p
                LEFT JOIN Teams t ON p.TeamId = t.Id
                LEFT JOIN Colleges c ON p.CollegeId = c.Id
                LEFT JOIN Cities ci ON p.HomeTownCityId = ci.Id
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
                            players.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Position = reader.GetString(reader.GetOrdinal("Position")),
                                TeamName = reader.GetString(reader.GetOrdinal("TeamName")),
                                CollegeName = reader.GetString(reader.GetOrdinal("CollegeName")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName"))
                            });
                        }
                    }
                }
            }

            return Ok(players);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> CreatePlayer(PlayerDto dto)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Players (Name, Position, CollegeId, TeamId, HomeTownCityId)
                        VALUES (@name, @position, @collegeId, @teamId, @cityId)
                    ";
                    
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@position"; p2.Value = dto.Position; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@collegeId"; p3.Value = dto.CollegeId; cmd.Parameters.Add(p3);
                    var p4 = cmd.CreateParameter(); p4.ParameterName = "@teamId"; p4.Value = dto.TeamId; cmd.Parameters.Add(p4);
                    var p5 = cmd.CreateParameter(); p5.ParameterName = "@cityId"; p5.Value = dto.HomeTownCityId; cmd.Parameters.Add(p5);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdatePlayer(int id, PlayerDto dto)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Players
                        SET Name = @name, Position = @position, CollegeId = @collegeId, TeamId = @teamId, HomeTownCityId = @cityId
                        WHERE Id = @id
                    ";
                    
                    var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name; cmd.Parameters.Add(p1);
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@position"; p2.Value = dto.Position; cmd.Parameters.Add(p2);
                    var p3 = cmd.CreateParameter(); p3.ParameterName = "@collegeId"; p3.Value = dto.CollegeId; cmd.Parameters.Add(p3);
                    var p4 = cmd.CreateParameter(); p4.ParameterName = "@teamId"; p4.Value = dto.TeamId; cmd.Parameters.Add(p4);
                    var p5 = cmd.CreateParameter(); p5.ParameterName = "@cityId"; p5.Value = dto.HomeTownCityId; cmd.Parameters.Add(p5);
                    var p6 = cmd.CreateParameter(); p6.ParameterName = "@id"; p6.Value = id; cmd.Parameters.Add(p6);

                    var affected = await cmd.ExecuteNonQueryAsync();
                    if (affected == 0)
                        return NotFound();
                }
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayer(int id)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Players WHERE Id = @id";
                    var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
                    var affected = await cmd.ExecuteNonQueryAsync();
                    if (affected == 0)
                        return NotFound();
                }
            }
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var teams = new List<object>();
            var colleges = new List<object>();
            var cities = new List<object>();

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

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Colleges";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            colleges.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            });
                        }
                    }
                }

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

            return Ok(new { teams, colleges, cities });
        }
    }
}