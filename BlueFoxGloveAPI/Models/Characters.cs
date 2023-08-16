using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Characters
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("playertype")]
        public string PlayerType { get; set; }

        [BsonElement("health")]
        public int Health { get; set; }
        [BsonElement("playerspeed")]
        public int PlayerSpeed { get; set; }

    }
}
