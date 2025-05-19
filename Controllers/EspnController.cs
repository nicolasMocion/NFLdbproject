using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EspnBackend.Data;
using EspnBackend.Models;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFLGameController : ControllerBase
    {
        private readonly EspnDbContext _context;

        public NFLGameController(EspnDbContext context)
        {
            _context = context;
        }

        // GET: api/NFLGame
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NFLGame>>> GetNFLGames()
        {
            return await _context.NFLGames.ToListAsync();
        }

        // GET: api/NFLGame/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NFLGame>> GetNFLGame(int id)
        {
            var nflGame = await _context.NFLGames.FindAsync(id);

            if (nflGame == null)
            {
                return NotFound();
            }

            return nflGame;
        }

        // POST: api/NFLGame
        [HttpPost]
        public async Task<ActionResult<NFLGame>> PostNFLGame(NFLGame nflGame)
        {
            _context.NFLGames.Add(nflGame);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNFLGame), new { id = nflGame.Id }, nflGame);
        }

        // PUT: api/NFLGame/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNFLGame(int id, NFLGame nflGame)
        {
            if (id != nflGame.Id)
            {
                return BadRequest();
            }

            _context.Entry(nflGame).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/NFLGame/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNFLGame(int id)
        {
            var nflGame = await _context.NFLGames.FindAsync(id);
            if (nflGame == null)
            {
                return NotFound();
            }

            _context.NFLGames.Remove(nflGame);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
