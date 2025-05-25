using Microsoft.AspNetCore.Mvc;
using EspnBackend.Models;
using EspnBackend.Data;
using EspnBackend.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCityController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCityController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCities()
        {
            var cities = await _context.Cities
                .FromSqlRaw("SELECT Id, Name, Population FROM Cities")
                .Select(c => new {
                    c.Id,
                    c.Name,
                    c.Population
                })
                .ToListAsync();

            return Ok(cities);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> CreateCity(CityDTO dto)
        {
            var sql = "INSERT INTO Cities (Name, Population) VALUES ({0}, {1})";
            await _context.Database.ExecuteSqlRawAsync(sql, dto.Name, dto.Population);
            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdateCity(int id, CityDTO dto)
        {
            // Check existence
            var exists = await _context.Cities
                .FromSqlRaw("SELECT * FROM Cities WHERE Id = {0}", id)
                .AnyAsync();

            if (!exists) return NotFound();

            var sql = "UPDATE Cities SET Name = {0}, Population = {1} WHERE Id = {2}";
            await _context.Database.ExecuteSqlRawAsync(sql, dto.Name, dto.Population, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(int id)
        {
            // Check existence
            var exists = await _context.Cities
                .FromSqlRaw("SELECT * FROM Cities WHERE Id = {0}", id)
                .AnyAsync();

            if (!exists) return NotFound();

            var sql = "DELETE FROM Cities WHERE Id = {0}";
            await _context.Database.ExecuteSqlRawAsync(sql, id);
            return Ok();
        }
    }
}