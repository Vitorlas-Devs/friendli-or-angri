using System.Reflection;
using System.Text.Json;

namespace FriendliOrAngri;

public class Software
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsFriendly { get; set; }
}

public partial class PlayPage : ContentPage
{
    public Software Software;

    int hearts;


    public PlayPage()
    {
        InitializeComponent();
        ChooseRandomSoftwareAsync();
        HeartsCreate(5);
        RefreshHearts(true);
    }
    public async void ChooseRandomSoftwareAsync()
    {
        Random random = new();
        bool isFriendly = random.Next(2) == 1;
        string fileName = isFriendly ? "data_friendli.json" : "data_angri.json";
        var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        string text = "";
        using (var reader = new StreamReader(stream))
        {
            text = reader.ReadToEnd();
        }
        List<Software> softwareList = JsonSerializer.Deserialize<List<Software>>(text);
        int randomIndex = random.Next(softwareList.Count);
        Software = softwareList[randomIndex];
        lbSoftware.Text = Software.Name;
        Software.IsFriendly = isFriendly;
    }

    private void btnAngry_Clicked(object sender, EventArgs e)
    {
        ShowResult(false);
    }

    private void btnFriendly_Clicked(object sender, EventArgs e)
    {
        ShowResult(true);
    }

    private void ShowResult(bool isFriendly)
    {
        if (Software.IsFriendly == isFriendly)
        {
            lbResult.Text = "Correct!";
        }
        else
        {
            lbResult.Text = "Nope!";
            RefreshHearts(false);
        }
        if (Software.IsFriendly)
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


    private void btnNext_Clicked(object sender, EventArgs e)
    {
        lbDescription.Text = "";
        lbResult.Text = "";
        lbSoftware.TextColor = Colors.Black;
        btnNext.IsVisible = false;
        btnNext.Text = "Go Next";
        ResetHeartLevel();
        ChooseRandomSoftwareAsync();
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
        hearts = maxHeartsCount;
    }

    private void RefreshHearts(bool isCorrect)
    {
        if (!isCorrect)
        {
            hearts--;
            hslHearts.Children.RemoveAt(hslHearts.Children.Count - 1);
            hslBlackHearts.Children.Add(new Label() { Text = "🖤", FontSize = 20 });

        }

        if (hearts > 0)
        {
            lbHearts.Text = $"Life: {hearts} - ";
        }
        else
        {
            lbHearts.Text = "Game over";
            GameOver();
        }
        

    }

    private void GameOver()
    {
        btnNext.Text = "Continue";
        btnNext.IsVisible = true;

    }

    private void ResetHeartLevel()
    {
        if (hearts == 0)
        {
            hearts = 5;
            HeartsCreate(5);
        }
        RefreshHearts(true);
        
    }
}
