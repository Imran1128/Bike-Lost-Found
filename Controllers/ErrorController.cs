using Microsoft.AspNetCore.Mvc;

namespace BikeLostAndFound.Controllers
{
    public class ErrorController :Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
