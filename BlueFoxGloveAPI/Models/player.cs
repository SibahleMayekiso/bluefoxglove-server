using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlueFoxGloveAPI.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("points")]
        public int Points { get; set; }

        [BsonElement("timestamp")]
        public DateTime PlayerTime { get; set; }
    }
}
