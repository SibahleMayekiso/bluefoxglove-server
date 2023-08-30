using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class PlayerControllerTests
    {
        private PlayerController _playerController;
        private IPlayerRepository _playerRepository;

        [SetUp]
        public void Setup()
        {
            _playerRepository = Substitute.For<IPlayerRepository>();
            _playerController = new PlayerController(_playerRepository);
        }
        [Test]
        public async Task GetPlayerProfileById_WhenCalledWithAValidPlayerIdAndThePlayerExists_ReturnsThePlayer()
        {
            // Arrange
            string playerId = "playerId";
            var expectedPlayer = new Player { PlayerId = playerId, PlayerName = "Jane Doe" };

            _playerRepository.GetPlayerByIdAsync(playerId).Returns(expectedPlayer);

            // Act
            var result = await _playerController.GetPlayerProfileById(playerId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedPlayer, okResult?.Value);
        }
        [Test]
        public async Task GetPlayerProfileById_WhenCalledWithAInvalidPlayerIdAndThePlayerDoesNotExist_ReturnsPlayerNotFound()
        {
            // Arrange
            string playerId = "invalidPlayerId";
            _playerRepository.GetPlayerByIdAsync(playerId).ReturnsNull();

            // Act
            IActionResult result = await _playerController.GetPlayerProfileById(playerId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);

        }
    }
}
