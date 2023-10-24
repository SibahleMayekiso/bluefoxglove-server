using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class CharacterRepositoryTests
    {
        private IMongoCollection<Characters> _charactersCollection;
        private IMongoDatabase _mongoDatabase;
        private IAsyncCursor<Characters> _cursor;
        private CharactersRepository _charactersRepository;
        private List<Characters> _characters;

        [SetUp]
        public void Setup()
        {
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _charactersCollection = Substitute.For<IMongoCollection<Characters>>();
            _mongoDatabase.GetCollection<Characters>("CharactersCollection").Returns(_charactersCollection);
            _cursor = Substitute.For<IAsyncCursor<Characters>>();
            _charactersRepository = new CharactersRepository(_mongoDatabase);

            _characters = new List<Characters>
            {
                new Characters
                {
                    CharacterId = "64d11cf27a6922a9505fc8be",
                    CharacterName = "Guppy",
                    CharacterType = "Soldier",
                    CharacterMaxHealth = 100,
                    CharacterMaxSpeed = 5.00
                }
            };
        }

        [Test]
        public async Task GetCharacterById_WithValidCharacterId_ReturnCharacter()
        {
            //Arrange
            var characterId = _characters[0].CharacterId;
            var expected = _characters[0];

            _cursor.Current.Returns(new List<Characters> { expected });
            _cursor.MoveNextAsync().Returns(Task.FromResult(true));

            _charactersCollection
                .FindAsync<Characters>(Builders<Characters>.Filter.Eq(filed => filed.CharacterId, characterId))
                .ReturnsForAnyArgs(Task.FromResult(_cursor));

            //Act
            var actual = await _charactersRepository.GetCharacterById(characterId);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}