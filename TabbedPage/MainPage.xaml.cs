using System.Collections.ObjectModel;
using System.Windows.Input;
using FriendliOrAngri;

namespace FriendliOrAngri;

public class PageModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Image Icon { get; set; }
}
public partial class MainPage : ContentPage
{
    public ObservableCollection<PageModel> Pages { get; set; }
    public MainPage()
    {
        InitializeComponent();
        Pages = new ObservableCollection<PageModel>
        {
            new PageModel { Title = "Home", Description = "You are here. Hi.", Icon = new Image { Source = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.GlobeAmericas, Color = Colors.White } } },
            new PageModel { Title = "Play", Description = "Start playing with 3 hearts.\nA random software name will be presented to you, and you have to guess if it is friendly or angry.", Icon = new Image { Source = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.Gamepad, Color = Colors.White } } },
            new PageModel { Title = "Free Mode", Description = "An endless mode where you can play without hearts.\nYou can play as long as you want, but you will not be able to see your score.", Icon = new Image { Source = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.Star, Color = Colors.White } } },
            new PageModel { Title = "History", Description = "See your history of games played.\nYou can see your score, the software name, and if you guessed correctly.", Icon = new Image { Source = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.History, Color = Colors.White } } },
            new PageModel { Title = "Stats", Description = "See your stats.\nYou can see your total score, your total games played, and your total games won.", Icon = new Image { Source = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.ThList, Color = Colors.White } } },
        };
    }
}

