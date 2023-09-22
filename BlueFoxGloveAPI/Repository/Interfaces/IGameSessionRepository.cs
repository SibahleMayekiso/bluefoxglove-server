using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IGameSessionRepository
    {
        Task<List<GameSession>> GetAllAsync();
        Task<List<Player>> GetPlayersByIdAsync(string GameSessionId, string PlayerId);
        Task CreateNewGameSessionAsync(GameSession newGameSession);
        Task UpdateGameTimeAsync(string gameSessionId, DateTime newTimeStamp);
        Task UpdatedPlayerPositionAysnc(string gameSessionId, string playerId, int newX, int newY);
        Task DeleteGamePlayersAsync(Player playersRemoved);
        Task<GameSession> GetGameSessionById(string gameSessionId);
        Task<GameSession> UpdateGameSessionAsync(GameSession gameSession, Player newPlayer);
    }
}