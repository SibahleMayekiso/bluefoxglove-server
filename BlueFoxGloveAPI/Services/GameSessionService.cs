using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using BlueFoxGloveAPI.Services.Interfaces;

namespace BlueFoxGloveAPI.Services
{
    public sealed class GameSessionService: IGameSessionService
    {
        private readonly IGameSessionRepository _gameRepository;

        public GameSessionService(IGameSessionRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void CreateNewGameSession(string oldGameName)
        {
            throw new NotImplementedException();
        }

        private (int x, int y) GenrateRandomPlayerPosition()
        {
            throw new NotImplementedException();
        }

        public Task JoinGameSession(string gameSessionId, string playerId)
        {
            throw new NotImplementedException();
        }

        public async Task<GameSession> UpdatePlayerPostion(string gameSessionId, string playerId, PlayerMovement playerMovement)
        {
            var gameSession = await _gameRepository.GetGameSessionById(gameSessionId);
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

            return await _gameRepository.UpdateGameSession(gameSession, player);
        }
    }
}