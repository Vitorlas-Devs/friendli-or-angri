using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;
using System.Text.Json;

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
        if (IsValidToken(userToken))
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

    public GameModel GetSoftware(string userToken)
    {
        if (IsValidToken(userToken))
            throw new ArgumentException(
                "Nincs ilyen token-ű felhassználó!");

        GameModel game = this.games.AsQueryable()
            .SingleOrDefault(g => g.UserToken == userToken);

        if (game is null)
            throw new MissingMemberException
                ("Nincs játék létrehozva! Missing `CreateNewGame`?");

        SoftwareModel software;
        do software = GetRandomSoftware();
        while (game.LastSoftwares.Any(s => s.Name == software.Name));

        game.CurrentSoftware = software;
        game.LastSoftwares.Add(software);
        int uniqueLimit = 0;
        switch (game.GameMode)
        {
            case GameMode.Normal:
                uniqueLimit = 100;
                break;
            case GameMode.Hardcore:
                uniqueLimit = 200;
                break;
        }
        if (game.LastSoftwares.Count > uniqueLimit)
            game.LastSoftwares.RemoveAt(0);

        games.InsertOne(game);

        game.CurrentSoftware.Description = null;
        game.CurrentSoftware.IsFriendli = false;
        game.LastSoftwares = new();

        return game;
    }

    private SoftwareModel GetRandomSoftware()
    {
        Random r = new Random();
        bool isFriendli = r.Next(0, 2) == 1;
        string fileName = "data_angri.json";
        if (isFriendli)
            fileName = "data_friendli.json";

        List<SoftwareModel> softwares = JsonSerializer
            .Deserialize<List<SoftwareModel>>(
                File.ReadAllText(fileName));

        SoftwareModel software = softwares[r.Next(0, softwares.Count - 1)];
        software.IsFriendli = isFriendli;

        return software;
    }

    private bool IsValidToken(string token) =>
        !this.users.AsQueryable().Any(u => u.Token == token);
}
