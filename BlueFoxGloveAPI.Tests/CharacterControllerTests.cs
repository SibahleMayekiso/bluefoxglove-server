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
    public class CharacterControllerTests
    {
        private ICharacterRepository _characterRepository;
        private CharacterController _characterController;
        private List<Characters> _characters;

        [SetUp]
        public void Setup()
        {
            _characterRepository = Substitute.For<ICharacterRepository>();
            _characterController = new CharacterController(_characterRepository);

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
        public async Task GetCharacter_WhenCharacterIdIsValid_ReturnOkResultStatusCode()
        {
            //Arrange
            var characterId = _characters[0].CharacterId;
            var expected = 200;

            _characterRepository.GetCharacterById(characterId).Returns(_characters[0]);

            //Act
            var result = await _characterController.GetCharacter(characterId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetCharacter_WhenCharacterIdIsValid_ReturnCharacter()
        {
            //Arrange
            var characterId = _characters[0].CharacterId;
            var expected = _characters[0];

            _characterRepository.GetCharacterById(characterId).Returns(_characters[0]);

            //Act
            var result = await _characterController.GetCharacter(characterId) as ObjectResult;
            var actual = result?.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("123")]
        [TestCase(null)]
        public async Task GetCharacter_WhenCharacterIdIsInvalid_ReturnBadRequestStatusCode(string invalidCharacterId)
        {
            //Arrange
            var characterId = invalidCharacterId;
            var expected = 400;

            _characterRepository.GetCharacterById(characterId).ReturnsNull();

            //Act
            var result = await _characterController.GetCharacter(characterId) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}