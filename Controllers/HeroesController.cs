using Microsoft.AspNetCore.Mvc;
using DotaBuffClone.Services;
using System.Threading.Tasks;

namespace DotaBuffClone.Controllers
{
    public class HeroesController : Controller
    {
        private readonly DotabuffParsingService _parsingService;

        public HeroesController(DotabuffParsingService parsingService)
        {
            _parsingService = parsingService;
        }

        public async Task<IActionResult> Index()
        {
            var heroes = await _parsingService.GetHeroesAsync();
            return View(heroes);
        }

    }
}
