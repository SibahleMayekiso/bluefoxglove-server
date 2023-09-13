using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Characters
    {
        [BsonElement("charactername")]
        public string CharacterName { get; set; }
        [BsonElement("characterType")]
        public string CharacterType { get; set; }
        [BsonElement("characterMaxHealth")]
        public int CharacterMaxHealth { get; set; }
        [BsonElement("characterMaxSpeed")]
        public int CharacterMaxSpeed { get; set; }
    }
}