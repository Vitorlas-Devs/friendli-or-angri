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

namespace FriendliOrAngri;

public class PageModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public FontImageSource Icon { get; set; }
}
public partial class MainPage : ContentPage
{
    Database database;
    UserModel User;

    public MainPage(Database database)
    {
        InitializeComponent();
        this.database = database;
        VerifyUserAsync();
    }

    private async void VerifyUserAsync()
    {
        HttpClient client = new();
        if (database.GetItemsAsync() == null)
        {
            string result = await DisplayPromptAsync("Register", "What's your name?");
            var response = await client.PostAsync($"http://localhost:5124/api/Users?userName={result}", null);
            string userString = await response.Content.ReadAsStringAsync();
            UserModel user = JsonSerializer.Deserialize<UserModel>(userString);
            await database.SaveItemAsync(user);
            User = user;
        }
        else
        {
            User = database.GetItemsAsync().Result.FirstOrDefault();
            CancellationTokenSource cancellationTokenSource = new();
            string text = $"Logged in as {User.Name}#{User.Id}";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }



}
