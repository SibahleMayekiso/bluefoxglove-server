using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlueFoxGloveAPI.Models
{
    public class player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("points")]
        public int points { get; set; }

        [BsonElement("timestamp")]
        public DateTime PlayerTime { get; set; }
    }
}
