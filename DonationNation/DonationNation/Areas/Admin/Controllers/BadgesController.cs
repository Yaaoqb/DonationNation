using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DonationNation.Areas.Admin.Models;
using DonationNation.Data;
using DonationNation.Data.Models;
using DonationNation.S3;

namespace DonationNation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BadgesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BadgesController(ApplicationDbContext context, IS3Service s3Service, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _s3Service = s3Service;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Admin/Badges
        public async Task<IActionResult> Index()
        {
            return View(await _context.Badges.ToListAsync());
        }

        // GET: Admin/Badges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var badge = await _context.Badges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (badge == null)
            {
                return NotFound();
            }

            return View(badge);
        }

        // GET: Admin/Badges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Badges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Image,Name,Id")] BadgeCreate badgeCreate)
        {
            if (ModelState.IsValid)
            {
                var _badge = _mapper.Map<Badge>(badgeCreate);
                _badge.Image = await _s3Service.AddFile(badgeCreate.Image, "badges");

                _context.Add(_badge);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var vm = _mapper.Map<Badge>(badgeCreate);
            return View(vm);
        }

        // GET: Admin/Badges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var badge = await _context.Badges.FindAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<BadgeEdit>(badge);

            return View(vm);
        }

        // POST: Admin/Badges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Image,Name,Id")] BadgeEdit badgeEdit)
        {
            if (id != badgeEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _badge = _mapper.Map<Badge>(badgeEdit);

                    //Check if viewmodel has image
                    if (badgeEdit.ImageFile != null)
                    {
                        //If new image set new image
                        _badge.Image = await _s3Service.AddFile(badgeEdit.ImageFile, "badges");
                    }
                    else //Set same image as before
                    {
                        _badge.Image = (await _context.Events.AsNoTracking().Where(x => x.Id == badgeEdit.Id).FirstOrDefaultAsync()).Image;
                    }

                    _context.Update(badgeEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BadgeExists(badgeEdit.Id))
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
            return View(badgeEdit);
        }

        // GET: Admin/Badges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var badge = await _context.Badges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (badge == null)
            {
                return NotFound();
            }

            return View(badge);
        }

        // POST: Admin/Badges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            await _s3Service.RemoveFile(badge.Image);
            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BadgeExists(int id)
        {
            return _context.Badges.Any(e => e.Id == id);
        }


        // GET: Admin/Badges/Edit/5
        public async Task<IActionResult> GiveBadges()
        {
            var users = await _userManager.GetUsersInRoleAsync("Donor");
            var usersForSelectList = users.Select(u => new
            {
                Id = u.Id,
                DisplayName = u.FirstName + " " + u.LastName
            }).ToList();

            ViewData["UserId"] = new SelectList(usersForSelectList, "Id", "DisplayName");
            ViewData["Badges"] = new SelectList(await _context.Badges.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GiveBadges([Bind("BadgeId,UserId")] GiveBadge giveBadge)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.ApplicationUsers.Include(x => x.Badges).FirstOrDefaultAsync(x => x.Id == giveBadge.UserId);
                if (user.Badges == null)
                {
                    user.Badges = new List<Badge>();
                }

                if (!user.Badges.Any(x => x.Id == giveBadge.BadgeId))
                {
                    user.Badges.Add(await _context.Badges.FirstOrDefaultAsync(x => x.Id == giveBadge.BadgeId));
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["BadgeError"] = false;
                }
                else
                {
                    TempData["BadgeError"] = true;
                }
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View(giveBadge);
        }
    }
}
