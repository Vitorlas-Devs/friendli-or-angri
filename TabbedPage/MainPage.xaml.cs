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


        Img.Source = new FontImageSource
        {
            Glyph = "\uf57d",
            FontFamily = "FontAwesome",
            Size = 44,
            Color = Colors.Black
        };

    }
}
