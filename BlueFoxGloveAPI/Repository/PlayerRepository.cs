using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class PlayerRepository: IPlayerRepository
    {
        private readonly IMongoCollection<Player> _playersCollection;
        public PlayerRepository(IMongoDatabase mongoDatabase)
        {
            _playersCollection = mongoDatabase.GetCollection<Player>("players");
        }
        public async Task CreateNewPlayer(Player newPlayer)
        {

        }
        public async Task<Player> GetPlayerById(string playerId)
        {
            return await _playersCollection.Find(player => player.Credentials.PlayerId == playerId).FirstOrDefaultAsync();
        }
        public async Task UpdatePlayer(Player playerToUpdate)
        {

        }
        public Task<List<Player>> GetAllPlayers()
        {
            return null;
        }
        public async Task DeletePlayer(string playerId)
        {

        }
    }
}