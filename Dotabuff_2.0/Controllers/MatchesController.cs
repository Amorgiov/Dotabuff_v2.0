using Dotabuff_2._0.Common.Interfaces;
using Dotabuff_2._0.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotabuff_2._0.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IDotabuffParsingService _parsingService;
        private ApplicationDbContext _context { get; set; }

        public MatchesController(IDotabuffParsingService parsingService, ApplicationDbContext context)
        {
            _parsingService = parsingService;
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            await _parsingService.ParseAndSaveMatchesAsync(page);
            var matches = await _context.Matches.ToListAsync();
            ViewBag.CurrentPage = page;
            return View(matches);
        }
    }
}
