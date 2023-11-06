using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class PlayerProfileRepository: IPlayerProfileRepository
    {
        private readonly IMongoCollection<PlayerProfile> _playerProfileCollection;
        private readonly IMongoCollection<Characters> _characterCollection;

        public PlayerProfileRepository(IMongoDatabase mongoDatabase)
        {
            _playerProfileCollection = mongoDatabase.GetCollection<PlayerProfile>("PlayerProfileCollection");
            _characterCollection = mongoDatabase.GetCollection<Characters>("CharactersCollection");
        }

        public async Task<List<PlayerProfile>> GetPlayerProfileById(string playerId)
        {
            var result = await _playerProfileCollection.Find(playerProfile => playerProfile.Credentials.PlayerId == playerId).ToListAsync();

            return result;
        }

        public async Task CreatePlayerProfile(PlayerProfile playerProfile)
        {
            await _playerProfileCollection.InsertOneAsync(playerProfile);
        }

        public async Task<PlayerProfile> UpdateSelectedCharacter(string playerId, string characterId)
        {
            var characterFilter = Builders<Characters>.Filter.Eq(filed => filed.CharacterId, characterId);
            var character = await _characterCollection.FindAsync(characterFilter);
            var value = await character.SingleAsync();

            var playerProfileFilter = Builders<PlayerProfile>.Filter.Eq(field => field.Credentials.PlayerId, playerId);
            var update = Builders<PlayerProfile>.Update.Set(field => field.SelectedCharacter, value);

            return await _playerProfileCollection.FindOneAndUpdateAsync(playerProfileFilter, update);
        }
    }
}