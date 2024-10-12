using Microsoft.AspNetCore.Mvc;

namespace LoginLogout.Controllers
{
    public class CustomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
