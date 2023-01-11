using FriendliOrAngri.Models;
using FriendliOrAngri.WebAPI.Data.Models;
using Java.Lang;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json;

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
        HeartsCreate(maxHearts);
    }
    
    public async Task GetSoftware()
    {
        //Response body: 
        //{
        //  "userToken": "87156aa6-5f57-65b5-4b78-190e47760266",
        //  "score": 0,
        //  "gameMode": 0,
        //  "livesLeft": 5,
        //  "date": "2023-01-10T22:09:13.6110568Z",
        //  "currentSoftware": {
        //    "name": "Gymnasium",
        //    "description": null,
        //    "isFriendli": false
        //  },
        //  "lastSoftwares": []
        //}

        using HttpClient client = new();

        var response = await client.GetStringAsync($"http://143.198.188.238/api/Games?userToken={User.Token}");
        Game = JsonConvert.DeserializeObject<GameModel>(response);
        lbSoftware.Text = Game.CurrentSoftware.Name;
    }

    public async Task Guess(bool isFriendly)
    {
        //Response body:
        //{
        //  "userToken": "87156aa6-5f57-65b5-4b78-190e47760266",
        //  "score": 0,
        //  "gameMode": 0,
        //  "livesLeft": 4,
        //  "date": "2023-01-10T22:09:13.611Z",
        //  "currentSoftware": null,
        //  "lastSoftwares": [
        //    {
        //      "name": "Gymnasium",
        //      "description": "Gymnasium is an open source Python library for developing and comparing reinforcement learning algorithms by providing a standard API. This is a fork of OpenAI's Gym.",
        //      "isFriendli": true
        //    }
        //  ]
        //}

        using HttpClient client = new();
        var response = await client.PutAsync($"http://143.198.188.238/api/Games?userToken={User.Token}&isFriendli={isFriendly}", null);
        string softwareString = await response.Content.ReadAsStringAsync();
        Game = JsonConvert.DeserializeObject<GameModel>(softwareString);
        Software = Game.LastSoftwares.Last();
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
            RefreshHearts(false, isFriendly);
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
        ResetHeartLevel();
        await GetSoftware();
        btnAngry.IsEnabled = true;
        btnFriendly.IsEnabled = true;
        btnAngry.Opacity = 1;
        btnFriendly.Opacity = 1;
    }

    private void HeartsCreate(int maxHeartsCount)
    {
        hslBlackHearts.Clear();
        for (int i = 0; i < maxHeartsCount; i++)
        {
            hslHearts.Children.Add(new Label() { Text = "❤️", FontSize = 20});
        }
    }

    private void RefreshHearts(bool isCorrect, bool isFriendly)
    {
        isCorrect = isFriendly == Software.IsFriendli;
        if (!isCorrect)
        {
            hslHearts.Children.RemoveAt(hslHearts.Children.Count - 1);
            hslBlackHearts.Children.Add(new Label() { Text = "🖤", FontSize = 20 });
        }

        if (Game.LivesLeft == 0)
        {
            lbHearts.Text = "Game over ";
            GameOver();
        }
        else
        {
            lbHearts.Text = "Life: ";
        }
    }

    private void GameOver()
    {
        btnNext.Text = "Continue";
        btnNext.IsVisible = true;
    }

    private void ResetHeartLevel()
    {
        if (Game.LivesLeft == 0)
        {
            HeartsCreate(maxHearts);
        }
    }
}
