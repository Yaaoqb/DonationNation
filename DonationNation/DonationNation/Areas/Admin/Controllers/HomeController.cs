using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DonationNation.Areas.Admin.Models;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel();
            vm.BadgesCount = (await _context.Badges.CountAsync()).ToString();
            vm.UsersCount = (await _userManager.Users.CountAsync()).ToString();
            vm.Eventscount = (await _context.Events.CountAsync()).ToString();
            vm.Activities = await _context.UserActivities.OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync();
            return View(vm);
        }
    }
}
