using Microsoft.AspNetCore.Mvc;

namespace EverMediaDiplom.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}