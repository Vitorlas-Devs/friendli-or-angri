using FriendliOrAngri.Data.Models;
using FriendliOrAngriASP.Data.Repositories;
using MongoDB.Driver;

namespace FriendliOrAngri.Data.Repositories;

public class UserRepository : GeneralRepository<UserModel>
{
    private IMongoCollection<UserModel> collection;

    public UserRepository() =>
        this.collection = this.database.GetCollection<UserModel>("users");

    public override IEnumerable<UserModel> GetAll()
    {
        List<UserModel> users = this.collection.AsQueryable().ToList();
        foreach (UserModel user in users)
            yield return user;
    }

    public override UserModel Insert(UserModel model)
    {
        model.Id = 1;
        if (GetAll().Count() > 0)
            model.Id = GetAll().Max(u => u.Id) + 1;
        this.collection.InsertOne(model);
        return model;
    }

    public override void Delete(int id) =>
        this.collection.DeleteOne(u => u.Id == id);
}
