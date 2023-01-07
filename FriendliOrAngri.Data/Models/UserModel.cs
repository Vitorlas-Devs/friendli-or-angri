using MongoDB.Bson.Serialization.Attributes;

namespace FriendliOrAngri.Data.Models;

public class UserModel
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; }
}
