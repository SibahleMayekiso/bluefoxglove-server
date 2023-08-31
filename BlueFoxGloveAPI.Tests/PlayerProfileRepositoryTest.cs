using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class PlayerProfileRepositoryTest
    {
        private IPlayerProfileRepository _playerProfileRepository;
        private List<PlayerProfile> _playerProfiles;

        [SetUp]
        public void Setup()
        {
            _playerProfileRepository = Substitute.For<IPlayerProfileRepository>();
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
        public void GetPlayerProfileById_WhenCalledWithAValidPlayerId_ReturnPlayerProfile()
        {
            //Arrange
            var playerProfile = _playerProfiles[0];
            var playerId = playerProfile.PlayerId ?? "";
            var expected = new List<PlayerProfile> {
                playerProfile
            };

            _playerProfileRepository.GetPlayerProfileById(playerId).Returns(expected);

            //Action
            var actual = _playerProfileRepository.GetPlayerProfileById(playerId).Result;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPlayerProfileById_WhenCalledWithNonExistantPlayerId_ReturnEmptyList()
        {
            //Arrange
            var playerId = "64dd1cf27a6922a9502fc8be";
            var expected = new List<PlayerProfile>();

            _playerProfileRepository.GetPlayerProfileById(playerId).Returns(new List<PlayerProfile>());

            //Action
            var actual = _playerProfileRepository.GetPlayerProfileById(playerId).Result;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
