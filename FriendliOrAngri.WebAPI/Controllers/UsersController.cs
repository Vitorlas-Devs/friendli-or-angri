using FriendliOrAngri.WebAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FriendliOrAngri.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UsersController() =>
            this.userRepository = new();

        [HttpPost]
        public IActionResult Insert(string userName) =>
            this.Run(() =>
                Ok(userRepository.Insert(userName))
            );

        [HttpDelete]
        public IActionResult Delete(string userName, int id, string password) =>
            this.Run(() =>
            {
                if (System.IO.File.ReadAllText("app.pwd").Trim() != password)
                    return StatusCode(401);

                userRepository.Delete(userName, id);
                return Ok();
            });
    }
}
