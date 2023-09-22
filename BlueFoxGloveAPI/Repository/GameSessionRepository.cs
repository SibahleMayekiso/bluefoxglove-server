using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using MongoDB.Driver;

namespace BlueFoxGloveAPI.Repository
{
    public sealed class GameSessionRepository: IGameSessionRepository
    {
        private readonly IMongoCollection<GameSession> _gameSessionCollection;

        public GameSessionRepository(IMongoDatabase mongoDatabase)
        {
            _gameSessionCollection = mongoDatabase.GetCollection<GameSession>("GameSessionCollection");
        }
        public async Task<List<GameSession>> GetAllAsync()
        {
            var filter = Builders<GameSession>.Filter.Empty;

            return await _gameSessionCollection.Find(filter).ToListAsync();
        }
        public Task<List<Player>> GetPlayersByIdAsync(string GameSessionId, string PlayerId)
        {
            return null;
        }
        public async Task CreateNewGameSessionAsync(GameSession newGameSession)
        {
            await _gameSessionCollection.InsertOneAsync(newGameSession);
        }
        public async Task DeleteGamePlayersAsync(Player playersRemoved)
        {
            throw new NotImplementedException();
        }
        public Task UpdateGameTimeAsync(string gameSessionId, DateTime newTimeStamp)
        {
            throw new NotImplementedException();
        }
        public Task UpdatedPlayerPositionAysnc(string gameSessionId, string playerId, int newX, int newY)
        {
            throw new NotImplementedException();
        }

        public async Task<GameSession> GetGameSessionById(string gameSessionId)
        {
            var filter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSessionId);

            var result = await _gameSessionCollection.FindAsync(filter);

            return result.Current.FirstOrDefault();
        }

        public async Task<GameSession> UpdateGameSessionAsync(GameSession gameSession, Player newPlayer)
        {
            var updatedPlayersSessionList = new List<Player> { newPlayer };
            updatedPlayersSessionList.AddRange(gameSession.PlayersJoiningSession);

            var filter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSession.GameSessionId);
            var update = Builders<GameSession>.Update.Set(field => field.PlayersJoiningSession, updatedPlayersSessionList);

            var result = await _gameSessionCollection.FindOneAndUpdateAsync<GameSession>(filter, update);

            return result;
        }
    }
}