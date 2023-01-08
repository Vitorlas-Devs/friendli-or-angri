using FriendliOrAngri.WebAPI.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendliOrAngri.WebAPI.Data.Models;

public class ScoreModel
{
    [BsonId]
    public int Id { get; set; }
    public int Score { get; set; }
    public GameMode GameMode { get; set; }
    public DateTime Date { get; set; }
}
