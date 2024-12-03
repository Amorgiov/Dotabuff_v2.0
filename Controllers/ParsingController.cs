
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DotaBuffClone.Services;

namespace DotaBuffClone.Controllers
{
    public class ParsingController : Controller
    {
        private readonly DotabuffParsingService _parsingService;

        public ParsingController(DotabuffParsingService parsingService)
        {
            _parsingService = parsingService;
        }

        public async Task<IActionResult> Heroes()
        {
            var heroes = await _parsingService.GetHeroesAsync();
            return View(heroes);
        }

        public async Task<IActionResult> Items()
        {
            var items = await _parsingService.GetItemsAsync();
            return View(items);
        }
    }
}
