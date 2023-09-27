using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerCredentialController: ControllerBase
    {
        private readonly IPlayerCredentialsRepository _playerCredentialsRepository;

        public PlayerCredentialController(IPlayerCredentialsRepository playerCredentialsRepository)
        {
            _playerCredentialsRepository = playerCredentialsRepository;
        }

        [HttpGet("/credentials/{playerId}")]
        public async Task<IActionResult> GetPlayersCredentialsById(string playerId)
        {
            return null;
        }

        [HttpPost("/credentials")]
        public async Task<IActionResult> CreatePlayer(PlayerCredentials newPlayer)
        {
            if (string.IsNullOrWhiteSpace(newPlayer.PlayerName))
            {
                return BadRequest("Invalid or incomplete game session data");
            }

            await _playerCredentialsRepository.CreatePlayer(newPlayer);

            return CreatedAtAction(nameof(GetPlayersCredentialsById), new { playerId = newPlayer.PlayerId }, newPlayer);
        }
    }
}