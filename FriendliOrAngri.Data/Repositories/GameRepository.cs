using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace FriendliOrAngri.WebAPI.Data.Repositories;

public class GameRepository
{
    protected readonly MongoClient dbClient;
    private IMongoCollection<GameModel> games;

    protected IMongoDatabase database => dbClient.GetDatabase("friendliOrAngri");

    public GameRepository()
    {
        this.dbClient = new("mongodb://localhost:27017");
        this.games = this.database.GetCollection<GameModel>("games");
    }

    public void CreateNewGame(string userToken, GameMode gameMode)
    {
        if (!IsValidToken(userToken))
            throw new MissingMemberException("Nincs ilyen token-ű felhassználó!");

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
        if (!IsValidToken(userToken))
            throw new MissingMemberException(
                "Nincs ilyen token-ű felhassználó!");

        GameModel game = this.games.AsQueryable()
            .SingleOrDefault(g => g.UserToken == userToken);

        if (game is null)
            throw new MissingMemberException
                ("Nincs játék létrehozva! Missing `CreateNewGame`?");

        byte answerTime = 0;
        int uniqueLimit = 0;
        switch (game.GameMode)
        {
            case GameMode.Normal:
                answerTime = 15;
                uniqueLimit = 100;
                break;
            case GameMode.Hardcore:
                answerTime = 5;
                uniqueLimit = 200;
                break;
        }

        game.Date = DateTime.Now
            .ToUniversalTime()
            .AddSeconds(answerTime);

        SoftwareModel software;
        do software = GetRandomSoftware();
        while (game.LastSoftwares
            .Take(uniqueLimit)
            .Any(s => s.Name == software.Name));

        game.CurrentSoftware = software;
        game.LastSoftwares.Insert(0, software);

        games.ReplaceOne(g => g.UserToken == game.UserToken, game);

        game.CurrentSoftware.Description = null;
        game.CurrentSoftware.IsFriendli = false;
        game.LastSoftwares = new();

        return game;
    }

    private SoftwareModel GetRandomSoftware()
    {
        Random r = new Random();
        bool isFriendli = r.Next(0, 2) == 1;
        string fileName = "Resources/Raw/data_angri.json";
        if (isFriendli)
            fileName = "Resources/Raw/data_friendli.json";

        List<SoftwareModel> softwares = JsonSerializer
            .Deserialize<List<SoftwareModel>>(
                File.ReadAllText(fileName));

        SoftwareModel software = softwares[r.Next(0, softwares.Count - 1)];
        software.IsFriendli = isFriendli;

        return software;
    }

    public GameModel Guess(string userToken, bool isFriendli)
    {
        if (!IsValidToken(userToken))
            throw new MissingMemberException(
                "Nincs ilyen token-ű felhassználó!");

        GameModel game = this.games.AsQueryable()
            .SingleOrDefault(g => g.UserToken == userToken);

        if (game is null)
            throw new MissingMemberException
                ("Nincs játék létrehozva! Missing `CreateNewGame`?");

        bool isLate = game.Date < DateTime.Now.ToUniversalTime();
        bool isCorrect = game.CurrentSoftware.IsFriendli == isFriendli;

        if (isLate || !isCorrect)
            game.LivesLeft--;
        else
            game.Score++;

        game.CurrentSoftware = null;

        if (game.LivesLeft == 0)
        {
            UserRepository userRepository = new UserRepository();
            UserModel user = userRepository.GetUserByToken(userToken);
            ScoreModel score = new()
            {
                Score = game.Score,
                GameMode = game.GameMode,
                Date = DateTime.Now
            };
            user.Scores.Add(score);

            userRepository.Update(user);
            Delete(userToken);
        }

        this.games.ReplaceOne(g => g.UserToken == game.UserToken, game);
        return game;
    }

    private bool IsValidToken(string token) =>
        new UserRepository().GetUserByToken(token) != null;

    public void Delete(string token) =>
        this.games.DeleteOne(g => g.UserToken == token);
}
