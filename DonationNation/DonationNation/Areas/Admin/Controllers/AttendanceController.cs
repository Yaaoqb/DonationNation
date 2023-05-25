using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var _events = await _context.Events.Where(x => x.DateTime > DateTime.Now).ToListAsync();
            return View(_events);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> MarkAttendance(int id)
        {
            var _event = await _context.Events.Include(x => x.Donors ).FirstOrDefaultAsync(x => x.Id == id);
            var enrolledDonors  = _event.Donors ;
            foreach (var item in enrolledDonors )
            {
                if (!await _context.Attendances.AnyAsync(x => x.EventId == id && x.UserId == item.Id))
                {
                    var attendance = new Attendance();
                    attendance.EventId = _event.Id;
                    attendance.UserId = item.Id;
                    attendance.isPresent = false;
                    _context.Add(attendance);
                }
                await _context.SaveChangesAsync();
            }

            var attendances = await _context.Attendances.Include(x => x.Event).Include(x => x.User).Where(x => x.EventId == id).ToListAsync();

            return View(attendances);
        }

        [Route("/markAbsentOrPresent")]
        public async Task MarkAbsentOrPresent(int eventId, string userId)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(x => x.EventId == eventId && x.UserId == userId);
            attendance.isPresent = !attendance.isPresent;
            _context.Update(attendance);
            await _context.SaveChangesAsync();
        }
    }
}