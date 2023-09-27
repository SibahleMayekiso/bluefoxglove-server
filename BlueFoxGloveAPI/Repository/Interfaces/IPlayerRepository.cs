using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayers();
        Task<Player> GetPlayerById(string playerId);
        Task CreateNewPlayer(Player newPlayer);
        Task UpdatePlayer(Player playerToUpdate);
        Task DeletePlayer(string playerId);
    }
}