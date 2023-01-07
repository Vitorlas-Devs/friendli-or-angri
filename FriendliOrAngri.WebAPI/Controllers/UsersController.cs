using FriendliOrAngri.Data.Models;
using FriendliOrAngri.Data.Repositories;
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
        public IActionResult GetAll()
        {
            return this.Run(() =>
                Ok(userRepository.GetAll())
            );
        }

        [HttpPost]
        public IActionResult Insert(UserModel model)
        {
            return this.Run(() =>
                Ok(userRepository.Insert(model))
            );
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return this.Run(() =>
            {
                userRepository.Delete(id);
                return Ok();
            });
        }
    }
}
