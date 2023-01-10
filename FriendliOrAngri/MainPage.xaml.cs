using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;
using System.Xml.Linq;
using FriendliOrAngri;
using FriendliOrAngri.WebAPI.Data.Models;
using CommunityToolkit.Maui.Alerts;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using FriendliOrAngri.Models;

namespace FriendliOrAngri;

public class PageModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public FontImageSource Icon { get; set; }
}
public partial class MainPage : ContentPage
{
    Database database = App.Database;
    AltUserModel User;

    public MainPage()
    {
        InitializeComponent();
        VerifyUserAsync();
    }

    public async void VerifyUserAsync()
    {
        HttpClient client = new();
        if (await database.GetUserAsync() == null)
        {
            string userName = await DisplayPromptAsync("Register", "What's your name?");
            var response = await client.PostAsync($"http://localhost:5124/api/Users?userName={userName}", null);
            string userString = await response.Content.ReadAsStringAsync();
            AltUserModel user = JsonSerializer.Deserialize<AltUserModel>(userString);
            await database.SaveUserAsync(user);
            User = user;
            await ShowToast();
        }
        else
        {
            User = database.GetUserAsync().Result;
            await ShowToast();
        }
    }

    public async Task ShowToast()
    {
        CancellationTokenSource cancellationTokenSource = new();
        string text = $"Logged in as {User.Name}#{User.Id}";
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;
        var toast = Toast.Make(text, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
}
