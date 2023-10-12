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
        private GameSessionRepository _gameSessionRepository;
        private List<GameSession> _gameSessions;

        [SetUp]
        public void Setup()
        {
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _gameSessionCollection = Substitute.For<IMongoCollection<GameSession>>();
            _mongoDatabase.GetCollection<GameSession>("GameSessionCollection").Returns(_gameSessionCollection);
            _cursor = Substitute.For<IAsyncCursor<GameSession>>();
            _gameSessionRepository = new GameSessionRepository(_mongoDatabase);

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
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player1",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 5, DateTimeKind.Utc),
                            PlayerScore = 0,
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
                            Credentials = new PlayerCredentials
                            {
                                PlayerId = "player1",
                                PlayerName = "John Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 10, DateTimeKind.Utc),
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
                                PlayerName = "James Doe"
                            },
                            PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 15, DateTimeKind.Utc),
                            PlayerScore = 0,
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
            _cursor.MoveNextAsync().Returns(Task.FromResult(true));

            _gameSessionCollection
                .FindAsync<GameSession>(Arg.Any<FilterDefinition<GameSession>>())
                .ReturnsForAnyArgs(Task.FromResult(_cursor));

            //Act
            var actual = await _gameSessionRepository.GetGameSessionById(gameSessionId);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateGameSession_WhenPlayerJoinsGameSession_GameSessionContainsNewPlayer()
        {
            //Arrange
            var gameSession = _gameSessions[0];
            var newPlayer = new Player
            {
                Credentials = new PlayerCredentials
                {
                    PlayerId = "player3",
                    PlayerName = "Jacob Doe"
                },
                PlayerExitTime = new DateTime(2023, 1, 1, 12, 0, 30, DateTimeKind.Utc),
                PlayerScore = 0,
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
            var result = await _gameSessionRepository.UpdateGameSession(gameSession, newPlayer);
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
                    new Player
                    {
                        Credentials = new PlayerCredentials
                        {
                            PlayerId = "64dd1cf27a6922a9502fc8be",
                            PlayerName = "John Doe"
                        },
                    }
                }
            };
            _gameSessionCollection
               .When(collection => collection.InsertOneAsync(newGame))
               .Do(callback => _gameSessions.Add(newGame));

            //Act 
            await _gameSessionRepository.CreateNewGameSession(newGame);

            //Assert
            Assert.Contains(newGame, _gameSessions);
        }
    }
}