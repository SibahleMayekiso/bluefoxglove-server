using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId PlayerId { get; set; }
        [BsonElement("gamename")]
        public string GameName { get; set; }
        [BsonElement("xco-ordinate")]
        public int XCoordinate { get; set; }
        [BsonElement("yco-ordinate")]
        public int YCoordinate { get; set; }
        [BsonElement("playercolor")]
        public string PlayerColor { get; set; }
        [BsonElement("points")]
        public int Points { get; set; }
        [BsonElement("timestamp")]
        public DateTime GameTimeStamp { get; set; }
        [BsonElement("health")]
        public int Health { get; set; }


    }
}
