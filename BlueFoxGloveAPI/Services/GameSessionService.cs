﻿using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services.Interfaces;
using System.Collections.Concurrent;

namespace BlueFoxGloveAPI.Services
{
    public sealed class GameSessionService: IGameSessionService
    {
        private readonly ILobbyTimerWrapper _lobbyTimer;
        private readonly IGameSessionTimerWrapper _gameSessionTimer;
        private readonly IGameSessionRepository _gameSessionRepository;
        private const int _minimumNumberOfPlayers = 5;

        public string GameSessionId { get; set; }
        public int ProjectileId { get; set; } = 0;
        public ConcurrentDictionary<int, Projectile> ProjectilesInPlay { get; set; }
        public List<Player> SurvivngPlayers { get; set; }

        public GameSessionService(IGameSessionRepository gameRepository, ILobbyTimerWrapper lobbyTimer, IGameSessionTimerWrapper gameSessionTimer)
        {
            _gameSessionRepository = gameRepository;
            _lobbyTimer = lobbyTimer;
            _gameSessionTimer = gameSessionTimer;
            _lobbyTimer.Tick += StartGameLobby;
            _gameSessionTimer.Tick += EndGameSession;
            ProjectilesInPlay = new ConcurrentDictionary<int, Projectile>();
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
                throw new PlayerNotFoundException("Player could not be found");
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
                throw new PlayerNotFoundException("Player could not be found");
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
                throw new PlayerNotFoundException("Player could not be found");
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

        public async Task CheckSurvivingPlayers()
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(GameSessionId);

            var survivingPlayers = gameSession.PlayersJoiningSession.FindAll(_ => _.PlayerHealth > 0);

            SurvivngPlayers = survivingPlayers;
        }

        public async void StartGameLobby()
        {
            await CheckGameLobby();
        }

        public void StartGameSession()
        {
            _lobbyTimer.Stop();
            _gameSessionTimer.Start();
        }

        public async void EndGameSession()
        {
            await CheckSurvivingPlayers();
        }

        public async Task<GameSession> AddScoreBoardInGameSession(string gameSessionId, string playerId)
        {
            var gameSession = await _gameSessionRepository.GetGameSessionById(gameSessionId);
            var player = gameSession.PlayersJoiningSession.Find(_ => _.Credentials.PlayerId == playerId);

            if (player == null)
            {
                throw new PlayerNotFoundException("Player could not be found");
            }

            player.PlayerScore += 5;

            return await _gameSessionRepository.UpdateGameSession(gameSession, player);
        }

        public Projectile CreateProjectile(string playerId, Vector position, Vector velocity)
        {
            const int projectileSpeed = 5;
            ProjectileId++;

            if (position.X > 1280 || position.X < 0)
            {
                throw new ProjectileCreationException();
            }

            if (position.Y > 720 || position.Y < 0)
            {
                throw new ProjectileCreationException();
            }

            return new Projectile { ProjectileId = ProjectileId, PlayerId = playerId, Position = position, Speed = projectileSpeed, Velocity = velocity };
        }

        public void FireProjectile(string playerId, Vector position, Vector velocity)
        {
            var newProjectile = CreateProjectile(playerId, position, velocity);

            ProjectilesInPlay.TryAdd(newProjectile.ProjectileId, newProjectile);
        }

        public void DisposeProjectile(int projectileId)
        {
            var projectile = ProjectilesInPlay.GetValueOrDefault(projectileId);

            if (projectile == null)
            {
                throw new InvalidOperationException($"Something went wrong. Attempted to dispose projectile with ID {projectileId} which does not exist");
            }

            ProjectilesInPlay.TryRemove(projectileId, out _);
        }

        public void UpdateProjectilePosition(int projectileId)
        {
            var projectile = ProjectilesInPlay.GetValueOrDefault(projectileId);

            if (projectile == null)
            {
                throw new InvalidOperationException($"Something went wrong. Attempted to dispose projectile with ID {projectileId} which does not exist");
            }

            Vector updatedProjectilePosition = projectile.Position;
            updatedProjectilePosition.X += projectile.Velocity.X;
            updatedProjectilePosition.Y += projectile.Velocity.Y;

            projectile.Position = updatedProjectilePosition;
        }

        public async Task<GameSession> RemovePlayerFromGameSession(string playerId)
        {
            return await _gameSessionRepository.RemovePlayerFromGameSession(GameSessionId, playerId);
        }
    }
}