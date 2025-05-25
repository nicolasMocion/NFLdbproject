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
                .Select(c => new
                {
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
            var city = new City
            {
                Name = dto.Name,
                Population = dto.Population
            };

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdateCity(int id, CityDTO dto)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();

            city.Name = dto.Name;
            city.Population = dto.Population;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}