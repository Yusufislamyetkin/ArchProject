using Microsoft.AspNetCore.Mvc;

namespace Arch.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
