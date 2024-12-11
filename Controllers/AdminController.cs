using DotaBuffClone.Services;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly DotabuffParsingService _parsingService;

    public AdminController(DotabuffParsingService parsingService)
    {
        _parsingService = parsingService;
    }

    public IActionResult Index()
    {
        return View();
    }

    /*[HttpPost]
    public async Task<IActionResult> UpdateItems()
    {
        await _parsingService.ParseAndSaveItemsAsync();
        return RedirectToAction("Index", "Items");
    }*/
}
