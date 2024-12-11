using DotaBuffClone.Common.Interfaces;
using DotaBuffClone.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotaBuffClone.Controllers
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
