using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface ICharacterRepository
    {
        Task<Characters> GetCharacterById(string characterId);
    }
}