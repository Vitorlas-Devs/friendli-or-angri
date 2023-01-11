using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendliOrAngri.ViewModels
{
    public class PagesListViewModel
    {
        public ObservableCollection<PageModel> Pages { get; set; }

        public PagesListViewModel()
        {
            Pages = new ObservableCollection<PageModel>
            {
                new PageModel { Title = "Home", Description = "You are here. Hi.", Icon = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.GlobeAmericas, Color = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["Primary"] } }, // 😂😂😂😂😂
                new PageModel { Title = "Play", Description = "Start playing with 5 hearts. A random software name will be presented to you, and you have to guess if it is friendly or angry.", Icon = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.Gamepad, Color = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["Primary"] } },
                new PageModel { Title = "Free Mode", Description = "An endless mode where you can play without hearts. You can play as long as you want, but you will not be able to see your score.", Icon = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.Star, Color = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["Primary"] } },
                new PageModel { Title = "History", Description = "See your history of games played.\nYou can see your score, the software name, and if you guessed correctly.", Icon = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.History, Color = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["Primary"] } },
                new PageModel { Title = "Stats", Description = "See your stats.\nYou can see your total score, your total games played, and your accuracy over time.", Icon = new FontImageSource { FontFamily = "FontAwesome", Glyph = IconFont.ThList, Color = (Color)Application.Current.Resources.MergedDictionaries.ToList()[0]["Primary"] } },
            };
        }
    }
}
