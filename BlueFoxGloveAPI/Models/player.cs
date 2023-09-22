using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Player
    {
        [BsonElement("PlayerCredentials")]
        public PlayerCredentials Credentials { get; set; }
        [BsonElement("playerXCoordinate")]
        public int PlayerXCoordinate { get; set; }
        [BsonElement("playerYCoordinate")]
        public int PlayerYCoordinate { get; set; }
        [BsonElement("playerScore")]
        public int PlayerScore { get; set; }
        [BsonElement("playerExitTime")]
        public DateTime PlayerExitTime { get; set; }
        [BsonElement("playerHealth")]
        public int PlayerHealth { get; set; }
    }
}