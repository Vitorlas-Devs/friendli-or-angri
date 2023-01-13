using FriendliOrAngri.Models;
using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using Newtonsoft.Json;

namespace FriendliOrAngri;

public partial class StatsPage : ContentPage
{
    Database database = App.Database;
    public UserScoreModel[] leaderboard { get; set; }
    public AltUserModel User;
    string dateSort = "lifeTime";
    string gameMode = "normal";

    public StatsPage()
    {
        InitializeComponent();
        InitStuff();
    }

    private async void InitStuff()
    {
        pickDateSort.SelectedIndex = 0; // erre 4 fajta megoldás létezik elv, de nekem csak így, kódban működik
        pickGameMode.SelectedIndex = 0; // ami nem baj csak hát wtf
        await GetUser();                // justified az InitStuff() jelenléte legalább, mert jól néz ki
        await GetLeaderboard();
        await GetUserLeaderboardPosition();
    }

    public async Task GetUser()
    {
        User = await database.GetUserAsync();
    }

    public async Task GetLeaderboard()
    {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"http://143.198.188.238/api/Leaderboard?dateSort={dateSort}&gameMode={gameMode}");
            leaderboard = JsonConvert.DeserializeObject<UserScoreModel[]>(response);
            var leaderboardWithIndex = leaderboard.Select((item, index) => new { item.Name, item.Id, item.Score, ListIndex = index + 1 + ". " }).ToList();
            leaderboardWithIndex.RemoveAll(x => x.Score == 0);
            lbError.IsVisible = false;
            slUser.IsVisible = true;
            lwLeaderboard.ItemsSource = leaderboardWithIndex;

        if (!leaderboardWithIndex.Any())
        {
            lbError.IsVisible = true;
            slUser.IsVisible = false;
            lwLeaderboard.ItemsSource = null;
        }
    }

    private async void pickGameMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (pickGameMode.SelectedIndex)
        {
            case 0:
                gameMode = "normal";
                break;
            case 1:
                gameMode = "hardcore";
                break;
        }
        await GetLeaderboard();
        await GetUserLeaderboardPosition();
    }

    private async void pickDateSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (pickDateSort.SelectedIndex)
        {
            case 0:
                dateSort = "lifeTime";
                break;
            case 1:
                dateSort = "lastMonth";
                break;
            case 2:
                dateSort = "lastWeek";
                break;
            case 3:
                dateSort = "lastDay";
                break;
        }
        await GetLeaderboard();
        await GetUserLeaderboardPosition();
    }

    public async Task GetUserLeaderboardPosition()
    {
        try
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"http://143.198.188.238/api/Leaderboard/Position?userToken={User.Token}&dateSort={dateSort}&gameMode={gameMode}");
            var position = JsonConvert.DeserializeObject<int>(response);
            lbUserPosition.Text = $"{position}.";
            lbUser.Text = $"{User.Name}#{User.Id}";
            var user = await client.GetStringAsync($"http://143.198.188.238/api/Users?token={User.Token}");
            UserModel altUser = JsonConvert.DeserializeObject<UserModel>(user);
            var altUserScore = 0;
            int dateFrom = 0;
            switch (dateSort)
            {
                case "lastMonth":
                    dateFrom = -30;
                    break;
                case "lastWeek":
                    dateFrom = -7;
                    break;
                case "lastDay":
                    dateFrom = -1;
                    break;
            }

            var scores = altUser.Scores
                    .Where(s =>
                    {
                        bool correctGameMode = s.GameMode.ToString().ToLower() == gameMode;
                        bool isTooOld = s.Date < DateTime.Now
                            .ToUniversalTime()
                            .AddDays(dateFrom);
                        bool filter = correctGameMode && (!isTooOld || dateFrom == 0);
                        return filter;
                    });
            if (scores.Any())
            {
                altUserScore = scores.Max(x => x.Score);
            }
            lbUserScore.Text = $"{altUserScore}";
        }
        catch (Exception)
        {
            throw;
        }
    }

}