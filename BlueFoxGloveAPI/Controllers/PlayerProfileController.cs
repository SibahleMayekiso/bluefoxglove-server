using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerProfileController : ControllerBase
    {
        private readonly IPlayerProfileRepository _playerProfileRepository;

        public PlayerProfileController(IPlayerProfileRepository playerProfileRepository)
        {
            _playerProfileRepository = playerProfileRepository;
        }

        [HttpGet("/playerprofile/{playerId}")]
        public async Task<IActionResult> GetPlayerProfileById(string playerId)
        {
            var playerProfile = await _playerProfileRepository.GetPlayerProfileById(playerId);

            if (playerProfile.Count == 0)
            {
                return NotFound(playerProfile);
            }

            return Ok(playerProfile);
        }
    }
}
