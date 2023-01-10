using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace FriendliOrAngri.WebAPI.Data.Repositories;

public class UserRepository
{
    protected readonly MongoClient dbClient;
    private IMongoCollection<UserModel> users;

    protected IMongoDatabase database => dbClient.GetDatabase("friendliOrAngri");

    public UserRepository()
    {
        this.dbClient = new("mongodb://localhost:27017");
        this.users = this.database.GetCollection<UserModel>("users");
    }

    public IEnumerable<UserModel> GetAll()
    {
        List<UserModel> users = this.users.AsQueryable().ToList();
        foreach (UserModel user in users)
            yield return user;
    }

    public UserModel GetUserByToken(string token) =>
        GetAll().SingleOrDefault(u => u.Token == token);

    public UserModel Insert(string userName)
    {
        userName = userName.Trim();

        if (userName.Length < 3 || userName.Length > 25)
            throw new ArgumentException("Nem megfelő a felhasználónév hossza!");

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

        this.users.InsertOne(model);
        return model;
    }

    public void Update(UserModel user) =>
        this.users.ReplaceOne(c => c.Token == user.Token, user);

    public void Delete(string userName, int id) =>
        this.users.DeleteOne(u => u.Name == userName && u.Id == id);
}
