using BlueFoxGloveAPI.Repository.Interfaces;

using BlueFoxGloveAPI.Models;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class PlayerRepository: IPlayerRepository
    {
        private readonly IMongoCollection<Player> _playersCollection;
        public PlayerRepository(IMongoDatabase mongoDatabase)
        {
     
        }
        public async Task CreateNewPlayerAsync(Player newPlayer)
        {
          
        }
        public async Task<List<Player>> GetPlayersByIdAsync()
        {
            return null; 
        }
        public async Task UpdatePlayerAsync(Player playerToUpdate)
        {
            
        }
        public async Task DeletePlayerAsync(string PlayerId)
        {
        }

        public Task<List<Player>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Player>> GetPlayersByIdAsync(string PlayerId)
        {
            throw new NotImplementedException();
        }
    }
}
