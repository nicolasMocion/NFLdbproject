using Microsoft.AspNetCore.Mvc;
using EspnBackend.DTO;
using EspnBackend.Data;
using EspnBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPlayerTeamHistoryController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminPlayerTeamHistoryController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllHistory()
        {
            var history = await _context.PlayerTeamHistory
                .Include(h => h.Player)
                .Include(h => h.Team)
                .Select(h => new
                {
                    PlayerName = h.Player != null ? h.Player.Name : "Unknown",
                    TeamName = h.Team != null ? h.Team.Name : "Unknown",
                    h.StartDate,
                    h.EndDate
                })
                .ToListAsync();

            return Ok(history);
        }

        [HttpPost("dto")]
        public async Task<ActionResult> Create(PlayerTeamHistoryDTO dto)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == dto.PlayerName);
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (player == null || team == null) return BadRequest("Player or Team not found");

            var history = new PlayerTeamHistory
            {
                PlayerId = player.Id,
                TeamId = team.Id,
                StartDate = DateTime.Parse(dto.StartDate),
                EndDate = string.IsNullOrEmpty(dto.EndDate) ? null : DateTime.Parse(dto.EndDate)
            };

            _context.PlayerTeamHistory.Add(history);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string playerName, string teamName)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == playerName);
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName);
            if (player == null || team == null) return NotFound();

            var history = await _context.PlayerTeamHistory
                .FirstOrDefaultAsync(pth => pth.PlayerId == player.Id && pth.TeamId == team.Id);
            if (history == null) return NotFound();

            _context.PlayerTeamHistory.Remove(history);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var players = await _context.Players
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            var teams = await _context.Teams
                .Select(t => new { t.Id, t.Name })
                .ToListAsync();

            return Ok(new { players, teams });
        }
            }
}