using MongoDB.Bson.Serialization.Attributes;

namespace BlueFoxGloveAPI.Models
{
    public class Characters
    {
        [BsonElement("charactername")]
        public string CharacterName { get; set; }

        [BsonElement("charactertype")]
        public string CharacterType { get; set; }

        [BsonElement("charactermaxhealth")]
        public int CharacterMaxHealth { get; set; }

        [BsonElement("charactermaxspeed")]
        public int CharacterMaxSpeed { get; set; }

    }
}
