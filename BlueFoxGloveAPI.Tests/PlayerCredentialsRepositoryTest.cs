using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFoxGloveAPI.Tests
{
    public class PlayerCredentialsRepositoryTest
    {
        private IMongoCollection<PlayerCredentials> _playersCredentailsCollection;
        private IMongoDatabase _mongoDatabase;
        private IAsyncCursor<PlayerCredentials> _cursor;
        private PlayerCredentialsRepository _playerCredentialsRepository;
        private List<PlayerCredentials> _playerCredentials;

        [SetUp]
        public void Setup()
        {
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _playersCredentailsCollection = Substitute.For<IMongoCollection<PlayerCredentials>>();
            _mongoDatabase.GetCollection<PlayerCredentials>("PlayerCredentialsCollection").Returns(_playersCredentailsCollection);
            _cursor = Substitute.For<IAsyncCursor<PlayerCredentials>>();
            _playerCredentialsRepository = new PlayerCredentialsRepository(_mongoDatabase);
            _playerCredentials = new List<PlayerCredentials>
            {
                new PlayerCredentials
                {
                     PlayerId = "64dd1cf27a6922a9502fc90a",
                     PlayerName = "Jane Doe"
                },
                new PlayerCredentials
                {
                      PlayerId = "64dd1cf27a6922a9502fc73b",
                     PlayerName = "john Doe"
                }
            };
        }

        [Test]
        public async Task CreatePlayer_WhenValidPlayerCredentialsAreAdded_PlayersCredentialsCollectionContainsNewPlayer()
        {
            //Arrange
            var newPlayerCredential = new PlayerCredentials
            {

                PlayerId = "64dd1cf27a6922a9502fc8be",
                PlayerName = "Jay Doe"

            };
            _playersCredentailsCollection
               .When(collection => collection.InsertOneAsync(Arg.Any<PlayerCredentials>()))
               .Do(callback => _playerCredentials.Add(newPlayerCredential));

            //Act 
            await _playerCredentialsRepository.CreatePlayer(newPlayerCredential);

            //Assert
            Assert.Contains(newPlayerCredential, _playerCredentials);
        }

        [Test]
        public async Task GetPlayersCredentialsById_WhenPlayerWithAnExistingId_ReturnPlayer()
        {
            //Arrange
            string playerCredentialId = _playerCredentials[0].PlayerId;
            var expected = _playerCredentials[0];

            _cursor.Current.Returns(new List<PlayerCredentials> { expected });
            _cursor.MoveNextAsync().Returns(Task.FromResult(true));

            _playersCredentailsCollection
                .FindAsync<PlayerCredentials>(Arg.Any<FilterDefinition<PlayerCredentials>>())
                .Returns(Task.FromResult(_cursor));

            //Act
            var actual = await _playerCredentialsRepository.GetPlayersCredentialsById(playerCredentialId);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}