namespace FriendliOrAngri.Data.Models;

public class AnswerModel
{
    public int Id { get; set; }
    public UserModel User { get; set; }
    public DateTime Date { get; set; }
    public bool Correct { get; set; }
}
