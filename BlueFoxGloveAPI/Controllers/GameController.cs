using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class GameController: ControllerBase
    {
        private readonly IGameSessionRepository _gameSessionRepository;
        private readonly IGameSessionService _gameSessionService;

        public GameController(IGameSessionRepository gameSessionRepository, IGameSessionService gameSessionService)
        {
            _gameSessionRepository = gameSessionRepository;
            _gameSessionService = gameSessionService;
        }

        [HttpPost("[controller]/gamesession")]
        public async Task<IActionResult> CreateGameSession(GameSession newGameSession)
        {
            if (string.IsNullOrWhiteSpace(newGameSession.GameName) ||
               newGameSession.PlayersJoiningSession == null ||
               !newGameSession.PlayersJoiningSession.Any())
            {
                return BadRequest("Invalid or incomplete game session data");
            }

            await _gameSessionRepository.CreateNewGameSession(newGameSession);

            return CreatedAtAction(nameof(GetAllGameSessions), newGameSession);
        }

        [HttpGet("[controller]/gamesession")]
        public Task<IActionResult> GetAllGameSessions()
        {
            return null;
        }

        [HttpGet("[controller]/survivingPlayers")]
        public IActionResult GetSurvivingPlayersInGameSession()
        {
            return _gameSessionService.SurvivngPlayers == null ? BadRequest("No Surviving players could be found") : Ok(_gameSessionService.SurvivngPlayers);
        }
    }
}