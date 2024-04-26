namespace SimpleChats.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
