using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services;
using BlueFoxGloveAPI.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

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
            Assert.ThrowsAsync<PlayerNotFoundException>(() => _gameSessionService.UpdatePlayerPostion(gameSessonId, playerId, playerMovement));
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
            Assert.ThrowsAsync<PlayerNotFoundException>(() => _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId));
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

        [Test]
        public async Task JoinGameSession_WhenPlayerJoins_ReturnPlayerXCoordinateInBounds()
        {
            //Arrange
            var gameSessionId = _gameSessions[0].GameSessionId;
            var playerId = _gameSessions[0].PlayersJoiningSession[0].Credentials.PlayerId;
            var expected = 1280;
            var player = new Player { Credentials = new PlayerCredentials { PlayerId = playerId, PlayerName = "JjDoe" } };
            var updatedSession = new GameSession
            {
                GameSessionId = _gameSessions[0].GameSessionId,
                PlayersJoiningSession = new List<Player> { player }
            };

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[0]);
            _gameRepository.UpdateGameSession(_gameSessions[0], Arg.Is<Player>(predicate => predicate.Credentials.PlayerId == playerId)).Returns(updatedSession);

            //Act
            var result = await _gameSessionService.JoinGameSession(gameSessionId, playerId);
            var actual = result.PlayersJoiningSession[0].PlayerXCoordinate;

            //Assert
            Assert.LessOrEqual(actual, expected);
        }

        [Test]
        public async Task JoinGameSession_WhenPlayerJoins_ReturnPlayerYCoordinateInBounds()
        {
            //Arrange
            var gameSessionId = _gameSessions[0].GameSessionId;
            var playerId = _gameSessions[0].PlayersJoiningSession[0].Credentials.PlayerId;
            var expected = 720;
            var player = new Player { Credentials = new PlayerCredentials { PlayerId = playerId, PlayerName = "JjDoe" } };
            var updatedSession = new GameSession
            {
                GameSessionId = _gameSessions[0].GameSessionId,
                PlayersJoiningSession = new List<Player> { player }
            };

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[0]);
            _gameRepository.UpdateGameSession(_gameSessions[0], Arg.Is<Player>(predicate => predicate.Credentials.PlayerId == playerId)).Returns(updatedSession);

            //Act
            var result = await _gameSessionService.JoinGameSession(gameSessionId, playerId);
            var actual = result.PlayersJoiningSession[0].PlayerYCoordinate;

            //Assert
            Assert.LessOrEqual(actual, expected);
        }

        [Test]
        public async Task AddScoreBoardInGameSession_WhenPlayerGains5Points_ReturnScoreDifferenceOf5()
        {
            //Arrange
            var gameSessionId = _gameSessions[0].GameSessionId;
            var playerId = _gameSessions[0].PlayersJoiningSession[0].Credentials.PlayerId;
            var scoreBeforeUpdate = _gameSessions[0].PlayersJoiningSession[0].PlayerScore;
            var expected = 5;

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSessions[0]);
            _gameRepository.UpdateGameSession(_gameSessions[0], Arg.Is<Player>(predicate => predicate.Credentials.PlayerId == playerId)).Returns(_gameSessions[0]);

            //Act
            var result = await _gameSessionService.AddScoreBoardInGameSession(gameSessionId, playerId);
            var actual = Math.Abs(scoreBeforeUpdate - result.PlayersJoiningSession[0].PlayerScore);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateProjectile_WhenProjectileCreatedWithinBounds_ReturnPositionInBounds()
        {
            //Arrange
            var playerId = "651b168a474e72e0a0cbc835";
            var currentPosition = new Vector { X = 100, Y = 100 };
            var velocity = new Vector { X = 1, Y = 1 };

            var expected = currentPosition;

            //Act
            var actual = _gameSessionService.CreateProjectile(playerId, currentPosition, velocity);

            //Assert
            Assert.AreEqual(expected, actual.Position);
        }

        private static IEnumerable<Vector> ProjectileOutOfBoundsTestCases()
        {
            yield return new Vector { X = -100, Y = -100 };
            yield return new Vector { X = 0, Y = -100 };
            yield return new Vector { X = -100, Y = 0 };
            yield return new Vector { X = 1000000000, Y = -100 };
            yield return new Vector { X = 1280, Y = -1000000000 };
            yield return new Vector { X = 1281, Y = 721 };
        }

        [TestCaseSource(nameof(ProjectileOutOfBoundsTestCases))]
        public void CreateProjectile_WhenProjectileCreatedOutOfBounds_ThrowProjectileCreationException(Vector position)
        {
            //Arrange
            var playerId = "651b168a474e72e0a0cbc835";
            var currentPosition = position;
            var velocity = new Vector { X = 1, Y = 1 };

            //Assert
            Assert.Throws<ProjectileCreationException>(() => _gameSessionService.CreateProjectile(playerId, currentPosition, velocity));
        }

        [Test]
        public void FireProjectile_WhenPlayerFires_AddProjectileToCollection()
        {
            //Arrange
            var playerId = "651b168a474e72e0a0cbc835";
            var currentPosition = new Vector { X = 100, Y = 100 };
            var velocity = new Vector { X = 1, Y = 1 };

            var expectedProjectile = new Projectile { ProjectileId = 1, PlayerId = playerId, Position = currentPosition, Speed = 5, Velocity = velocity };
            var expected = expectedProjectile.ProjectileId;

            //Act
            _gameSessionService?.FireProjectile(playerId, currentPosition, velocity);
            var actual = _gameSessionService?.ProjectilesInPlay.Values.LastOrDefault()?.ProjectileId;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DisposeProjectile_WhenProjectileIsInactive_RemoveProjectileToCollection()
        {
            //Arrange
            _gameSessionService.ProjectilesInPlay = new ConcurrentDictionary<int, Projectile>(new Dictionary<int, Projectile>
            {
                { 1, new Projectile { ProjectileId = 1, PlayerId = "player1", Position = new Vector { X = 100, Y = 100 }, Speed = 5, Velocity = new Vector { X = 1, Y = 1 } }}
            });

            //Act
            _gameSessionService.DisposeProjectile(1);
            var actual = _gameSessionService.ProjectilesInPlay.Keys;

            //Assert
            CollectionAssert.DoesNotContain(actual, 1);
        }

        [Test]
        public void DisposeProjectile_WhenNonExistantProjectileIdIsUsed_ThrowInvalidOperationException()
        {
            //Arrange
            _gameSessionService.ProjectilesInPlay = new ConcurrentDictionary<int, Projectile>(new Dictionary<int, Projectile>
            {
                {
                    1,
                    new Projectile
                    {
                        ProjectileId = 1,
                        PlayerId = "player1",
                        Position = new Vector { X = 100, Y = 100 },
                        Speed = 5,
                        Velocity = new Vector { X = 1, Y = 1 }
                    }
                }
            });

            var invalidProjecileId = 2;

            //Assert
            Assert.Throws<InvalidOperationException>(() => _gameSessionService.DisposeProjectile(invalidProjecileId));
        }
    }
}