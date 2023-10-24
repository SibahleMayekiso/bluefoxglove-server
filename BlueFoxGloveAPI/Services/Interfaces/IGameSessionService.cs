using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;
using System.Collections.Concurrent;

namespace BlueFoxGloveAPI.Services.Interfaces
{
    public interface IGameSessionService
    {
        string GameSessionId { get; set; }
        ConcurrentDictionary<int, Projectile> ProjectilesInPlay { get; set; }

        void CreateNewGameSession(string oldGameName);
        Task<GameSession> JoinGameSession(string gameSessionId, string playerId);
        Task<GameSession> UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement);
        Task<GameSession> UpdatePlayerHealth(string gameSessionId, string playerId);
        Task CheckGameLobby();
        void StartGameLobby();
        void StartGameSession();
        Task<GameSession> AddScoreBoardInGameSession(string gameSessionId, string playerId);

        Projectile CreateProjectile(string playerId, Vector position, Vector velocity);

        void FireProjectile(string playerId, Vector position, Vector velocity);
        void DisposeProjectile(int projectileId);
        void UpdateProjectilePosition(int projectileId);
    }
}