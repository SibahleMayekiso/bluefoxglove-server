using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class CharacterRepository: ICharacterRepository
    {
        private readonly IMongoCollection<Characters> _charactersCollection;
        public CharacterRepository(IMongoDatabase mongoDatabase)
        {

        }
        public async Task<List<Characters>> GetAll()
        {
            return null;
        }
    }
}