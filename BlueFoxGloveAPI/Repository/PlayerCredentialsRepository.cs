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
            _playersCredentialsCollection = mongoDatabase.GetCollection<PlayerCredentials>("PlayerCredentialsCollection");
        }
        public async Task CreatePlayer(PlayerCredentials newPlayer)
        {
            await _playersCredentialsCollection.InsertOneAsync(newPlayer);
        }
        public async Task<PlayerCredentials> GetPlayersCredentialsById(string playerId)
        {
            var filter = Builders<PlayerCredentials>.Filter.Eq(field => field.PlayerId, playerId);

            var result = await _playersCredentialsCollection.FindAsync(filter);

            return await result.SingleAsync();
        }
        public async Task UpdatePlayerCredentials(PlayerCredentials playerToUpdate)
        {

        }
        public async Task DeletePlayerCredentials(string playerId)
        {

        }
    }
}