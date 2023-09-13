using BlueFoxGloveAPI.Hubs;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using MongoDB.Driver.Core.Connections;
using NSubstitute;
using NUnit.Framework;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class GameSessionHubTests
    {
        private IGameRepository _gameRepository;
        private IPlayerRepository _playerRepository;
        private HubCallerContext _hubCallerContext;
        private IGroupManager _groupManager;
        private GameSessionHub _gameHub;
        private IHubCallerClients _clients;
        private IClientProxy _clientProxy;
        private GameSession _gameSession;

        [SetUp]
        public void SetUp()
        {
            _gameRepository = Substitute.For<IGameRepository>();
            _playerRepository = Substitute.For<IPlayerRepository>();
            _groupManager = Substitute.For<IGroupManager>();
            _hubCallerContext = Substitute.For<HubCallerContext>();
            _clients = Substitute.For<IHubCallerClients>();
            _clientProxy = Substitute.For<IClientProxy>();

            _gameHub = new GameSessionHub(_gameRepository, _playerRepository)
            {
                Groups = _groupManager,
                Context = _hubCallerContext,
                Clients = _clients,
            };

            _gameSession = new GameSession
            {
                GameSessionId = "session 1",
                PlayersJoiningSession = new List<Player>
                {
                    new Player
                    {
                        PlayerId = "Player 1",
                        PlayerName = "JohnDoe",
                        PlayerTimestamp = DateTime.Now,
                        PlayerPoints = 0,
                        PlayerHealth = 100,
                        PlayerXCoordinate = 100,
                        PlayerYCoordinate = 100
                    }
                }
            };
        }

        [Test]
        public async Task OnConnectedAsync_WhenAddingClientConnectionToGameSessionGroup_AddToGroupAsyncIsReceived()
        {
            //Arrange
            var connectionId = "testConnectionId";
            var sessionId = "testSessionId";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "sessionId", sessionId } });

            _hubCallerContext.ConnectionId.Returns(connectionId);
            _hubCallerContext.GetHttpContext().Returns(httpContext);

            //Act
            await _gameHub.OnConnectedAsync();

            //Assert
            await _groupManager.Received().AddToGroupAsync(connectionId, sessionId);
        }

        [Test]
        public async Task JoinGameSession_WhenPlayerJoinsGamessesion_GameSessionCollectionConatainsPlayer()
        {
            //Arrange
            var expected = new Player
            {
                PlayerId = "64dd1cf27a6922a9502fc8be",
                PlayerName = "John Doe",
                PlayerTimestamp = DateTime.Now,
                PlayerPoints = 0,
                PlayerHealth = 100,
                PlayerXCoordinate = 100,
                PlayerYCoordinate = 100
            };

            _playerRepository.GetPlayerByIdAsync(expected.PlayerId).Returns(expected);
            _gameRepository.GetGameSessionById(_gameSession.GameSessionId).Returns(_gameSession);

            _gameRepository
                .When(repository => repository.UpdateGameSessionAsync(_gameSession, expected))
                .Do(callback => _gameSession.PlayersJoiningSession.Add(expected));

            //Act
            await _gameHub.JoinGameSession(_gameSession.GameSessionId, expected.PlayerId);
            var actual = _gameSession.PlayersJoiningSession;

            //Assert
            Assert.Contains(expected, actual);
        }
    }
}