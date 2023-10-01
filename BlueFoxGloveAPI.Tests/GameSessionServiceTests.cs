using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    internal class GameSessionServiceTests
    {
        private IGameSessionRepository _gameRepository;
        private GameSessionService _gameSessionService;
        private GameSession _gameSession;

        [SetUp]
        public void SetUp()
        {
            _gameRepository = Substitute.For<IGameSessionRepository>();
            _gameSessionService = new GameSessionService(_gameRepository);

            _gameSession = new GameSession
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
                        PlayerXCoordinate = 100,
                        PlayerYCoordinate = 100
                    }
                }
            };
        }

        [TestCase(100, 99, PlayerMovement.MOVEUP)]
        [TestCase(100, 101, PlayerMovement.MOVEDOWN)]
        [TestCase(99, 100, PlayerMovement.MOVELEFT)]
        [TestCase(101, 100, PlayerMovement.MOVERIGHT)]
        public async Task UpdatePlayerPostion_WhenPlayerMoves_UpdateGameSessionAsyncCalledWithNewCoordinates(int xCoordinate, int yCoordinate, PlayerMovement playerMovement)
        {
            //Arrange
            var gameSessonId = _gameSession.GameSessionId;
            var playerId = _gameSession.PlayersJoiningSession[0].Credentials.PlayerId;

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

            _gameRepository.GetGameSessionById(gameSessonId).Returns(_gameSession);

            //Act
            await _gameSessionService.UpdatePlayerPostion(gameSessonId, playerId, playerMovement);

            //Assert
            await _gameRepository
                .Received(1)
                .UpdateGameSession(_gameSession, Arg.Is<Player>(player =>
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
            var gameSessonId = _gameSession.GameSessionId;
            var playerMovement = PlayerMovement.MOVEDOWN;

            _gameRepository.GetGameSessionById(gameSessonId).Returns(_gameSession);

            //Act

            //Assert
            Assert.ThrowsAsync<PlayerNotFoundExcpetion>(() => _gameSessionService.UpdatePlayerPostion(gameSessonId, playerId, playerMovement));
        }

        [Test]
        public async Task UpdatePlayerHealth_WhenPlayerTakesDamage_UpdateGameSessionAsyncCalledWithNewPlayerHealth()
        {
            //Arrange
            var gameSessionId = _gameSession.GameSessionId;
            var playerId = _gameSession.PlayersJoiningSession[0].Credentials.PlayerId;

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

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSession);

            //Act
            await _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId);

            //Assert
            await _gameRepository
                .Received(1)
                .UpdateGameSession(_gameSession, Arg.Is<Player>(player => player.PlayerHealth == updatedGameSession.PlayersJoiningSession[0].PlayerHealth));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("!@#$%")]
        public void UpdatePlayerHealth_WhenInvalidPlayerIdIsUsed_ThrowPlayerNotFoundExcpetion(string playerId)
        {
            //Arrange
            var gameSessionId = _gameSession.GameSessionId;

            _gameRepository.GetGameSessionById(gameSessionId).Returns(_gameSession);

            //Act

            //Assert
            Assert.ThrowsAsync<PlayerNotFoundExcpetion>(() => _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId));
        }
    }
}