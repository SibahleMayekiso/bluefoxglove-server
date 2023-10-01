﻿using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services;
using BlueFoxGloveAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlueFoxGloveAPI.Hubs
{
    public class GameSessionHub: Hub
    {
        private readonly IGameSessionService _gameSessionService;

        private readonly IGameSessionRepository _gameSessionRepository;
        private readonly IPlayerRepository _playerRepository;
        private static readonly Dictionary<string, GameSession> _gameSessions = new Dictionary<string, GameSession>();
        private static readonly object _lockObject = new object();
        private Timer? _timer;
        private const int _maxPlayers = 10;
        private const int _waitingLobbyTimeLimitInSeconds = 300;
        private int _gameIdCounter = 0;

        public GameSessionHub(IGameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
        }

        public GameSessionHub(IGameSessionRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameSessionRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var sessionId = httpContext.Request.Query["sessionId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

            string gameName;

            lock (_lockObject)
            {
                gameName = $"Game_{_gameIdCounter}";

                if (!_gameSessions.ContainsKey(gameName))
                {
                    _gameSessions[gameName] = new GameSession
                    {
                        GameSessionId = "",
                        GameName = gameName,
                        GameSessionTimeStamp = DateTime.UtcNow,
                        PlayersJoiningSession = new List<Player>()
                    };

                    _timer = new Timer(state => CreateNewGameSession(gameName), null
                        , TimeSpan.FromSeconds(_waitingLobbyTimeLimitInSeconds), Timeout.InfiniteTimeSpan);
                }

                if (PlayersJoiningSession(gameName) <= _maxPlayers)
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, gameName);
                }
            }

            await base.OnConnectedAsync();
        }

        private void CreateNewGameSession(string oldGameName)
        {
            lock (_lockObject)
            {
                _timer?.Dispose();

                if (_gameSessions.TryGetValue(oldGameName, out var oldGameSession))
                {
                    _gameSessionRepository.CreateNewGameSession(oldGameSession);
                }

                _gameSessions.Remove(oldGameName);
                _gameIdCounter++;
                string newGameName = $"Game_{_gameIdCounter}";
                _gameSessions[newGameName] = new GameSession
                {
                    GameSessionId = "",
                    GameName = newGameName,
                    GameSessionTimeStamp = DateTime.UtcNow,
                    PlayersJoiningSession = new List<Player>()
                };
                Groups.AddToGroupAsync(Context.ConnectionId, newGameName);
            }
        }

        private int PlayersJoiningSession(string gameName)
        {
            if (_gameSessions.TryGetValue(gameName, out var gameSession))
            {
                return gameSession.PlayersJoiningSession.Count;
            }

            return 0;
        }

        private (int x, int y) GenrateRandomPlayerPosition()
        {
            Random random = new Random();
            int randomXPosition = random.Next(0, 1280);
            int randomPosition = random.Next(0, 720);

            return (randomXPosition, randomPosition);
        }

        public async Task JoinGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = await _playerRepository.GetPlayerById(playerId);

            var playerCoordinates = GenrateRandomPlayerPosition();
            player.PlayerXCoordinate = playerCoordinates.x;
            player.PlayerYCoordinate = playerCoordinates.y;

            var updatedGameSession = await _gameSessionRepository.UpdateGameSession(gameSession, player);

            await Clients.Group(gameSessionId).SendAsync("PlayerJoiningGame", updatedGameSession);
        }

        public async Task AddScoreBoardInGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = await _playerRepository.GetPlayerById(playerId);
            player.PlayerScore += 5;

            var newGameSession = await _gameSessionRepository.UpdateGameSession(gameSession, player);

            await Clients.Group(gameSessionId).SendAsync("UpdateLeaderBoard", newGameSession.PlayersJoiningSession);
        }

        public async Task UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement)
        {
            var updatedGameSession = await _gameSessionService.UpdatePlayerPostion(gameSessionId, playerId, playerMovement);

            await Clients.Group(gameSessionId).SendAsync("UpdatePlayerPosition", updatedGameSession);
        }

        public async Task UpdatePlayerHealth(string gameSessionId, string playerId)
        {
            var updatedGameSession = await _gameSessionService.UpdatePlayerHealth(gameSessionId, playerId);

            await Clients.Group(gameSessionId).SendAsync("UpdatePlayerHealth", updatedGameSession);
        }
    }
}