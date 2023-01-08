﻿using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendliOrAngri.Data.Models;

public class UserModel
{
    [BsonId]
    public string Token { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public List<ScoreModel> Scores { get; set; }
}
