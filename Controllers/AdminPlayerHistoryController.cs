using Microsoft.AspNetCore.Mvc;
using EspnBackend.DTO;
using EspnBackend.Data;
using EspnBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EspnBackend.Controllerså
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
                    playerId = h.PlayerId,
                    teamId = h.TeamId,
                    startDate = h.StartDate.ToString("yyyy-MM-dd"),
                    endDate = h.EndDate != null ? ((DateTime)h.EndDate).ToString("yyyy-MM-dd") : null,
                    playerName = h.Player != null ? h.Player.Name : "Unknown",
                    teamName = h.Team != null ? h.Team.Name : "Unknown"
                })
                .ToListAsync();

            return Ok(history);
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

        [HttpPost("dto")]
        public async Task<ActionResult> Create(PlayerTeamHistoryDTO dto)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == dto.PlayerName);
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (player == null || team == null) return BadRequest("Player or Team not found");

            if (!DateTime.TryParse(dto.StartDate, out DateTime startDate))
                return BadRequest("Invalid StartDate format");

            DateTime? endDate = null;
            if (!string.IsNullOrEmpty(dto.EndDate))
            {
                if (!DateTime.TryParse(dto.EndDate, out DateTime parsedEndDate))
                    return BadRequest("Invalid EndDate format");
                endDate = parsedEndDate;
            }

            var history = new PlayerTeamHistory
            {
                PlayerId = player.Id,
                TeamId = team.Id,
                StartDate = startDate,
                EndDate = endDate
            };

            _context.PlayerTeamHistory.Add(history);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("dto/{playerId:int}/{teamId:int}/{startDate}")]
        public async Task<ActionResult> UpdatePlayerTeamHistory(int playerId, int teamId, string startDate, PlayerTeamHistoryDTO dto)
        {
            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                return BadRequest("Invalid StartDate in URL");

            var history = await _context.PlayerTeamHistory
                .FirstOrDefaultAsync(h => h.PlayerId == playerId && h.TeamId == teamId && h.StartDate == parsedStartDate);
            if (history == null) return NotFound();

            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == dto.PlayerName);
            if (player == null) return BadRequest("Player not found");

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (team == null) return BadRequest("Team not found");

            if (!DateTime.TryParse(dto.StartDate, out DateTime newStartDate))
                return BadRequest("Invalid StartDate format in body");

            DateTime? endDate = null;
            if (!string.IsNullOrEmpty(dto.EndDate))
            {
                if (!DateTime.TryParse(dto.EndDate, out DateTime parsedEndDate))
                    return BadRequest("Invalid EndDate format");
                endDate = parsedEndDate;
            }

            // If you allow changing the PK, you should delete and re-add (or warn)
            history.PlayerId = player.Id;
            history.TeamId = team.Id;
            history.StartDate = newStartDate;
            history.EndDate = endDate;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{playerId:int}/{teamId:int}/{startDate}")]
        public async Task<ActionResult> Delete(int playerId, int teamId, string startDate)
        {
            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                return BadRequest("Invalid StartDate in URL");

            var history = await _context.PlayerTeamHistory
                .FirstOrDefaultAsync(h => h.PlayerId == playerId && h.TeamId == teamId && h.StartDate == parsedStartDate);
            if (history == null) return NotFound();

            _context.PlayerTeamHistory.Remove(history);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}