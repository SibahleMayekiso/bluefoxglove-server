using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services;
using BlueFoxGloveAPI.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    internal class GameSessionServiceTests
    {
        private ILobbyTimerWrapper _lobbyTimer;
        private IGameSessionRepository _gameRepository;
        private GameSessionService _gameSessionService;
        private List<GameSession> _gameSessions;

        [SetUp]
        public void SetUp()
        {
            _gameSessions = new List<GameSession>
            {
                new GameSession
                {
                    GameSessionId = "651b168a474e72e070cbc831",
                    GameName = "Game_0",
                    GameSessionTimeStamp = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayersJoiningSession = new List<Player>
                    {
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player1",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        }
                    }
                },
                new GameSession
                {
                    GameSessionId = "651b168a474e72e070cbc835",
                    GameName = "Game_0",
                    GameSessionTimeStamp = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                    PlayersJoiningSession = new List<Player>
                    {
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player1",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        },
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player2",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        },
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player3",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        },
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player4",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        },
                        new Player
                        {
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player5",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                            PlayerScore = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        }
                    }
                }
            };

            _lobbyTimer = Substitute.For<ILobbyTimerWrapper>();
            _gameRepository = Substitute.For<IGameSessionRepository>();
            _gameSessionService = new GameSessionService(_gameRepository, _lobbyTimer);
        }

        [TestCase(100, 99, PlayerMovement.MOVEUP)]
        [TestCase(100, 101, PlayerMovement.MOVEDOWN)]
        [TestCase(99, 100, PlayerMovement.MOVELEFT)]
        [TestCase(101, 100, PlayerMovement.MOVERIGHT)]
        public async Task UpdatePlayerPostion_WhenPlayerMoves_UpdateGameSessionAsyncCalledWithNewCoordinates(int xCoordinate, int yCoordinate, PlayerMovement playerMovement)
        {
            //Arrange
            var gameSessonId = _gameSessions[0].GameSessionId;
            var playerId = _gameSessions[0].PlayersJoiningSession[0].Credentials.PlayerId;

            var updatedGameSession = new GameSession
            {
                GameSessionId = "session 1",
                PlayersJoiningSession = new List<Player>
                {
                    new Player
                    {
                        Credentials = new PlayerCredentials
                        {
                            PlayerId = "player1",
                            PlayerName = "John Doe"
                        },
                        PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                        PlayerScore = 0,
                        PlayerHealth = 100,
                        PlayerXCoordinate = xCoordinate,
                        PlayerYCoordinate = yCoordinate
                    }
                }
            };

            _gameRepository.GetGameSessionById(gameSessonId).Returns(_gameSessions[0]);

            //Act
            await _gameSessionService.UpdatePlayerPostion(gameSessonId, playerId, playerMovement);

            //Assert
            await _gameRepository
                .Received(1)
                .UpdateGameSession(_gameSessions[0], Arg.Is<Player>(player =>
                player.PlayerXCoordinate == updatedGameSession.PlayersJoiningSession[0].PlayerXCoordinate &&
                player.PlayerYCoordinate == updatedGameSession.PlayersJoiningSession[0].PlayerYCoordinate));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("!@#$%")]
        public void UpdatePlayerPostion_WhenInvalidPlayerIdIsUsed_ThrowPlayerNotFoundExcpetion(string playerId)
        {
            //Arrange
            var gameSessonId = _gameSessions[0].GameSessionId;
            var playerMovement = PlayerMovement.MOVEDOWN;

            _gameRepository.GetGameSessionById(gameSessonId).Returns(_gameSessions[0]);

            //Act

            //Assert
            Assert.ThrowsAsync<PlayerNotFoundExcpetion>(() => _gameSessionService.UpdatePlayerPostion(gameSessonId, playerId, playerMovement));
        }

        [Test]
        public async Task UpdatePlayerHealth_WhenPlayerTakesDamage_UpdateGameSessionAsyncCalledWithNewPlayerHealth()
        {
            //Arrange
            var gameSessionId = _gameSessions[0].GameSessionId;
            var playerId = _gameSessions[0].PlayersJoiningSession[0].Credentials.PlayerId;

            var updatedGameSession = new GameSession
            {
                GameSessionId = gameSessionId,
                PlayersJoiningSession = new List<Player>
                {
                    new Player
                    {
                        Credentials = new PlayerCredentials
                        {
                            PlayerId = playerId,
                            PlayerName = "John Doe"
                        },
                        PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                        PlayerScore = 0,
                        PlayerHealth = 99,
                        PlayerXCoordinate = 100,
                        PlayerYCoordinate = 100
                    }
                }
            };

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[0]);

            //Act
            await _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId);

            //Assert
            await _gameRepository
                .Received(1)
                .UpdateGameSession(_gameSessions[0], Arg.Is<Player>(player => player.PlayerHealth == updatedGameSession.PlayersJoiningSession[0].PlayerHealth));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("!@#$%")]
        public void UpdatePlayerHealth_WhenInvalidPlayerIdIsUsed_ThrowPlayerNotFoundExcpetion(string playerId)
        {
            //Arrange
            var gameSessionId = _gameSessions[0].GameSessionId;

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[0]);

            //Act

            //Assert
            Assert.ThrowsAsync<PlayerNotFoundExcpetion>(() => _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId));
        }

        [Test]
        public async Task CheckGameLobby_WhenMinimumNumberOfPlayersReached_InvokeStartGameSession()
        {
            //Arrange
            var gameSessionId = _gameSessions[1].GameSessionId;

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[1]);

            //Act
            _gameSessionService.GameSessionId = gameSessionId;
            await _gameSessionService.CheckGameLobby();

            //Assert
            Received.InOrder(() => _gameSessionService.StartGameSession());
        }

        [Test]
        public async Task CheckGameLobby_WhenMinimumNumberOfPlayersNotReached_RestartLobbyTimer()
        {
            //Arrange
            _gameSessionService.GameSessionId = _gameSessions[0].GameSessionId;
            _gameRepository.GetGameSessionById(_gameSessionService.GameSessionId).Returns(_gameSessions[0]);

            //Act
            await _gameSessionService.CheckGameLobby();

            //Assert
            _lobbyTimer.Received().Change(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
        }
    }
}