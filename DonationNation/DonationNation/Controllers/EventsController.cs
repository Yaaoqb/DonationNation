using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DonationNation.Communication.SMS;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Controllers
{
    [Authorize(Roles = "Donor")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISNSService _sNSService;

        public EventsController(ApplicationDbContext context, ISNSService sNSService)
        {
            _context = context;
            _sNSService = sNSService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _context.Events.Include(x => x.Donors ).Where(x => x.Donors .Any(x => x.UserName == User.Identity.Name)).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Enroll(int id)
        {
            var _event = await _context.Events.Include(x => x.Donors ).FirstOrDefaultAsync(a => a.Id == id);

            if (_event.Donors ?.Any(x => x.UserName == User.Identity.Name) ?? false)
            {
                TempData["EventError"] = true;
                return Redirect("/");
            }

            if (_event.Donors  == null)
            {
                _event.Donors  = new List<ApplicationUser>();
            }
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == User.Identity.Name);
            _event.Donors .Add(user);

            await _sNSService.SendSMS($"+{user.PhoneNumber}", $"You have successfully registered for the event {_event.Name}");

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
