using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    [BsonIgnoreExtraElements]
    public class GameSession
    {
        [BsonElement("gameSessionId")]
        public string GameSessionId { get; set; }
        [BsonElement("gameName")]
        public string GameName { get; set; }
        [BsonElement("gameSessionTimeStamp")]
        public DateTime GameSessionTimeStamp { get; set; }
        [BsonElement("playersJoiningSession")]
        public List<Player> PlayersJoiningSession { get; set; }
    }
}