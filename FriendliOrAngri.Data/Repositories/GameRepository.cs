using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;

namespace FriendliOrAngri.WebAPI.Data.Repositories;

public class GameRepository
{
    protected readonly MongoClient dbClient;
    private IMongoCollection<GameModel> games;
    private IMongoCollection<UserModel> users;

    protected IMongoDatabase database => dbClient.GetDatabase("friendliOrAngri");

    public GameRepository()
    {
        this.dbClient = new("mongodb://localhost:27017");
        this.games = this.database.GetCollection<GameModel>("games");
        this.users = this.database.GetCollection<UserModel>("users");
    }

    public void CreateNewGame(string userToken, GameMode gameMode)
    {
        if (!this.users.AsQueryable().Any(u => u.Token == userToken))
            throw new ArgumentException("Nincs ilyen token-ű felhassználó!");

        this.games.DeleteMany(g => g.UserToken == userToken);

        byte livesLeft = 0;
        switch (gameMode)
        {
            case GameMode.Normal:
                livesLeft = 5;
                break;
            case GameMode.Hardcore:
                livesLeft = 1;
                break;
            default:
                break;
        }

        GameModel newGame = new()
        {
            UserToken = userToken,
            GameMode = gameMode,
            LivesLeft = livesLeft,
            Date = DateTime.Now,
            CurrentSoftware = null,
            LastSoftwares = new()
        };

        this.games.InsertOne(newGame);
    }
}
