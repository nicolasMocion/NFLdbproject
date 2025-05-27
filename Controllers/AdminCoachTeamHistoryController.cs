using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Data;
using EspnBackend.Models;
using EspnBackend.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCoachTeamHistoryController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminCoachTeamHistoryController(EspnDbContext context)
        {
            _context = context;
        }

        #region GET

        [HttpGet("all")]
public async Task<ActionResult<IEnumerable<object>>> GetAllCoachTeamHistories()
{
    var histories = await _context.CoachTeamHistory
        .Include(h => h.Coach)
        .Include(h => h.Team)
        .Select(h => new
        {
            coachId = h.CoachId,
            teamId = h.TeamId,
            startDate = h.StartDate.ToString("yyyy-MM-dd"),
            endDate = h.EndDate != null ? ((DateTime)h.EndDate).ToString("yyyy-MM-dd") : null,
            coachName = h.Coach.Name,
            teamName = h.Team.Name
        })
        .ToListAsync();

    return Ok(histories);
}

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var coaches = await _context.Coaches
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            var teams = await _context.Teams
                .Select(t => new { t.Id, t.Name })
                .ToListAsync();

            return Ok(new { coaches, teams });
        }

        #endregion

        #region POST

        [HttpPost("dto")]
        public async Task<ActionResult> CreateCoachTeamHistory(CoachTeamHistoryDTO dto)
        {
            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Name == dto.CoachName);
            if (coach == null) return BadRequest("Coach not found");

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
            if (team == null) return BadRequest("Team not found");

            if (!DateTime.TryParse(dto.StartDate, out DateTime startDate))
                return BadRequest("Invalid StartDate format");

            DateTime? endDate = null;
            if (!string.IsNullOrEmpty(dto.EndDate))
            {
                if (!DateTime.TryParse(dto.EndDate, out DateTime parsedEndDate))
                    return BadRequest("Invalid EndDate format");
                endDate = parsedEndDate;
            }

            var history = new CoachTeamHistory
            {
                CoachId = coach.Id,
                TeamId = team.Id,
                StartDate = startDate,
                EndDate = endDate
            };

            _context.CoachTeamHistory.Add(history);
            await _context.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region PUT

        [HttpPut("dto/{coachId:int}/{teamId:int}/{startDate}")]
public async Task<ActionResult> UpdateCoachTeamHistory(int coachId, int teamId, string startDate, CoachTeamHistoryDTO dto)
{
    if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
        return BadRequest("Invalid StartDate in URL");

    var history = await _context.CoachTeamHistory
        .FirstOrDefaultAsync(h => h.CoachId == coachId && h.TeamId == teamId && h.StartDate == parsedStartDate);
    if (history == null) return NotFound();

    var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Name == dto.CoachName);
    if (coach == null) return BadRequest("Coach not found");

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
    history.CoachId = coach.Id;
    history.TeamId = team.Id;
    history.StartDate = newStartDate;
    history.EndDate = endDate;

    await _context.SaveChangesAsync();
    return Ok();
}
        #endregion

        #region DELETE

        [HttpDelete("{coachId:int}/{teamId:int}/{startDate}")]
public async Task<ActionResult> DeleteCoachTeamHistory(int coachId, int teamId, string startDate)
{
    if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
        return BadRequest("Invalid StartDate in URL");

    var history = await _context.CoachTeamHistory
        .FirstOrDefaultAsync(h => h.CoachId == coachId && h.TeamId == teamId && h.StartDate == parsedStartDate);
    if (history == null) return NotFound();

    _context.CoachTeamHistory.Remove(history);
    await _context.SaveChangesAsync();

    return Ok();
}

        #endregion
    }
}