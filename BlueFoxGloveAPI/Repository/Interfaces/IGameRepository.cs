using BlueFoxGloveAPI.Models;

 namespace BlueFoxGloveAPI.Repository.Interfaces
  {
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync();
        Task<List<Player>> GetPlayersByIdAsync(string GameSessionId, string PlayerId);
        Task CreateNewGameAsync(Game newGame);
        Task UpdateGameTimeAsync(string gameSessionId, DateTime newTimeStamp);
        Task UpdatedPlayerPositionAysnc(string gameSessionId, string playerId, int newX, int newY);
        Task DeleteGamePlayersAsync(Player playersRemoved);
    }
 }
