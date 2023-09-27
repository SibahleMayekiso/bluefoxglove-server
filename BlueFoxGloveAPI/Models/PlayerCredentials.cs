using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class PlayerCredentials
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId), BsonElement("playerId")]
        public string PlayerId { get; set; }
        [BsonElement("playerName")]
        public string PlayerName { get; set; }
    }
}