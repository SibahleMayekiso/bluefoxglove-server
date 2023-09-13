using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class GameSessionRepositoryTest
    {
        private IMongoCollection<GameSession> _gameSessionCollection;
        private IMongoDatabase _mongoDatabase;
        private IAsyncCursor<GameSession> _cursor;
        private GameSessionRepository _gameRepository;
        private List<GameSession> _gameSessions;

        [SetUp]
        public void Setup()
        {
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _gameSessionCollection = Substitute.For<IMongoCollection<GameSession>>();
            _mongoDatabase.GetCollection<GameSession>("GameSessionCollection").Returns(_gameSessionCollection);
            _cursor = Substitute.For<IAsyncCursor<GameSession>>();
            _gameRepository = new GameSessionRepository(_mongoDatabase);

            _gameSessions = new List<GameSession>
            {
                new GameSession
                {
                    GameSessionId = "64dd1cf27a6922a9505fc8be",
                    GameSessionTimeStamp = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    PlayersJoiningSession = new List<Player>
                    {
                        new Player
                        {
                            PlayerId = "Player 1",
                            PlayerName = "JohnDoe",
                            PlayerTimestamp = new DateTime(2023, 1, 1, 12, 0, 5, DateTimeKind.Utc),
                            PlayerPoints = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        }
                    }
                },
                new GameSession
                {
                    GameSessionId = "64dd1cf27a6922a9505fc8ba",
                    GameSessionTimeStamp = new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc),
                    PlayersJoiningSession = new List<Player>
                    {
                        new Player
                        {
                            PlayerId = "Player 1",
                            PlayerName = "JohnDoe",
                            PlayerTimestamp = new DateTime(2023, 1, 1, 12, 0, 10, DateTimeKind.Utc),
                            PlayerPoints = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 100,
                            PlayerYCoordinate = 100
                        },
                        new Player
                        {
                            PlayerId = "Player 2",
                            PlayerName = "JamesDoe",
                            PlayerTimestamp = new DateTime(2023, 1, 1, 12, 0, 15, DateTimeKind.Utc),
                            PlayerPoints = 0,
                            PlayerHealth = 100,
                            PlayerXCoordinate = 150,
                            PlayerYCoordinate = 150
                        }
                    }
                }
            };
        }

        [Test]
        public async Task GetGameSessionById_WithAnExistingGameSessionId_ReturnGameSessionContainingGameSessionId()
        {
            //Arrange
            string gameSessionId = _gameSessions[0].GameSessionId;
            var expected = _gameSessions[0];

            _cursor.Current.Returns(new List<GameSession> { expected });

            _gameSessionCollection
                .FindAsync<GameSession>(Arg.Any<FilterDefinition<GameSession>>())
                .ReturnsForAnyArgs(Task.FromResult(_cursor));

            //Act
            var actual = await _gameRepository.GetGameSessionById(gameSessionId);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateGameSessionAsync_WhenPlayerJoinsGameSession_GameSessionContainsNewPlayer()
        {
            //Arrange
            var gameSession = _gameSessions[0];
            var newPlayer = new Player
            {
                PlayerId = "Player 3",
                PlayerName = "JacobDoe",
                PlayerTimestamp = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                PlayerPoints = 0,
                PlayerHealth = 100,
                PlayerXCoordinate = 150,
                PlayerYCoordinate = 300
            };
            var updatedPlayersSessionList = new List<Player> { newPlayer };
            updatedPlayersSessionList.AddRange(gameSession.PlayersJoiningSession);

            var updatedGameSesion = new GameSession
            {
                GameSessionId = gameSession.GameSessionId,
                GameSessionTimeStamp = gameSession.GameSessionTimeStamp,
                PlayersJoiningSession = gameSession.PlayersJoiningSession
            };

            updatedGameSesion.PlayersJoiningSession = updatedPlayersSessionList;

            _gameSessionCollection
                .FindOneAndUpdateAsync<GameSession>(Arg.Any<FilterDefinition<GameSession>>(), Arg.Any<UpdateDefinition<GameSession>>())
                .ReturnsForAnyArgs(Task.FromResult(updatedGameSesion));

            //Act
            var result = await _gameRepository.UpdateGameSessionAsync(gameSession, newPlayer);
            var actual = result.PlayersJoiningSession;

            //Assert
            Assert.Contains(newPlayer, actual);
        }

        [Test]
        public async Task CreateNewGameSessionAsync_CreateValidGameSession_GameSessionCollectionContainsNewGameSession()
        {
            //Arrange
            var newGame = new GameSession
            {
                GameSessionId = "newGameSessionId",
                GameSessionTimeStamp = DateTime.Now,
                PlayersJoiningSession = new List<Player>
                {
                    new Player {PlayerId = "playerId", PlayerName = "Jane Doe"}
                }
            };

            _gameSessionCollection
                .When(collection => collection.InsertOneAsync(newGame))
                .Do(callback => _gameSessions.Add(newGame));

            //Act 
            await _gameRepository.CreateNewGameSessionAsync(newGame);

            //Assert
            Assert.Contains(newGame, _gameSessions);
        }
    }
}