using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class PlayerController: ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpPost("[controller]/playerprofile")]
        public async Task<IActionResult> CreatePlayerProfile(Player player)
        {
            return null;
        }

        [HttpGet("[controller]/playerprofile")]
        public async Task<IActionResult> GetAllPlayerProfiles()
        {
            return null;
        }

        [HttpGet("[controller]/playerprofile/{playerID}")]
        public async Task<IActionResult> GetPlayerProfileById(string playerId)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPut("[controller]/playerprofile")]
        public async Task<IActionResult> UpdatePlayerProfile(Player player)
        {
            return null;
        }

        [HttpDelete("[controller]/playerprofile/{playerId}")]
        public async Task<IActionResult> DeletePlayerProfileById(string playerId)
        {
            return null;
        }
    }
}