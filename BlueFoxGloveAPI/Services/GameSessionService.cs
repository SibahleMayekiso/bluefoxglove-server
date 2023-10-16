﻿using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services.Interfaces;

namespace BlueFoxGloveAPI.Services
{
    public sealed class GameSessionService: IGameSessionService
    {
        private readonly ILobbyTimerWrapper _lobbyTimer;
        private readonly IGameSessionRepository _gameSessionRepository;
        private const int _minimumNumberOfPlayers = 5;

        public string GameSessionId { get; set; }

        public GameSessionService(IGameSessionRepository gameRepository, ILobbyTimerWrapper lobbyTimer)
        {
            _gameSessionRepository = gameRepository;
            _lobbyTimer = lobbyTimer;
            _lobbyTimer.Tick += StartGameLobby;
        }

        public void CreateNewGameSession(string oldGameName)
        {
            throw new NotImplementedException();
        }

        private (int x, int y) GenrateRandomPlayerPosition()
        {
            Random random = new Random();
            int randomXPosition = random.Next(0, 1280);
            int randomPosition = random.Next(0, 720);

            return (randomXPosition, randomPosition);
        }

        public async Task<GameSession> JoinGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            Player? player = gameSession.PlayersJoiningSession.Find(_ => _.Credentials.PlayerId == playerId);

            if (player == null)
            {
                throw new PlayerNotFoundExcpetion("Player could not be found");
            }

            var playerCoordinates = GenrateRandomPlayerPosition();
            player.PlayerXCoordinate = playerCoordinates.x;
            player.PlayerYCoordinate = playerCoordinates.y;

            return await _gameSessionRepository.UpdateGameSession(gameSession, player);
        }

        public async Task<GameSession> UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            Player? player = gameSession.PlayersJoiningSession.Find(_ => _.Credentials.PlayerId == playerId);

            if (player == null)
            {
                throw new PlayerNotFoundExcpetion("Player could not be found");
            }

            switch (playerMovement)
            {
                case PlayerMovement.MOVEUP:
                    player.PlayerYCoordinate -= 1;
                    break;
                case PlayerMovement.MOVERIGHT:
                    player.PlayerXCoordinate += 1;
                    break;
                case PlayerMovement.MOVELEFT:
                    player.PlayerXCoordinate -= 1;
                    break;
                case PlayerMovement.MOVEDOWN:
                    player.PlayerYCoordinate += 1;
                    break;
                default:
                    break;
            }

            return await _gameSessionRepository.UpdateGameSession(gameSession, player);
        }

        public async Task<GameSession> UpdatePlayerHealth(string gameSessionId, string playerId)
        {
            const int damageAmount = 1;
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            Player? player = gameSession.PlayersJoiningSession.Find(_ => _.Credentials.PlayerId == playerId);

            if (player == null)
            {
                throw new PlayerNotFoundExcpetion("Player could not be found");
            }

            player.PlayerHealth -= damageAmount;

            return await _gameSessionRepository.UpdateGameSession(gameSession, player);
        }

        public async Task CheckGameLobby()
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(GameSessionId);

            if (gameSession.PlayersJoiningSession.Count < _minimumNumberOfPlayers)
            {
                _lobbyTimer.Change(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));

                return;
            }

            StartGameSession();
        }

        public async void StartGameLobby()
        {
            await CheckGameLobby();
        }

        public void StartGameSession()
        {
            _lobbyTimer.Stop();
        }

        public async Task<GameSession> AddScoreBoardInGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = gameSession.PlayersJoiningSession.Find(_ => _.Credentials.PlayerId == playerId);

            if (player == null)
            {
                throw new PlayerNotFoundExcpetion("Player could not be found");
            }

            player.PlayerScore += 5;

            return await _gameSessionRepository.UpdateGameSession(gameSession, player);
        }
    }
}