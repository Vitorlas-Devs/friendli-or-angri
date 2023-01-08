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

        [HttpGet]
        public IActionResult GetAll() =>
            this.Run(() =>
                Ok(userRepository.GetAll())
            );

        [HttpPost]
        public IActionResult Insert(string userName) =>
            this.Run(() =>
                Ok(userRepository.Insert(userName))
            );

        [HttpDelete]
        public IActionResult Delete(int id) =>
            this.Run(() =>
            {
                userRepository.Delete(id);
                return Ok();
            });
    }
}
