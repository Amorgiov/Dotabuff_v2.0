using Microsoft.AspNetCore.Mvc;

namespace Dotabuff_2._0.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
