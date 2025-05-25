using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Data;
using EspnBackend.DTO;
using EspnBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPlayerStatsController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminPlayerStatsController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PlayerStatsDto>>> GetAllStats()
        {
            return await _context.PlayerStats
                .Include(ps => ps.Player)
                .Select(ps => new PlayerStatsDto
                {
                    Id = ps.Id,
                    PlayerName = ps.Player.Name,
                    Yards = ps.Yards,
                    Touchdowns = ps.Touchdowns,
                    Interceptions = ps.Interceptions
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerStatsDto dto)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == dto.PlayerName);
            if (player == null) return BadRequest("Player not found");

            var stats = new PlayerStats
            {
                Player = player,
                Yards = dto.Yards,
                Touchdowns = dto.Touchdowns,
                Interceptions = dto.Interceptions
            };

            _context.PlayerStats.Add(stats);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlayerStatsDto dto)
        {
            var stats = await _context.PlayerStats.FindAsync(id);
            if (stats == null) return NotFound();

            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == dto.PlayerName);
            if (player == null) return BadRequest("Player not found");

            stats.PlayerId = player.Id;
            stats.Yards = dto.Yards;
            stats.Touchdowns = dto.Touchdowns;
            stats.Interceptions = dto.Interceptions;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stats = await _context.PlayerStats.FindAsync(id);
            if (stats == null) return NotFound();

            _context.PlayerStats.Remove(stats);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-players")]
        public async Task<IActionResult> GetPlayerDropdown()
        {
            var players = await _context.Players
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            return Ok(players);
        }
    }
}