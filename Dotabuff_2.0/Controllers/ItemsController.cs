using Dotabuff_2._0.Common.Interfaces;
using Dotabuff_2._0.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dotabuff_2._0.Controllers
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
            try
            {
                var items = await _parsingService.GetItemsAsync(date);
                return View(items);
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем пустой список
                Console.WriteLine($"Ошибка при загрузке предметов: {ex.Message}");
                return View(new List<Item>());
            }
        }

    }
}
