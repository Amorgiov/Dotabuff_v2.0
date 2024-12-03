
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DotaBuffClone.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DotaBuffClone.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var comments = _context.Comments.Where(c => c.UserId == user.Id).ToList();

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Comments = comments
            };

            return View(model);
        }
    }
}
