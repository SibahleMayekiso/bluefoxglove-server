﻿using BlueFoxGloveAPI.Models;
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
        public async Task<List<GameSession>> GetAll()
        {
            var filter = Builders<GameSession>.Filter.Empty;

            return await _gameSessionCollection.Find(filter).ToListAsync();
        }
        public Task<List<Player>> GetPlayersById(string GameSessionId, string PlayerId)
        {
            return null;
        }
        public async Task CreateNewGameSession(GameSession newGameSession)
        {
            await _gameSessionCollection.InsertOneAsync(newGameSession);
        }
        public async Task DeleteGamePlayers(Player playersRemoved)
        {
            throw new NotImplementedException();
        }
        public Task UpdateGameTime(string gameSessionId, DateTime newTimeStamp)
        {
            throw new NotImplementedException();
        }
        public Task UpdatedPlayerPosition(string gameSessionId, string playerId, int newX, int newY)
        {
            throw new NotImplementedException();
        }

        public async Task<GameSession> GetGameSessionById(string gameSessionId)
        {
            var filter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSessionId);
            var result = await _gameSessionCollection.FindAsync(filter);

            return await result.SingleAsync();
        }

        public async Task<GameSession> UpdateGameSession(GameSession gameSession, Player newPlayer)
        {
            var updatedPlayersSessionList = new List<Player> { newPlayer };
            updatedPlayersSessionList.AddRange(gameSession.PlayersJoiningSession);

            var filter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSession.GameSessionId);
            var update = Builders<GameSession>.Update.Set(field => field.PlayersJoiningSession, updatedPlayersSessionList);

            var result = await _gameSessionCollection.FindOneAndUpdateAsync<GameSession>(filter, update);

            return result;
        }

        public async Task<GameSession> RemovePlayerFromGameSession(string gameSessionId, string playerId)
        {
            var findFilter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSessionId);
            var gameSession = await _gameSessionCollection.FindAsync(findFilter);

            var updatedPlayersSessionList = gameSession.Current.First().PlayersJoiningSession.FindAll(_ => _.Credentials.PlayerId != playerId);

            var updatefilter = Builders<GameSession>.Filter.Eq(field => field.GameSessionId, gameSessionId);
            var update = Builders<GameSession>.Update.Set(field => field.PlayersJoiningSession, updatedPlayersSessionList);

            return await _gameSessionCollection.FindOneAndUpdateAsync<GameSession>(updatefilter, update);
        }
    }
}