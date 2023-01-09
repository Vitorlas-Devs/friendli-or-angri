using FriendliOrAngri.WebAPI.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendliOrAngri.WebAPI.Data.Models;

public class GameModel
{
    [BsonId]
    public string Token { get; set; }
    public GameMode GameMode { get; set; }
    public byte LivesLeft { get; set; }
    public DateTime Date { get; set; }
    public SoftwareModel CurrentSoftware { get; set; }
    public List<SoftwareModel> LastSoftwares { get; set; }
}
