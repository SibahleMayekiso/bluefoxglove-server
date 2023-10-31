using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IGameSessionRepository
    {
        Task<List<GameSession>> GetAll();
        Task<List<Player>> GetPlayersById(string GameSessionId, string PlayerId);
        Task CreateNewGameSession(GameSession newGameSession);
        Task UpdateGameTime(string gameSessionId, DateTime newTimeStamp);
        Task UpdatedPlayerPosition(string gameSessionId, string playerId, int newX, int newY);
        Task DeleteGamePlayers(Player playersRemoved);
        Task<GameSession> GetGameSessionById(string gameSessionId);
        Task<GameSession> UpdateGameSession(GameSession gameSession, Player newPlayer);
        Task<GameSession> RemovePlayerFromGameSession(string gameSessionId, string playerId);
    }
}