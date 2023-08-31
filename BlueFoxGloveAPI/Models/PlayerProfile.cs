using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlueFoxGloveAPI.Models
{
    public class PlayerProfile
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? PlayerId { get; set; }

        [BsonElement("playername"), Required, MaxLength(24)]
        public string? PlayerName { get; set; }

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
