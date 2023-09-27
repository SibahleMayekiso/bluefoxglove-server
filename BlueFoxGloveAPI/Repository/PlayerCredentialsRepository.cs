using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public sealed class PlayerCredentialsRepository: IPlayerCredentialsRepository
    {
        private readonly IMongoCollection<PlayerCredentials> _playersCredentialsCollection;
        public PlayerCredentialsRepository(IMongoDatabase mongoDatabase)
        {
            _playersCredentialsCollection = mongoDatabase.GetCollection<PlayerCredentials>("playersCredentialsCollection");
        }
        public async Task CreatePlayer(PlayerCredentials newPlayer)
        {
            await _playersCredentialsCollection.InsertOneAsync(newPlayer);
        }
        public async Task<PlayerCredentials> GetPlayersCredentialsById(string playerId)
        {
            return null;
        }
        public async Task UpdatePlayerCredentials(PlayerCredentials playerToUpdate)
        {

        }
        public async Task DeletePlayerCredentials(string playerId)
        {

        }
    }
}