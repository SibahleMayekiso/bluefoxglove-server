using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public class PlayerProfileRepository : IPlayerProfileRepository
    {
        private readonly IMongoCollection<PlayerProfile> _playerProfileCollection;

        public PlayerProfileRepository(IMongoDatabase mongoDatabase)
        {
            _playerProfileCollection = mongoDatabase.GetCollection<PlayerProfile>("playerProfileCollection");
        }

        public async Task<List<PlayerProfile>> GetPlayerProfileById(string playerId)
        {
            var result = await _playerProfileCollection.Find(playerProfile => playerProfile.PlayerId == playerId).ToListAsync();

            return result;
        }
    }
}
