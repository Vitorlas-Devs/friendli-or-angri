using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace FriendliOrAngri.WebAPI.Data.Repositories;

public class UserRepository
{
    protected readonly MongoClient dbClient;

    private IMongoCollection<UserModel> users =>
        dbClient.GetDatabase("friendliOrAngri")
            .GetCollection<UserModel>("users");

    public UserRepository() =>
        this.dbClient = new("mongodb://localhost:27017");

    public IEnumerable<UserModel> GetAll()
    {
        List<UserModel> users = this.users.AsQueryable().ToList();
        foreach (UserModel user in users)
            yield return user;
    }

    public IEnumerable<UserScoreModel> GetLeaderboard(
        DateSort dateSort, GameMode gameMode)
    {
        List<UserScoreModel> leaderboard = new();

        foreach (UserModel user in GetAll())
        {
            int dateFrom = 0;
            switch (dateSort)
            {
                case DateSort.LastMonth:
                    dateFrom = -30;
                    break;
                case DateSort.LastWeek:
                    dateFrom = -7;
                    break;
                case DateSort.LastDay:
                    dateFrom = -1;
                    break;
            }
            int score = 0;

            if (user.Scores.Any())
                score = user.Scores
                    .Where(s =>
                    {
                        bool correctGameMode = s.GameMode == gameMode;
                        bool isTooOld = s.Date < DateTime.Now
                            .ToUniversalTime()
                            .AddDays(dateFrom);
                        return correctGameMode && (!isTooOld || dateFrom == 0);
                    })
                    .Max(s => s.Score);

            leaderboard.Add(new()
            {
                Name = user.Name,
                Id = user.Id,
                Score = score,
            });
        }

        return leaderboard.OrderByDescending(x => x.Score);
    }

    public int GetUserCount() =>
        GetAll().Count();

    public int GetUsersLeaderboardPosition(
        string userToken, DateSort dateSort, GameMode gameMode)
    {
        List<UserScoreModel> leaderboard =
            GetLeaderboard(dateSort, gameMode).ToList();
        UserModel user = GetUserByToken(userToken);

        if (user is null)
            throw new MissingMemberException("Nincs ilyen token-ű felhassználó!");

        return leaderboard.IndexOf(
            leaderboard.SingleOrDefault(u =>
                u.Name == user.Name &&
                u.Id == user.Id)) + 1;
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
