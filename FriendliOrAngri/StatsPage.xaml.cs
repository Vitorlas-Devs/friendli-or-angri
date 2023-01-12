using FriendliOrAngri.Models;
using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Models;
using Newtonsoft.Json;

namespace FriendliOrAngri;

public class ExtendedUserScoreModel : UserScoreModel
{
    public int Index { get; set; }
}
public partial class StatsPage : ContentPage
{
    Database database = App.Database;
    public UserScoreModel[] leaderboard { get; set; }
    public List<ExtendedUserScoreModel> leaderboardEx { get; set; }
    public AltUserModel User;
    string dateSort = "lifeTime";
    string gameMode = "normal";

    public StatsPage()
    {
        //lbUser.BindingContext = User;
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
        try
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"http://143.198.188.238/api/Leaderboard?dateSort={dateSort}&gameMode={gameMode}");
            leaderboard = JsonConvert.DeserializeObject<UserScoreModel[]>(response);
            for (int i = 0; i < leaderboard.Length; i++)
            {
                leaderboardEx.Add(new ExtendedUserScoreModel
                {
                    Index = i + 1,
                    Name = leaderboard[i].Name,
                    Id = leaderboard[i].Id,
                    Score = leaderboard[i].Score,
                });
            }
            lbError.IsVisible = false;
            lbUser.IsVisible = true;
            lwLeaderboard.ItemsSource = leaderboardEx;
        }
        catch (Exception)
        {
            lbError.IsVisible = true;
            lbUser.IsVisible = false;
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
    }

    public async Task GetUserLeaderboardPosition()
    {
        try
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync($"http://143.198.188.238/api/Leaderboard/Position?userToken={User.Token}&dateSort={dateSort}&gameMode={gameMode}");
            var position = JsonConvert.DeserializeObject<int>(response);
            lbUserPosition.Text = position.ToString();
        }
        catch (Exception)
        {
            throw;
        }
    }

}