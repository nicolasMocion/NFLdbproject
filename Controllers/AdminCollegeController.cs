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
    public class AdminCollegeController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCollegeController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllColleges()
        {
            var colleges = await _context.Colleges
                .Include(c => c.City)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    CityName = c.City.Name
                })
                .ToListAsync();

            return Ok(colleges);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> CreateCollege(CollegeDTO dto)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == dto.CityName);
            if (city == null) return BadRequest($"City '{dto.CityName}' not found");

            var college = new College
            {
                Name = dto.Name,
                CityId = city.Id
            };

            _context.Colleges.Add(college);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdateCollege(int id, CollegeDTO dto)
        {
            var college = await _context.Colleges.FindAsync(id);
            if (college == null) return NotFound();

            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == dto.CityName);
            if (city == null) return BadRequest($"City '{dto.CityName}' not found");

            college.Name = dto.Name;
            college.CityId = city.Id;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCollege(int id)
        {
            var college = await _context.Colleges.FindAsync(id);
            if (college == null) return NotFound();

            _context.Colleges.Remove(college);
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