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

        public GameSessionHub(IGameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
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

        public async Task JoinGameSession(string gameSessionId, string playerId)
        {
            var updatedGameSession = await _gameSessionService.JoinGameSession(gameSessionId, playerId);

            await Clients.Group(gameSessionId).SendAsync("PlayerJoiningGame", updatedGameSession);
        }

        public async Task AddScoreBoardInGameSession(string gameSessionId, string playerId)
        {
            var newGameSession = await _gameSessionService.AddScoreBoardInGameSession(gameSessionId, playerId);

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

        public async Task FireProjectile(string gameSessionId, string playerId, Vector position, Vector velocity)
        {
            _gameSessionService.FireProjectile(playerId, position, velocity);

            await Clients.Group(gameSessionId).SendAsync("GetProjectilesInPlay", _gameSessionService.ProjectilesInPlay);
        }
    }
}