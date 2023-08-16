using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlueFoxGloveAPI.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerId { get; set; }

        [BsonElement("playername")]
        public string PlayerName { get; set; }

        [BsonElement("playerxcoordinate")]
        public int PlayerXCoordinate { get; set; }

        [BsonElement("playerycoordinate")]
        public int PlayerYCoordinate { get; set; }

        [BsonElement("playerpoints")]
        public int PlayerPoints { get; set; }

        [BsonElement("playertimestamp")]
        public DateTime PlayerTimestamp { get; set; }

        [BsonElement("playerhealth")]
        public int PlayerHealth { get; set; }
    }
}
