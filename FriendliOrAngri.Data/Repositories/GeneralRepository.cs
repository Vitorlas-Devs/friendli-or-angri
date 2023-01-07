using MongoDB.Driver;
using System.Configuration;

namespace FriendliOrAngriASP.Data.Repositories;

public abstract class GeneralRepository<T>
    where T : class
{
	protected readonly MongoClient dbClient;

	protected virtual IMongoDatabase database => dbClient.GetDatabase("friendliOrAngri");

	public GeneralRepository()
	{
		string connectionString = ConfigurationManager
			.ConnectionStrings["friendliOrAngri"]?
			.ConnectionString;
        this.dbClient = new("mongodb://localhost:27017");
	}

	public abstract IEnumerable<T> GetAll();
	public abstract T Insert(T item);
	public abstract void Delete(int id);
}
