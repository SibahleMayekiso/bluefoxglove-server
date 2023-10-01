using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Services.Interfaces
{
    public interface IGameSessionService
    {
        void CreateNewGameSession(string oldGameName);
        Task JoinGameSession(string gameSessionId, string playerId);
        Task<GameSession> UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement);
        Task<GameSession> UpdatePlayerHealth(string gameSessionId, string playerId);
    }
}