using FriendliOrAngri.Data.Models;
using FriendliOrAngri.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FriendliOrAngri.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly AnswerRepository answerRepository;

        public AnswersController() =>
            this.answerRepository = new();

        [HttpGet]
        public IActionResult GetAll() => 
            this.Run(() =>
                Ok(answerRepository.GetAll())
            );

        [HttpPost]
        public IActionResult Insert(AnswerModel model) =>
            this.Run(() =>
                Ok(answerRepository.Insert(model))
            );

        [HttpDelete]
        public IActionResult Delete(int id) =>
            this.Run(() =>
            {
                answerRepository.Delete(id);
                return Ok();
            });
    }
}
