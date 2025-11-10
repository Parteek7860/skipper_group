using Microsoft.AspNetCore.Mvc;

namespace skipper_group_new.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Error/Handle/{statusCode}")]
        public IActionResult Handle(int statusCode)
        {
            Response.StatusCode = statusCode;

            return statusCode switch
            {
                404 => View("~/Views/Shared/Error.cshtml"),
                500 => View("~/Views/Shared/Error.cshtml"),
                _ => View("~/Views/Shared/Error.cshtml"),
            };
        }
    }
}
