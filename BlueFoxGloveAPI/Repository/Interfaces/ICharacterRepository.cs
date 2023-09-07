using BlueFoxGloveAPI.Models;

namespace BlueFoxGloveAPI.Repository.Interfaces
{
    public interface ICharacterRepository
    {
        Task<List<Characters>> GetAllAsync();
    }
}