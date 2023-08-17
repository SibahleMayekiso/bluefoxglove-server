using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetPlayersByIdAsync(string PlayerId);
        Task CreateNewPlayerAsync(Player newPlayer);
        Task UpdatePlayerAsync(Player playerToUpdate);
        Task DeletePlayerAsync(string PlayerId);
    }
}
