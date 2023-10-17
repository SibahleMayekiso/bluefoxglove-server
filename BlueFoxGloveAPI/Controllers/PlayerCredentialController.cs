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
            var result = await _playerCredentialsRepository.GetPlayersCredentialsById(playerId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
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

        [HttpPut("{playerId}")]
        public async Task<IActionResult> UpdatePlayerName(string playerId, [FromBody] string newName)
        {
            if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(newName))
            {
                return BadRequest("Invalid request data.");
            }

            var existingPlayer = await _playerCredentialsRepository.GetPlayersCredentialsById(playerId);
            if (existingPlayer == null)
            {
                return NotFound("Player not found.");
            }

            existingPlayer.PlayerName = newName;

            await _playerCredentialsRepository.UpdatePlayerName(playerId, newName);

            return NoContent();
        }
    }
}