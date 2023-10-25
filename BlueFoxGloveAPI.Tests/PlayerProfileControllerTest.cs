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
    public class PlayerProfileControllerTest
    {
        private PlayerProfileController _playerProfileController;
        private IPlayerProfileRepository _playerProfileRepository;
        private List<PlayerProfile> _playerProfiles;

        [SetUp]
        public void Setup()
        {
            _playerProfileRepository = Substitute.For<IPlayerProfileRepository>();
            _playerProfileController = new PlayerProfileController(_playerProfileRepository);
            _playerProfiles = new List<PlayerProfile>
            {
                new PlayerProfile
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "64dd1cf27a6922a9502fc8be",
                        PlayerName = "John Doe"
                    },
                    SelectedCharacter = new Characters
                    {
                        CharacterId = "64d11cf27a6922a9505fc8be",
                        CharacterName = "Guppy",
                        CharacterType = "Soldier",
                        CharacterMaxHealth = 100,
                        CharacterMaxSpeed = 5.00
                    },
                    LongestSurvivalTime = 0,
                    TotalPlayTime = 0,
                    NumberOfGamesPlayed = 0,
                    KillCount = 0
                }
            };
        }

        [Test]
        public async Task GetPlayerProfileById_WhenCalledWithAValidPlayerId_ReturnOkStatusCode()
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8be";
            var expected = 200;

            _playerProfileRepository
                .GetPlayerProfileById(playerId)
                .Returns(Task.FromResult(_playerProfiles.Where(player => player.Credentials.PlayerId == playerId).ToList()));

            //Action
            var result = await _playerProfileController.GetPlayerProfileById(playerId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetPlayerProfileById_WhenCalledWithANonExistantPlayerId_ReturnNotFoundStatusCode()
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8b0";
            var expected = 404;

            _playerProfileRepository
                .GetPlayerProfileById(playerId)
                .Returns(Task.FromResult(_playerProfiles.Where(player => player.Credentials.PlayerId == playerId).ToList()));

            //Action
            var result = await _playerProfileController.GetPlayerProfileById(playerId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateSelectedCharacter_WithAValidCharacterId_ReturnOkStatusCode()
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8be";
            var characterId = "64d11cf27a6922a9505fc8be";
            var updatedPlayer = new PlayerProfile
            {
                Credentials = new PlayerCredentials { PlayerId = playerId, PlayerName = "Player1" },
                SelectedCharacter = new Characters
                {
                    CharacterId = "64d11cf27a6922a9505fc8be",
                    CharacterName = "Guppy",
                    CharacterType = "Soldier",
                    CharacterMaxHealth = 100,
                    CharacterMaxSpeed = 5.00
                },
                KillCount = 0,
                LongestSurvivalTime = 0,
                NumberOfGamesPlayed = 0,
                TotalPlayTime = 0
            };

            var expected = 200;

            _playerProfileRepository.UpdateSelectedCharacter(playerId, characterId).Returns(updatedPlayer);

            //Action
            var result = await _playerProfileController.UpdateSelectedCharacter(playerId, characterId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateSelectedCharacter_WithAValidCharacterId_ReturnsPlayerWithSelectedCharacter()
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8be";
            var characterId = "64d11cf27a6922a9505fc8be";
            var updatedPlayer = new PlayerProfile
            {
                Credentials = new PlayerCredentials { PlayerId = playerId, PlayerName = "Player1" },
                SelectedCharacter = new Characters
                {
                    CharacterId = "64d11cf27a6922a9505fc8be",
                    CharacterName = "Guppy",
                    CharacterType = "Soldier",
                    CharacterMaxHealth = 100,
                    CharacterMaxSpeed = 5.00
                },
                KillCount = 0,
                LongestSurvivalTime = 0,
                NumberOfGamesPlayed = 0,
                TotalPlayTime = 0
            };

            var expected = updatedPlayer;

            _playerProfileRepository
                .UpdateSelectedCharacter(playerId, characterId)
                .Returns(updatedPlayer);

            //Action
            var result = await _playerProfileController.UpdateSelectedCharacter(playerId, characterId) as ObjectResult;
            var actual = result?.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("agjdagavnaidngu")]
        [TestCase("!@#$%")]
        [TestCase(null)]
        public async Task UpdateSelectedCharacter_WithInvalidValidCharacterId_ReturnBadRequestStatusCode(string characterId)
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8be";
            var expected = 400;

            _playerProfileRepository
                .UpdateSelectedCharacter(playerId, characterId)
                .ReturnsNull();

            //Action
            var result = await _playerProfileController.UpdateSelectedCharacter(playerId, characterId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}