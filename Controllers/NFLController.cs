using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using EspnBackend.Data;
using EspnBackend.Models; 
using System.Text.Json;
using System.Linq;
using System;

namespace EspnBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFLController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly EspnDbContext _context; 

        public NFLController(IHttpClientFactory httpClientFactory, EspnDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient("ESPN");
            _context = context; 
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetNFLGames()
        {
            var response = await _httpClient.GetAsync("sports/football/nfl/scoreboard?seasontype=2&season=2024&week=1");


            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch NFL data from ESPN API");
            }
        }

        [HttpGet("random-games")]
        public IActionResult GetRandomNFLGames()
        {
            // Get all NFL games from the database
            var allGames = _context.NFLGames.ToList();

            foreach (var game in allGames)
        {
            Console.WriteLine($"Game: {game.HomeTeam} vs {game.AwayTeam} on {game.GameDate}");
        }

            // Check if there are enough games in the database
            if (allGames.Count == 0)
            {
                return NotFound("No NFL games found in the database.");
            }

            // Randomly select 30 games from the list (or fewer if less than 30)
            var randomGames = allGames.OrderBy(g => Guid.NewGuid()).Take(30).ToList();

            return Ok(randomGames);
        }

        

    }
}
