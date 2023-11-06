using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerProfileRepository
    {
        Task<List<PlayerProfile>> GetPlayerProfileById(string playerId);
        Task<PlayerProfile> UpdateSelectedCharacter(string playerId, string characterId);
        Task CreatePlayerProfile(PlayerProfile playerProfile);
    }
}