using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class PlayerProfileRepositoryTest
    {
        private IMongoCollection<PlayerProfile> _playerProfileCollection;
        private IMongoCollection<Characters> _characterCollection;
        private IMongoDatabase _mongoDatabase;
        private IAsyncCursor<Characters> _cursor;
        private IPlayerProfileRepository _playerProfileRepository;
        private PlayerProfileRepository _profileRepository;
        private List<PlayerProfile> _playerProfiles;

        [SetUp]
        public void Setup()
        {
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _playerProfileCollection = Substitute.For<IMongoCollection<PlayerProfile>>();
            _characterCollection = Substitute.For<IMongoCollection<Characters>>();
            _mongoDatabase.GetCollection<PlayerProfile>("PlayerProfileCollection").Returns(_playerProfileCollection);
            _mongoDatabase.GetCollection<Characters>("CharactersCollection").Returns(_characterCollection);
            _cursor = Substitute.For<IAsyncCursor<Characters>>();
            _profileRepository = new PlayerProfileRepository(_mongoDatabase);

            _playerProfileRepository = Substitute.For<IPlayerProfileRepository>();
            _playerProfiles = new List<PlayerProfile>
            {
                new PlayerProfile
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "64dd1cf27a6922a9502fc8be",
                        PlayerName = "John Doe"
                    },
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
            var playerId = playerProfile.Credentials.PlayerId ?? "";
            var expected = new List<PlayerProfile>
            {
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
            var playerId = "64dd1cf27a6922a9502fc8b0";
            var expected = new List<PlayerProfile>();

            _playerProfileRepository.GetPlayerProfileById(playerId).Returns(new List<PlayerProfile>());

            //Action
            var actual = _playerProfileRepository.GetPlayerProfileById(playerId).Result;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateSelectedCharacter_WithValidCharacterId_ReturnPlayerProfileWithUpdatedSelectedCharacter()
        {
            //Arrange
            var playerId = "64da1cf27a6922a9502fc8b4";
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

            var expected = updatedPlayer.SelectedCharacter;

            _cursor.Current.Returns(new List<Characters> { expected });
            _cursor.MoveNextAsync().Returns(Task.FromResult(true));

            _characterCollection
                .FindAsync<Characters>(Arg.Any<FilterDefinition<Characters>>())
                .Returns(Task.FromResult(_cursor));

            _playerProfileCollection
                .FindOneAndUpdateAsync<PlayerProfile>(Arg.Any<FilterDefinition<PlayerProfile>>(), Arg.Any<UpdateDefinition<PlayerProfile>>())
                .Returns(Task.FromResult(updatedPlayer));

            //Act
            var result = await _profileRepository.UpdateSelectedCharacter(playerId, characterId);
            var actual = result.SelectedCharacter;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreatePlayerProfile_CreatePlayerProfile_PlayerProfileCollectionContainsNewProfile()
        {
            //Arrange
            var playerId = "653fafe25893539767c6360f";
            var playerProfile = new PlayerProfile
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

            _playerProfileCollection
               .When(collection => collection.InsertOneAsync(playerProfile))
               .Do(callback => _playerProfiles.Add(playerProfile));

            //Act 
            await _profileRepository.CreatePlayerProfile(playerProfile);

            //Assert
            Assert.Contains(playerProfile, _playerProfiles);
        }
    }
}