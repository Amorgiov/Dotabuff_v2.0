using Microsoft.AspNetCore.Mvc;

namespace Dotabuff_2._0.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
