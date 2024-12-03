
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DotaBuffClone.Controllers
{
    public class MatchesController : Controller
    {
        public IActionResult Index()
        {
            // Placeholder for match data
            var matches = new List<object> {
                new { Id = 1, Team1 = "Radiant", Team2 = "Dire", Score1 = 35, Score2 = 40 },
                new { Id = 2, Team1 = "Radiant", Team2 = "Dire", Score1 = 50, Score2 = 45 }
            };
            return View(matches);
        }
    }
}
