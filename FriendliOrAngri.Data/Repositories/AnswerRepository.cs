using FriendliOrAngri.Data.Models;
using FriendliOrAngriASP.Data.Repositories;
using MongoDB.Driver;

namespace FriendliOrAngri.Data.Repositories;

public class AnswerRepository : GeneralRepository<AnswerModel>
{
    private IMongoCollection<AnswerModel> collection;

    public AnswerRepository() =>
        this.collection = this.database.GetCollection<AnswerModel>("answers");

    public override IEnumerable<AnswerModel> GetAll()
    {
        List<AnswerModel> answers = this.collection.AsQueryable().ToList();
        foreach (AnswerModel answer in answers)
            yield return answer;
    }

    public override AnswerModel Insert(AnswerModel model)
    {
        model.Id = 1;
        if (GetAll().Count() > 0)
            model.Id = GetAll().Max(u => u.Id) + 1;
        this.collection.InsertOne(model);
        return model;
    }

    public override void Delete(int id) =>
        this.collection.DeleteOne(a => a.Id == id);
}
