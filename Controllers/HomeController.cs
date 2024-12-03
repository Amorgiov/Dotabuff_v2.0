
using Microsoft.AspNetCore.Mvc;

namespace DotaBuffClone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
