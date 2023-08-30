using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoCollection<Player> _playersCollection;
        public PlayerRepository(IMongoDatabase mongoDatabase)
        {
            _playersCollection = mongoDatabase.GetCollection<Player>("players");
        }
        public async Task CreateNewPlayerAsync(Player newPlayer)
        {

        }
        public async Task<Player> GetPlayerByIdAsync(string playerId)
        {
            return await _playersCollection.Find(player => player.PlayerId == playerId).FirstOrDefaultAsync();
        }
        public async Task UpdatePlayerAsync(Player playerToUpdate)
        {

        }
        public Task<List<Player>> GetAllPlayersAsync()
        {
            return null;
        }
        public async Task DeletePlayerAsync(string playerId)
        {

        }

    }
}
