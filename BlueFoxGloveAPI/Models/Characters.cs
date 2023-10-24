using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Characters
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CharacterId { get; set; }
        [BsonElement("charactername")]
        public string CharacterName { get; set; }
        [BsonElement("characterType")]
        public string CharacterType { get; set; }
        [BsonElement("characterMaxHealth")]
        public int CharacterMaxHealth { get; set; }
        [BsonElement("characterMaxSpeed")]
        public double CharacterMaxSpeed { get; set; }
    }
}