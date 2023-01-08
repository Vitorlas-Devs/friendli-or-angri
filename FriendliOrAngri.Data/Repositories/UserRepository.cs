using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;
using System.Configuration;
using System.Security.Cryptography;

namespace FriendliOrAngri.WebAPI.Data.Repositories;

public class UserRepository
{
    protected readonly MongoClient dbClient;
    private IMongoCollection<UserModel> collection;

    protected IMongoDatabase database => dbClient.GetDatabase("friendliOrAngri");

    public UserRepository()
    {
        string connectionString = ConfigurationManager
            .ConnectionStrings["friendliOrAngri"]?
            .ConnectionString;
        this.dbClient = new("mongodb://localhost:27017");
        this.collection = this.database.GetCollection<UserModel>("users");
    }

    public IEnumerable<UserModel> GetAll()
    {
        List<UserModel> users = this.collection.AsQueryable().ToList();
        foreach (UserModel user in users)
            yield return user;
    }

    public UserModel Insert(string userName)
    {
        using var r = RandomNumberGenerator.Create();
        byte[] tokenData = new byte[16];
        r.GetBytes(tokenData);

        UserModel model = new()
        {
            Token = new Guid(tokenData).ToString(),
            Name = userName,
            Id = 1,
            Scores = new()
        };

        List<UserModel> filteredUsers = GetAll()
            .Where(u => u.Name == model.Name)
            .ToList();

        if (filteredUsers.Count > 0)
            model.Id = filteredUsers.Max(u => u.Id) + 1;

        this.collection.InsertOne(model);
        return model;
    }

    public void Delete(string userName, int id) =>
        this.collection.DeleteOne(u => u.Name == userName && u.Id == id);
}
