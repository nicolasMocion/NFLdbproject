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
            var players = await _context.Players
                .Include(p => p.Team)
                .Include(p => p.College)
                .Include(p => p.HomeTownCity)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Position,
                    TeamName = p.Team.Name,
                    CollegeName = p.College.Name,
                    CityName = p.HomeTownCity.Name
                })
                .ToListAsync();

            return Ok(players);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> CreatePlayer(PlayerDto dto)
        {
            var player = new Player
            {
                Name = dto.Name,
                Position = dto.Position,
                CollegeId = dto.CollegeId,
                TeamId = dto.TeamId,
                HomeTownCityId = dto.HomeTownCityId
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("dto/{id}")]
        public async Task<ActionResult> UpdatePlayer(int id, PlayerDto dto)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            player.Name = dto.Name;
            player.Position = dto.Position;
            player.CollegeId = dto.CollegeId;
            player.TeamId = dto.TeamId;
            player.HomeTownCityId = dto.HomeTownCityId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var teams = await _context.Teams.Select(t => new { t.Id, t.Name }).ToListAsync();
            var colleges = await _context.Colleges.Select(c => new { c.Id, c.Name }).ToListAsync();
            var cities = await _context.Cities.Select(c => new { c.Id, c.Name }).ToListAsync();

            return Ok(new { teams, colleges, cities });
        }
    }
}
 