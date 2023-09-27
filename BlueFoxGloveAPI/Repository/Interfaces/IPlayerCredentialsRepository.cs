using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerCredentialsRepository
    {
        Task CreatePlayer(PlayerCredentials newPlayer);
        Task<PlayerCredentials> GetPlayersCredentialsById(string playerId);
        Task UpdatePlayerCredentials(PlayerCredentials playerToUpdate);
        Task DeletePlayerCredentials(string playerId);
    }
}