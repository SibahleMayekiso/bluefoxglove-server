using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerProfileRepository
    {
        Task<List<PlayerProfile>> GetPlayerProfileById(string playerId);
    }
}
