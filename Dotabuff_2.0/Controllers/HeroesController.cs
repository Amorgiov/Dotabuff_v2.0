using Dotabuff_2._0.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dotabuff_2._0.Controllers
{
    public class HeroesController : Controller
    {
        private readonly IDotabuffParsingService _parsingService;

        public HeroesController(IDotabuffParsingService parsingService)
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
