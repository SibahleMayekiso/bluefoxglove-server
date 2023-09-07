using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class GameRepository: IGameRepository
    {
        private readonly IMongoCollection<Game> _gameCollection;
        private readonly IMongoCollection<Player> _playerCollection;
        public GameRepository(IMongoDatabase mongoDatabase)
        {

        }
        public async Task<List<Game>> GetAllAsync()
        {
            return null;
        }
        public async Task<List<Player>> GetPlayersByIdAsync(string GameSessionId, string PlayerId)
        {
            return null;
        }
        public async Task CreateNewGameAsync(Game newGame)
        {

        }
        public async Task UpdateGameTime(string gameSessionId, DateTime newTimeStamp)
        {

        }
        public async Task DeleteGamePlayersAsync(Player playersRemoved)
        {

        }

        public Task UpdateGameTimeAsync(string gameSessionId, DateTime newTimeStamp)
        {
            throw new NotImplementedException();
        }

        public Task UpdatedPlayerPositionAysnc(string gameSessionId, string playerId, int newX, int newY)
        {
            throw new NotImplementedException();
        }
    }
}