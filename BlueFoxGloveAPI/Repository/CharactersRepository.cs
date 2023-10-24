using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class CharactersRepository: ICharacterRepository
    {
        private readonly IMongoCollection<Characters> _charactersCollection;
        public CharactersRepository(IMongoDatabase mongoDatabase)
        {
            _charactersCollection = mongoDatabase.GetCollection<Characters>("CharactersCollection");
        }

        public async Task<Characters> GetCharacterById(string characterId)
        {
            var filter = Builders<Characters>.Filter.Eq(filed => filed.CharacterId, characterId);
            var result = await _charactersCollection.FindAsync(filter);

            return await result.SingleAsync();
        }
    }
}