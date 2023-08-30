using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(string playerId);
        Task CreateNewPlayerAsync(Player newPlayer);
        Task UpdatePlayerAsync(Player playerToUpdate);
        Task DeletePlayerAsync(string playerId);
    }
}
