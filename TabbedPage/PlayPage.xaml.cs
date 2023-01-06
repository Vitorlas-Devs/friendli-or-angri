using System.Reflection;
using System.Text.Json;

namespace FriendliOrAngri;

public class Software
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public partial class PlayPage : ContentPage
{
    public string RandomSoftware { get; set; }

    public PlayPage()
    {
        InitializeComponent();
        RandomSoftware = ChooseRandomSoftwareAsync().Result;
        lbSoftware.Text = RandomSoftware;
    }
    public static async Task<string> ChooseRandomSoftwareAsync()
    {
        Random random = new();
        bool isFriendli = random.Next(2) == 1;
        string fileName = isFriendli ? "data_friendli.json" : "data_angri.json";
        var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        string text = "";
        using (var reader = new StreamReader(stream))
        {
            text = reader.ReadToEnd();
        }
        List<Software> softwareList = JsonSerializer.Deserialize<List<Software>>(text);
        int randomIndex = random.Next(softwareList.Count);
        return softwareList[randomIndex].Name;
    }
}
