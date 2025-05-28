using Microsoft.AspNetCore.Mvc;

namespace EverMediaDiplom.Controllers.Test
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}