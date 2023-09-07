using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
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
            _playerProfiles = new List<PlayerProfile> {
                new PlayerProfile {
                    PlayerId = "64dd1cf27a6922a9502fc8be",
                    PlayerName = "JohnDoe001",
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
                .Returns(Task.FromResult(_playerProfiles.Where(player => player.PlayerId == playerId).ToList()));

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
                .Returns(Task.FromResult(_playerProfiles.Where(player => player.PlayerId == playerId).ToList()));

            //Action
            var result = await _playerProfileController.GetPlayerProfileById(playerId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}