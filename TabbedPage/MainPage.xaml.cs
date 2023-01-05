using System.Collections.ObjectModel;
using System.Windows.Input;
using FriendliOrAngri;

namespace FriendliOrAngri;

public class PageModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public FontImageSource Icon { get; set; }
}
public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();

    }
}
