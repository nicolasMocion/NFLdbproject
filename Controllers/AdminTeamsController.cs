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
    public class AdminTeamsController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminTeamsController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAllTeams()
        {
            return await _context.Teams
                .Include(t => t.City)
                .Include(t => t.Stadium)
                .Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Stadium = t.Stadium.Name,
                    City = t.City.Name
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamDto teamDto)
        {
            var stadium = await _context.Stadiums.FirstOrDefaultAsync(s => s.Name == teamDto.Stadium);
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == teamDto.City);

            if (stadium == null || city == null)
                return BadRequest("Invalid stadium or city name.");

            var team = new Team
            {
                Name = teamDto.Name,
                Stadium = stadium,
                City = city
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllTeams), new { id = team.Id }, teamDto);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, TeamDto dto)
        {
            var team = await _context.Teams.Include(t => t.Stadium).Include(t => t.City).FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();

            var stadium = await _context.Stadiums.FirstOrDefaultAsync(s => s.Name == dto.Stadium);
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == dto.City);

            if (stadium == null || city == null)
                return BadRequest("Invalid stadium or city name.");

            team.Name = dto.Name;
            team.Stadium = stadium;
            team.City = city;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<IActionResult> GetDropdownData()
        {
            var cities = await _context.Cities.Select(c => new { c.Id, c.Name }).ToListAsync();
            var stadiums = await _context.Stadiums.Select(s => new { s.Id, s.Name }).ToListAsync();

            return Ok(new { cities, stadiums });
        }
    }
}