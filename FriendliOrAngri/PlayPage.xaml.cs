using CommunityToolkit.Maui.Views;
using FriendliOrAngri.Models;
using FriendliOrAngri.WebAPI.Data.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace FriendliOrAngri;

public partial class PlayPage : ContentPage
{
    Database database = App.Database;
    AltUserModel User;

    public SoftwareModel Software;
    public GameModel Game;

    readonly int maxHearts = 5;

    public PlayPage()
    {
        InitializeComponent();
        InitStuff();
    }

    private async void InitStuff()
    {
        await GetUser();
        await CreateNewGame();
        await GetSoftware();
    }

    public async Task GetUser()
    {
        User = await database.GetUserAsync();
    }
    public async Task CreateNewGame()
    {
        using HttpClient client = new();
        await client.PostAsync($"http://143.198.188.238/api/Games?userToken={User.Token}&gameMode=normal", null);
        CreateHearts(maxHearts);
    }
    
    public async Task GetSoftware()
    {
        using HttpClient client = new();
        var response = await client.GetStringAsync($"http://143.198.188.238/api/Games?userToken={User.Token}");
        Game = JsonConvert.DeserializeObject<GameModel>(response);
        lbSoftware.Text = Game.CurrentSoftware.Name;
    }

    public async Task Guess(bool isFriendly)
    {
        using HttpClient client = new();
        var response = await client.PutAsync($"http://143.198.188.238/api/Games?userToken={User.Token}&isFriendli={isFriendly}", null);
        string softwareString = await response.Content.ReadAsStringAsync();
        Game = JsonConvert.DeserializeObject<GameModel>(softwareString);
        Software = Game.LastSoftwares.First();
        ShowResult(isFriendly);
    }
    
    private async void btnAngry_Clicked(object sender, EventArgs e)
    {
        await Guess(false);
    }

    private async void btnFriendly_Clicked(object sender, EventArgs e)
    {
        await Guess(true);
    }

    private void ShowResult(bool isFriendly)
    {
        if (Software.IsFriendli == isFriendly)
        {
            lbResult.Text = "Correct!";
        }
        else
        {
            lbResult.Text = "Nope!";
            RefreshHearts(isFriendly);
        }
        if (Software.IsFriendli)
        {
            lbSoftware.TextColor = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["FriendliColor"];
            lbSoftware.Text = $"😇 {lbSoftware.Text}";
        }
        else
        {
            lbSoftware.TextColor = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["AngriColor"];
            lbSoftware.Text = $"😠 {lbSoftware.Text}";
        }
        lbDescription.Text = Software.Description;
        btnNext.IsVisible = true;
        btnAngry.IsEnabled = false;
        btnFriendly.IsEnabled = false;
        btnAngry.Opacity = 0.7;
        btnFriendly.Opacity = 0.7;
    }


    private async void btnNext_Clicked(object sender, EventArgs e)
    {
        lbDescription.Text = "";
        lbResult.Text = "";
        lbSoftware.TextColor = Colors.Black;
        btnNext.IsVisible = false;
        btnNext.Text = "Go Next";

        if (Game.LivesLeft == 0)
        {
            await CreateNewGame();
        }

        await GetSoftware();
        btnAngry.IsEnabled = true;
        btnFriendly.IsEnabled = true;
        btnAngry.Opacity = 1;
        btnFriendly.Opacity = 1;
    }

    private void CreateHearts(int maxHeartsCount)
    {
        hslBlackHearts.Clear();
        for (int i = 0; i < maxHeartsCount; i++)
        {
            hslHearts.Children.Add(new Label() { Text = "❤️", FontSize = 25});
        }
    }

    private void RefreshHearts(bool isFriendly)
    {
        bool isCorrect = isFriendly == Software.IsFriendli;
        if (!isCorrect)
        {
            hslHearts.Children.RemoveAt(hslHearts.Children.Count - 1);
            hslBlackHearts.Children.Add(new Label() { Text = "🖤", FontSize = 25 });
        }

        if (Game.LivesLeft == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        btnNext.Text = "Continue";
        btnNext.IsVisible = true;
        this.ShowPopup(new GameOverPopUp());
    }
}
