using FriendliOrAngri.WebAPI.Data.Enums;
using FriendliOrAngri.WebAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FriendliOrAngri.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameRepository gameRepository;

        public GamesController() =>
            this.gameRepository = new();

        [HttpPost]
        public IActionResult CreateNewGame(string userToken, string gameMode) =>
            this.Run(() =>
            {
                GameMode validGameMode;

                switch (gameMode)
                {
                    case "normal":
                        validGameMode = GameMode.Normal;
                        break;
                    case "hardcore":
                        validGameMode = GameMode.Hardcore;
                        break;
                    default:
                        throw new ArgumentException("Ismeretlen játékmód!");
                }
                
                try
                {
                    this.gameRepository.CreateNewGame(userToken, validGameMode);
                    return Ok();
                }
                catch (MissingMemberException e)
                {
                    return StatusCode(404, e.Message);
                }
            });

        [HttpGet]
        public IActionResult GetSoftware(string userToken) =>
            this.Run(() =>
            {
                try
                {
                    return Ok(this.gameRepository.GetSoftware(userToken));
                }
                catch (MissingMemberException e)
                {
                    return StatusCode(404, e.Message);
                }
            });

        [HttpPut]
        public IActionResult Guess(string userToken, bool isFriendli) =>
            this.Run(() =>
            {
                try
                {
                    return Ok(this.gameRepository.Guess(userToken, isFriendli));
                }
                catch (MissingMemberException e)
                {
                    return StatusCode(404, e.Message);
                }
            });
    }
}
