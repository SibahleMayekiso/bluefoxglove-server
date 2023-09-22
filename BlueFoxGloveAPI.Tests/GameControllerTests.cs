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
        private IGameSessionRepository _gameSessionRepository;

        [SetUp]
        public void Setup()
        {
            _gameSessionRepository = Substitute.For<IGameSessionRepository>();
            _gameController = new GameController(_gameSessionRepository);
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
                    new Player
                    {
                        Credentials = new PlayerCredentials
                        {
                            PlayerId = "playerId",
                            PlayerName = "John Doe"
                        }
                    }
                }
            };
            _gameSessionRepository
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
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player1"
                    }
                },
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                         PlayerId = "player2"
                    }
                }
            };
            var newGame = new GameSession
            {
                GameSessionId = "newGameSessionId",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = expected
            };
            _gameSessionRepository
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