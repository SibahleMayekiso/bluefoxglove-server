using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class GameSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId), BsonElement("gameSessionId")]
        public string GameSessionId { get; set; }
        [BsonElement("gameName")]
        public string GameName { get; set; }
        [BsonElement("gameSessionTimestamp")]
        public DateTime GameSessionTimeStamp { get; set; }
        [BsonElement("playersJoiningSession")]
        public List<Player> PlayersJoiningSession { get; set; }
    }
}