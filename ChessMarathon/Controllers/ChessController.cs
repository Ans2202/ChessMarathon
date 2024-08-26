using ChessMarathon.DAO;
using ChessMarathon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChessMarathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private readonly IChessDao _chessDao;

        public ChessController(IChessDao chessDao)
        {
            _chessDao = chessDao;
        }

        // Endpoint to get a player by ID
        [HttpGet("player/{id:int}", Name = "GetMatchById")]
        public async Task<ActionResult<Matches?>> GetMatchById(int id)
        {
            Matches? player = await _chessDao.GetMatchById(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        // Endpoint to insert a new player
        [HttpPost("player")]
        public async Task<ActionResult<int>> InsertPlayer(Matches p)
        {
            if (p != null)
            {
                if (ModelState.IsValid)
                {
                    int res = await _chessDao.InsertMatch(p);
                    if (res > 0)
                    {
                        return CreatedAtRoute(nameof(GetMatchById), new { id = p.MatchId }, p);
                    }
                }
                return BadRequest("Failed to add player");
            }
            else
            {
                return BadRequest();
            }
        }
        //2nd

        [HttpGet("ByCountry/{country}")]
        public async Task<ActionResult<List<Players>>> GetPlayersbyCountry(string country)
        {
            var players = await _chessDao.GetPlayersbyCountry(country);

            if (players == null || players.Count == 0)
            {
                return NotFound($"No players found for country: {country}");
            }

            return Ok(players);
        }

        // Endpoint to get player performances
        [HttpGet("playerPerformances")]
        public async Task<ActionResult<List<PlayerPerformance>>> GetPlayerPerformances()
        {
            List<PlayerPerformance> playerPerformances = await _chessDao.GetPlayerPerformances();
            if (playerPerformances == null || playerPerformances.Count == 0)
            {
                return NotFound();
            }
            return Ok(playerPerformances);
        }

        // Endpoint to get top players by win percentage
        [HttpGet("topPlayersByWinPercentage")]
        public async Task<ActionResult<List<PlayerPerformance>>> GetTopPlayersByWinPercentage([FromQuery] double minWinPercentage)
        {
            List<PlayerPerformance> topPlayers = await _chessDao.GetTopPlayersByWinPercentage();
            if (topPlayers == null || topPlayers.Count == 0)
            {
                return NotFound();
            }
            return Ok(topPlayers);
        }

       
       
    }
}

