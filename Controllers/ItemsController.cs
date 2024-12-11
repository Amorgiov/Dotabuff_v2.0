using DotaBuffClone.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotaBuffClone.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IDotabuffParsingService _parsingService;

        public ItemsController(IDotabuffParsingService parsingService)
        {
            _parsingService = parsingService;
        }

        public async Task<IActionResult> Index(string date = "all")
        {
            var items = await _parsingService.GetItemsAsync(date);
            return View(items);
        }

    }
}
