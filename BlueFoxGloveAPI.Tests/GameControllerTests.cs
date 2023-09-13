using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class GameControllerTest
    {
        private GameController _gameController;
        private IGameRepository _gameRepository;

        [SetUp]
        public void Setup()
        {
            _gameRepository = Substitute.For<IGameRepository>();
            _gameController = new GameController(_gameRepository);
        }

        [Test]
        public async Task CreateGameSession_CreateNewGameSession_ReturnsGameSession()
        {
            //Arrange
            var expected = 201;
            var newGame = new GameSession
            {
                GameSessionId = "newGameSessionId",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = new List<Player>
                {
                    new Player {PlayerId = "playerId", PlayerName = "Jane Doe"}
                }
            };
            _gameRepository
            .CreateNewGameSessionAsync(Arg.Any<GameSession>())
            .Returns(Task.FromResult(newGame));

            //Act 
            var result = await _gameController.CreateGameSession(newGame) as CreatedAtActionResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateNewGameSession_WhenPassingNullValue_ReturnsBadRequest()
        {
            // Arrange
            var expected = 400;
            var newGame = new GameSession
            {
                GameSessionId = "",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = new List<Player>()
            };

            // Act 
            var result = await _gameController.CreateGameSession(newGame) as ObjectResult;
            var actual = result?.StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CreateGameSession_GameSessionCreatedWithAListOfPlayers_ReturnsListOfPlayers()
        {
            // Arrange
            var expected = new List<Player>
            {
                new Player { PlayerId = "player1" },
                new Player { PlayerId = "player2" }
            };
            var newGame = new GameSession
            {
                GameSessionId = "newGameSessionId",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = expected
            };
            _gameRepository
            .CreateNewGameSessionAsync(Arg.Any<GameSession>())
            .Returns(Task.FromResult(newGame));

            //Act 
            var result = await _gameController.CreateGameSession(newGame) as ObjectResult;
            var actual = (result?.Value as GameSession)?.PlayersJoiningSession;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}