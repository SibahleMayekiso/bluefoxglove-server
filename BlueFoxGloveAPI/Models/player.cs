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
        [BsonElement("playerPoints")]
        public int PlayerPoints { get; set; }
        [BsonElement("playerTimestamp")]
        public DateTime PlayerTimestamp { get; set; }
        [BsonElement("playerHealth")]
        public int PlayerHealth { get; set; }
    }
}