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
    public class PlayerCredentialsControllerTest
    {
        private PlayerCredentialController _playerCredentialController;
        private IPlayerCredentialsRepository _playerCredentialsRepository;

        [SetUp]
        public void Setup()
        {
            _playerCredentialsRepository = Substitute.For<IPlayerCredentialsRepository>();
            _playerCredentialController = new PlayerCredentialController(_playerCredentialsRepository);
        }

        [Test]
        public async Task CreatePlayer_WhenANewPlayerIsCreated_ReturnsSuccessStatusCode()
        {
            //Arrange
            var expected = 201;
            var newPlayerCredentials = new PlayerCredentials
            {
                PlayerId = "64dd1cf27a6922a9502fc90a",
                PlayerName = "Jane Doe"
            };

            _playerCredentialsRepository
           .CreatePlayer(Arg.Any<PlayerCredentials>())
           .Returns(Task.FromResult(newPlayerCredentials));

            //Act 
            var result = await _playerCredentialController.CreatePlayer(newPlayerCredentials) as CreatedAtActionResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreatePlayer_WhenPlayerWithAnEmptyNameIsCreated_ReturnsABadRequestError()
        {
            //Arrange
            var expected = 400;
            var newPlayerCredentials = new PlayerCredentials
            {
                PlayerId = "",
                PlayerName = ""
            };

            _playerCredentialsRepository
           .CreatePlayer(Arg.Any<PlayerCredentials>())
           .Returns(Task.FromResult(newPlayerCredentials));

            //Act 
            var result = await _playerCredentialController.CreatePlayer(newPlayerCredentials) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetPlayersCredentialsById_WhenPlayerIdIsValid_ReturnThePlayer()
        {
            // Arrange
            string playerId = "64dd1cf27a6922a9502fc90a";
            var expectedPlayer = new PlayerCredentials
            {
                PlayerId = playerId,
                PlayerName = "Jane Doe"

            };

            _playerCredentialsRepository.GetPlayersCredentialsById(playerId).Returns(expectedPlayer);

            // Act
            var result = await _playerCredentialController.GetPlayersCredentialsById(playerId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedPlayer, okResult?.Value);
        }

        [Test]
        public async Task GetPlayersCredentialsById_WhenAnInvalidPlayerIdIsCalled_ReturnsPlayerNotFound()
        {
            // Arrange
            string playerId = "4dd1cf27a6922a9502fc8be";
            _playerCredentialsRepository.GetPlayersCredentialsById(playerId).ReturnsNull();

            // Act
            IActionResult result = await _playerCredentialController.GetPlayersCredentialsById(playerId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task UpdatePlayerName_ValidNameInput_ReturnsNoContentResult()
        {
            // Arrange
            string playerId = "64dd1cf27a6922a9502fc90a";
            string newName = "Jonny Doe";

            var existingPlayer = new PlayerCredentials
            {
                PlayerId = playerId,
                PlayerName = "Jane Doe"

            };
            _playerCredentialsRepository.GetPlayersCredentialsById(playerId).Returns(existingPlayer);

            //Act
            var result = await _playerCredentialController.UpdatePlayerName(playerId, newName);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}