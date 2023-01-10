using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using FriendliOrAngri;
using FriendliOrAngri.WebAPI.Data.Models;
using CommunityToolkit.Maui.Alerts;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using FriendliOrAngri.Models;
using Newtonsoft.Json;

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
        //ShowDialogAsync();
    }

    private async void ShowDialogAsync()
    {

        string result = await App.Current.MainPage.DisplayPromptAsync("Question 1", "What's your name?");

        // OK
        if (result != null)
        {
            await DisplayAlert("Alert", "User created!", "OK");
        }
        else
        {
            await DisplayAlert("Alert", "User not created!", "OK");
        }
    }

    public async void VerifyUserAsync()
    {
        HttpClient client = new();
        if (await database.GetUserAsync() == null)
        {
            string userName = await App.Current.MainPage.DisplayPromptAsync("Register", "What's your name?");
            var response = await client.PostAsync($"http://localhost:5124/api/Users?userName={userName}", null); //ÍRD ÁT SAJÁT IP-RE!!!!!!!!!
            string userString = await response.Content.ReadAsStringAsync();
            AltUserModel user = JsonConvert.DeserializeObject<AltUserModel>(userString);
            await database.SaveUserAsync(user);
            User = user;
            await ShowToast();
        }
        else
        {
            User = await database.GetUserAsync();
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
