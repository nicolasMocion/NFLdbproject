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
    public class AdminTeamTitleController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public AdminTeamTitleController(EspnDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTeamTitles()
        {
            var teamTitles = await _context.TeamTitles
                .Include(tt => tt.Team)
                .Include(tt => tt.Title)
                .Select(tt => new
                {
                    TeamName = tt.Team.Name,
                    TitleName = tt.Title.Name,
                    tt.YearWon
                })
                .ToListAsync();

            return Ok(teamTitles);
        }

        [HttpPost("dto")]
public async Task<ActionResult> CreateTeamTitle(TeamTitleDTO dto)
{
    // Find team and title by name
    var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
    if (team == null) return BadRequest($"Team '{dto.TeamName}' not found");

    var title = await _context.Titles.FirstOrDefaultAsync(t => t.Name == dto.TitleName);
    if (title == null) return BadRequest($"Title '{dto.TitleName}' not found");

    // Check if the combination already exists
    var existing = await _context.TeamTitles
        .AnyAsync(tt => tt.TeamId == team.Id && tt.TitleId == title.Id);

    if (existing)
        return Conflict($"Team '{dto.TeamName}' already has the title '{dto.TitleName}'.");

    var teamTitle = new TeamTitle
    {
        TeamId = team.Id,
        TitleId = title.Id,
        YearWon = dto.YearWon
    };

    _context.TeamTitles.Add(teamTitle);
    await _context.SaveChangesAsync();

    return Ok();
}

        [HttpPut("dto/{teamId}/{titleId}")]
public async Task<ActionResult> UpdateTeamTitle(int teamId, int titleId, TeamTitleDTO dto)
{
    // First find the existing record using the original composite key
    var existingTeamTitle = await _context.TeamTitles
        .FirstOrDefaultAsync(tt => tt.TeamId == teamId && tt.TitleId == titleId);
        
    if (existingTeamTitle == null) return NotFound();

    // Find the new team and title (if they're being changed)
    var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.TeamName);
    if (team == null) return BadRequest($"Team '{dto.TeamName}' not found");

    var title = await _context.Titles.FirstOrDefaultAsync(t => t.Name == dto.TitleName);
    if (title == null) return BadRequest($"Title '{dto.TitleName}' not found");

    // Check if we're changing the team or title association
    if (team.Id != teamId || title.Id != titleId)
    {
        // Check if the new combination already exists
        var exists = await _context.TeamTitles
            .AnyAsync(tt => tt.TeamId == team.Id && tt.TitleId == title.Id);
            
        if (exists)
        {
            return BadRequest("This team-title combination already exists");
        }

        // Remove the old record and create a new one
        _context.TeamTitles.Remove(existingTeamTitle);
        
        var newTeamTitle = new TeamTitle
        {
            TeamId = team.Id,
            TitleId = title.Id,
            YearWon = dto.YearWon
        };
        
        _context.TeamTitles.Add(newTeamTitle);
    }
    else
    {
        // Just update the year if the team/title association isn't changing
        existingTeamTitle.YearWon = dto.YearWon;
    }

    await _context.SaveChangesAsync();
    return Ok();
}

        [HttpDelete("{teamId}/{titleId}")]
        public async Task<ActionResult> DeleteTeamTitle(int teamId, int titleId)
        {
            var teamTitle = await _context.TeamTitles
                .FirstOrDefaultAsync(tt => tt.TeamId == teamId && tt.TitleId == titleId);
                
            if (teamTitle == null) return NotFound();

            _context.TeamTitles.Remove(teamTitle);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("dropdown-data")]
        public async Task<ActionResult> GetDropdownData()
        {
            var teams = await _context.Teams.Select(t => new { t.Id, t.Name }).ToListAsync();
            var titles = await _context.Titles.Select(t => new { t.Id, t.Name }).ToListAsync();

            return Ok(new { teams, titles });
        }

         [HttpGet("get-ids")]
    public async Task<ActionResult> GetIdsByName(string teamName, string titleName)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName);
        if (team == null) return NotFound();
        
        var title = await _context.Titles.FirstOrDefaultAsync(t => t.Name == titleName);
        if (title == null) return NotFound();

        return Ok(new { teamId = team.Id, titleId = title.Id });
    }

    }

    
}