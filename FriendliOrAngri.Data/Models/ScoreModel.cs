using FriendliOrAngri.WebAPI.Data.Enums;

namespace FriendliOrAngri.WebAPI.Data.Models;

public class ScoreModel
{
    public int Score { get; set; }
    public GameMode GameMode { get; set; }
    public DateTime Date { get; set; }
}
