using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class GameControllerTest
    {
        private GameController _gameController;
        private IGameSessionRepository _gameSessionRepository;
        private IGameSessionService _gameSessionService;

        [SetUp]
        public void Setup()
        {
            _gameSessionRepository = Substitute.For<IGameSessionRepository>();
            _gameSessionService = Substitute.For<IGameSessionService>();
            _gameController = new GameController(_gameSessionRepository, _gameSessionService);
        }

        [Test]
        public async Task CreateGameSession_CreateNewGameSession_ReturnsGameSession()
        {
            //Arrange
            var expected = 201;
            var newGame = new GameSession
            {
                GameSessionId = "newGameSessionId",
                GameName = "1",
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
            .CreateNewGameSession(Arg.Any<GameSession>())
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
                GameName = "",
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
                GameName = "1",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = expected
            };
            _gameSessionRepository
            .CreateNewGameSession(Arg.Any<GameSession>())
            .Returns(Task.FromResult(newGame));

            //Act 
            var result = await _gameController.CreateGameSession(newGame) as ObjectResult;
            var actual = (result?.Value as GameSession)?.PlayersJoiningSession;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSurvivingPlayers_WhenSurvivngPlayersCollectionIsNotEmpty_ReturnSurvivingPlayers()
        {
            //Arrange
            var survingPlayers = new List<Player>
            {
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player1",
                        PlayerName = "John Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 20,
                    PlayerHealth = 80,
                    PlayerXCoordinate = 100,
                    PlayerYCoordinate = 100
                },
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player3",
                        PlayerName = "Jane Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 10,
                    PlayerHealth = 50,
                    PlayerXCoordinate = 100,
                    PlayerYCoordinate = 500
                },
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player4",
                        PlayerName = "Joe Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 10,
                    PlayerHealth = 55,
                    PlayerXCoordinate = 500,
                    PlayerYCoordinate = 100
                }
            };
            _gameSessionService.SurvivngPlayers.Returns(survingPlayers);

            var expected = survingPlayers;

            //Act
            var result = _gameController.GetSurvivingPlayersInGameSession() as ObjectResult;
            var actual = result?.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSurvivingPlayers_WhenSurvivngPlayersCollectionIsNotEmpty_ReturnOkStatusCode()
        {
            //Arrange
            var survingPlayers = new List<Player>
            {
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player1",
                        PlayerName = "John Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 20,
                    PlayerHealth = 80,
                    PlayerXCoordinate = 100,
                    PlayerYCoordinate = 100
                },
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player3",
                        PlayerName = "Jane Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 10,
                    PlayerHealth = 50,
                    PlayerXCoordinate = 100,
                    PlayerYCoordinate = 500
                },
                new Player
                {
                    Credentials = new PlayerCredentials
                    {
                        PlayerId = "player4",
                        PlayerName = "Joe Doe"
                    },
                    PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayerScore = 10,
                    PlayerHealth = 55,
                    PlayerXCoordinate = 500,
                    PlayerYCoordinate = 100
                }
            };
            _gameSessionService.SurvivngPlayers.Returns(survingPlayers);

            var expected = 200;

            //Act
            var result = _gameController.GetSurvivingPlayersInGameSession() as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSurvivingPlayers_WhenSurvivngPlayersCollectionIsEmpty_ReturnBadRequestCode()
        {
            //Arrange
            _gameSessionService.SurvivngPlayers.ReturnsNull();

            var expected = 400;

            //Act
            var result = _gameController.GetSurvivingPlayersInGameSession() as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}