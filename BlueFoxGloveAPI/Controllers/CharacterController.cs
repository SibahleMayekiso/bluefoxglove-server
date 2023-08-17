using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class CharacterController: ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        [HttpGet("[controller]/{characterId}")]
        public async Task<IActionResult> GetCharacter(string characterId)
        {
            return null;
        }
    }
}
