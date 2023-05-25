using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DonationNation.Data;
using DonationNation.Models;

namespace DonationNation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel();
            vm.Events = await _context.Events.ToListAsync();
            vm.Testimonials = await _context.Testimonials.Include(x => x.User).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> MyBadges()
        {
            var user = await _context.ApplicationUsers.Include(x => x.Badges).FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var badges = (dynamic)null;
            if (user.Badges.Count > 0)
            {
                badges = user.Badges;
            }
            return View(badges);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
