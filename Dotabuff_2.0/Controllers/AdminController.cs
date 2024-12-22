using Dotabuff_2._0.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotabuff_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDotabuffParsingService _parsingService;

        public AdminController(IDotabuffParsingService parsingService)
        {
            _parsingService = parsingService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHeroes()
        {
            await _parsingService.ParseAndSaveHeroesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItems()
        {
            await _parsingService.ParseAndSaveItemsAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAll()
        {
            await _parsingService.ParseAndSaveAllAsync();
            return RedirectToAction("Index");
        }
    }
}
