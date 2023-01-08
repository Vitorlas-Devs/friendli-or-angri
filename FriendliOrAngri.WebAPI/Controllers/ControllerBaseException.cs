using Microsoft.AspNetCore.Mvc;

namespace FriendliOrAngri.WebAPI.Controllers
{
    public static class ControllerBaseException
    {
        public static IActionResult Run(this ControllerBase controller, Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                return controller.StatusCode(501, new
                {
                    error = ex.Message
                });
            }
        }
    }
}
