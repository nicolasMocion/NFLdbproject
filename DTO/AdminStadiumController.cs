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
    public class AdminStadiumController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminStadiumController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllStadiums()
        {
            var stadiums = await _context.Stadiums
                .Include(s => s.CityName)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Capacity,
                    CityName = s.CityName.Name
                })
                .ToListAsync();

            return Ok(stadiums);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> CreateStadium(StadiumDTO dto)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == dto.CityName);
            if (city == null) return BadRequest($"City '{dto.CityName}' not found");

            var stadium = new Stadium
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                CityId = city.Id
            };

            _context.Stadiums.Add(stadium);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdateStadium(int id, StadiumDTO dto)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null) return NotFound();

            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == dto.CityName);
            if (city == null) return BadRequest($"City '{dto.CityName}' not found");

            stadium.Name = dto.Name;
            stadium.Capacity = dto.Capacity;
            stadium.CityId = city.Id;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStadium(int id)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null) return NotFound();

            _context.Stadiums.Remove(stadium);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var cities = await _context.Cities.Select(c => new { c.Id, c.Name }).ToListAsync();
            return Ok(new { cities });
        }
    }
}