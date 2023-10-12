using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Services.Interfaces
{
    public interface IGameSessionService
    {
        string GameSessionId { get; set; }

        void CreateNewGameSession(string oldGameName);
        Task<GameSession> JoinGameSession(string gameSessionId, string playerId);
        Task<GameSession> UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement);
        Task<GameSession> UpdatePlayerHealth(string gameSessionId, string playerId);
        Task CheckGameLobby();
        void StartGameLobby();
        void StartGameSession();
    }
}