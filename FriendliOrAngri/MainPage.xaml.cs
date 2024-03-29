﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using FriendliOrAngri;
using CommunityToolkit.Maui.Alerts;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using FriendliOrAngri.Models;
using Newtonsoft.Json;
using FriendliOrAngri.WebAPI.Data.Models;

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
            string userName = await App.Current.MainPage.DisplayPromptAsync("Register", "What's your name?");
            if (userName == null || userName.Length < 3 || userName.Length > 25)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Your name must be between 3 and 25 characters long.", "OK");
                VerifyUserAsync();
            }
            else
            {
                var response = await client.PostAsync($"http://143.198.188.238/api/Users?userName={userName}", null);
                string userString = await response.Content.ReadAsStringAsync();
                AltUserModel user = JsonConvert.DeserializeObject<AltUserModel>(userString);
                await database.SaveUserAsync(user);
                User = user;
                await ShowToast();
            }
        }
        else
        {
            User = await database.GetUserAsync();
            var userData = await client.GetStringAsync($"http://143.198.188.238/api/Users?token={User.Token}");
            try
            {
                UserModel altUser = JsonConvert.DeserializeObject<UserModel>(userData);
                await ShowToast();
            }
            catch (Exception)
            {
                await database.DeleteUserAsync(User);
                VerifyUserAsync();
            }
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
