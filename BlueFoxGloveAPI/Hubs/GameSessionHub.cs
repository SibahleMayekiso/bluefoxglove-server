using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services;
using BlueFoxGloveAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlueFoxGloveAPI.Hubs
{
    public class GameSessionHub: Hub
    {
        private readonly IGameSessionService _gameSessionService;

        private readonly IGameSessionRepository _gameSessionRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameSessionHub(IGameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
        }

        public GameSessionHub(IGameSessionRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameSessionRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var sessionId = httpContext.Request.Query["sessionId"];
            _gameSessionService.GameSessionId = sessionId;

            await _gameSessionService.CheckGameLobby();

            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await base.OnConnectedAsync();
        }

        private (int x, int y) GenrateRandomPlayerPosition()
        {
            Random random = new Random();
            int randomXPosition = random.Next(0, 1280);
            int randomPosition = random.Next(0, 720);

            return (randomXPosition, randomPosition);
        }

        public async Task JoinGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = await _playerRepository.GetPlayerById(playerId);

            var playerCoordinates = GenrateRandomPlayerPosition();
            player.PlayerXCoordinate = playerCoordinates.x;
            player.PlayerYCoordinate = playerCoordinates.y;

            var updatedGameSession = await _gameSessionRepository.UpdateGameSession(gameSession, player);

            await Clients.Group(gameSessionId).SendAsync("PlayerJoiningGame", updatedGameSession);
        }

        public async Task AddScoreBoardInGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = await _playerRepository.GetPlayerById(playerId);
            player.PlayerScore += 5;

            var newGameSession = await _gameSessionRepository.UpdateGameSession(gameSession, player);

            await Clients.Group(gameSessionId).SendAsync("UpdateLeaderBoard", newGameSession.PlayersJoiningSession);
        }

        public async Task UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement)
        {
            var updatedGameSession = await _gameSessionService.UpdatePlayerPostion(gameSessionId, playerId, playerMovement);

            await Clients.Group(gameSessionId).SendAsync("UpdatePlayerPosition", updatedGameSession);
        }

        public async Task UpdatePlayerHealth(string gameSessionId, string playerId)
        {
            var updatedGameSession = await _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId);

            await Clients.Group(gameSessionId).SendAsync("UpdatePlayerHealth", updatedGameSession);
        }
    }
}