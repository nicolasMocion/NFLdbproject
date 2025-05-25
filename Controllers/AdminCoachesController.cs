using Microsoft.AspNetCore.Mvc;
using EspnBackend.Models; // Adjust if your models are elsewhere
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
    public class AdminCoachesController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCoachesController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CoachDTO>>> GetAllCoaches()
        {
            return await _context.Coaches
                .Include(c => c.Team)
                .Select(c => new CoachDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role,
                    Speciality = c.Speciality,
                    TeamName = c.Team.Name
                }).ToListAsync();
        }

        [HttpPost("dto")]
        public async Task<IActionResult> CreateCoach(CoachDTO dto)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (team == null) return BadRequest("Team not found");

            var coach = new Coach
            {
                Name = dto.Name,
                Role = dto.Role,
                Speciality = dto.Speciality,
                TeamId = team.Id
            };

            _context.Coaches.Add(coach);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateCoach(int id, CoachDTO dto)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null) return NotFound();

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (team == null) return BadRequest("Team not found");

            coach.Name = dto.Name;
            coach.Role = dto.Role;
            coach.Speciality = dto.Speciality;
            coach.TeamId = team.Id;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null) return NotFound();

            _context.Coaches.Remove(coach);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<IActionResult> GetDropdownData()
        {
            var teams = await _context.Teams
                .Select(t => new { t.Id, t.Name })
                .ToListAsync();

            return Ok(new { teams });
        }
    }
}