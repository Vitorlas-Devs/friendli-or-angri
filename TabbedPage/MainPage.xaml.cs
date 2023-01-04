using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FriendliOrAngri;

public class PageModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}
public partial class MainPage : ContentPage
{
    public ObservableCollection<PageModel> Pages { get; set; }
    public MainPage()
    {
        InitializeComponent();
        Pages = new ObservableCollection<PageModel>
        {
            new PageModel { Title = "Home", Description = "You are here. Hi." },
            new PageModel { Title = "Play", Description = "Start playing with 3 hearts.\nA random software name will be presented to you, and you have to guess if it is friendly or angry." },
            new PageModel { Title = "Free Mode", Description = "An endless mode where you can play without hearts.\nYou can play as long as you want, but you will not be able to see your score." },
            new PageModel { Title = "History", Description = "See your history of games played.\nYou can see your score, the software name, and if you guessed correctly." },
            new PageModel { Title = "Stats", Description = "See your stats.\nYou can see your total score, your total games played, and your total games won." },
        };
    }
}

