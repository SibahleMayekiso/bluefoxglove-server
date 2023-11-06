using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace BlueFoxGloveAPI.Models
{
    public class PlayerProfile
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId), BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("PlayerCredentials")]
        public PlayerCredentials Credentials { get; set; }
        [BsonElement("selectedCharacter")]
        public Characters SelectedCharacter { get; set; }
        [BsonElement("longestSurvivalTime"), DefaultValue(0)]
        public int LongestSurvivalTime { get; set; }
        [BsonElement("totalPlayTime"), DefaultValue(0)]
        public int TotalPlayTime { get; set; }
        [BsonElement("numberOfGamesPlayed"), DefaultValue(0)]
        public int NumberOfGamesPlayed { get; set; }
        [BsonElement("killCount"), DefaultValue(0)]
        public int KillCount { get; set; }
    }
}