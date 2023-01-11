using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FriendliOrAngri.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly UserRepository userRepository;

    public LeaderboardController() =>
        this.userRepository = new();

    [HttpGet]
    public IActionResult GetLeaderboard(string dateSort, string gameMode) =>
        this.Run(() =>
            Ok(this.userRepository.GetLeaderboard(
                ValidateDateSort(dateSort),
                ValidateGameMode(gameMode)))
        );

    [HttpGet]
    [Route("Position")]
    public IActionResult GetUsersLeaderboardPosition(
        string userToken, string dateSort, string gameMode) =>
        this.Run(() =>
        {
            try
            {
                return Ok(this.userRepository.GetUsersLeaderboardPosition(
                    userToken,
                    ValidateDateSort(dateSort),
                    ValidateGameMode(gameMode)));
            }
            catch (MissingMemberException e)
            {
                return StatusCode(404, e.Message);
            }
        }
        );

    private DateSort ValidateDateSort(string dateSort)
    {
        switch (dateSort)
        {
            case "lifeTime":
                return DateSort.LifeTime;
            case "lastMonth":
                return DateSort.LastMonth;
            case "lastWeek":
                return DateSort.LastWeek;
            case "lastDay":
                return DateSort.LastDay;
            default:
                throw new ArgumentException("Ismeretlen dátum szűrés!");
        }
    }

    private GameMode ValidateGameMode(string gameMode)
    {
        switch (gameMode)
        {
            case "normal":
                return GameMode.Normal;
            case "hardcore":
                return GameMode.Hardcore;
            default:
                throw new ArgumentException("Ismeretlen játékmód!");
        }
    }
}
