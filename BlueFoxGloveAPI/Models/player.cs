using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace BlueFoxGloveAPI.Models
{
    public class Player
    {
        [BsonElement("PlayerCredentials")]
        public PlayerCredentials Credentials { get; set; }
        [BsonElement("playerXCoordinate"), BsonDefaultValue(640)]
        public int PlayerXCoordinate { get; set; }
        [BsonElement("playerYCoordinate"), DefaultValue(360)]
        public int PlayerYCoordinate { get; set; }
        [BsonElement("playerScore")]
        public int PlayerScore { get; set; }
        [BsonElement("playerExitTime")]
        public DateTime PlayerExitTime { get; set; }
        [BsonElement("playerHealth"), BsonDefaultValue(100)]
        public int PlayerHealth { get; set; }
    }
}