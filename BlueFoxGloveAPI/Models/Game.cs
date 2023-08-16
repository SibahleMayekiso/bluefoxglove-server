using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Game
    {
        [BsonId]
        [BsonElement("gamesessionid")]
        public string GameSessionId { get; set; }

        [BsonElement("gamesessiontimestamp")]
        public DateTime GameSessionTimeStamp { get; set; }

        [BsonElement("playersjoiningsession")]
        public List<Player> PlayersJoiningSession { get; set; }
    }
}
