using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Testimonials
        public async Task<IActionResult> Index()
        {
            var testimonials = _context.Testimonials.Include(t => t.User);
            return View(await testimonials.ToListAsync());
        }

        // GET: Admin/Testimonials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Admin/Testimonials/Create
        public IActionResult Create()
        {
            var usersForSelectList = _context.ApplicationUsers.Select(u => new
            {
                Id = u.Id,
                DisplayName = u.FirstName + " " + u.LastName
            }).ToList();

            ViewData["UserId"] = new SelectList(usersForSelectList, "Id", "DisplayName");
            return View();
        }

        // POST: Admin/Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Message,Id")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var usersForSelectList = _context.ApplicationUsers.Select(u => new
            {
                Id = u.Id,
                DisplayName = u.FirstName + " " + u.LastName
            }).ToList();

            ViewData["UserId"] = new SelectList(usersForSelectList, "Id", "DisplayName");
            return View(testimonial);
        }

        // GET: Admin/Testimonials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            var usersForSelectList = _context.ApplicationUsers.Select(u => new
            {
                Id = u.Id,
                DisplayName = u.FirstName + " " + u.LastName
            }).ToList();

            ViewData["UserId"] = new SelectList(usersForSelectList, "Id", "DisplayName");
            return View(testimonial);
        }

        // POST: Admin/Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Message,Id")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var usersForSelectList = _context.ApplicationUsers.Select(u => new
            {
                Id = u.Id,
                DisplayName = u.FirstName + " " + u.LastName
            }).ToList();

            ViewData["UserId"] = new SelectList(usersForSelectList, "Id", "DisplayName");
            return View(testimonial);
        }

        // GET: Admin/Testimonials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Admin/Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(int id)
        {
            return _context.Testimonials.Any(e => e.Id == id);
        }
    }
}
