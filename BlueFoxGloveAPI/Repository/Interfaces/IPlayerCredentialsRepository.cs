using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface IPlayerCredentialsRepository
    {
        Task CreatePlayer(PlayerCredentials newPlayer);
        Task<PlayerCredentials> GetPlayersCredentialsById(string playerId);
        Task<PlayerCredentials> UpdatePlayerName(string playerId, string newName);
        Task DeletePlayerCredentials(string playerId);
    }
}