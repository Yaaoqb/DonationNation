using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;

        public EventsController(ApplicationDbContext context, IS3Service s3Service, IMapper mapper)
        {
            _context = context;
            _s3Service = s3Service;
            _mapper = mapper;
        }

        // GET: Admin/Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Admin/Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Admin/Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location,DateTime,Description,Image,Id")] EventCreate eventCreate)
        {
            if (ModelState.IsValid)
            {
                var _event = _mapper.Map<Event>(eventCreate);

                _event.Image = await _s3Service.AddFile(eventCreate.Image, "events");

                _context.Add(_event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var vm = _mapper.Map<Event>(eventCreate);
            return View(vm);
        }

        // GET: Admin/Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<EventEdit>(@event);

            return View(vm);
        }

        // POST: Admin/Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Location,DateTime,Description,Image,Id")] EventEdit eventEdit)
        {
            if (id != eventEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _event = _mapper.Map<Event>(eventEdit);

                    //Check if viewmodel has image
                    if (eventEdit.ImageFile != null)
                    {
                        //If new image set new image
                        _event.Image = await _s3Service.AddFile(eventEdit.ImageFile, "events");
                    }
                    else //Set same image as before
                    {
                        _event.Image = (await _context.Events.AsNoTracking().Where(x => x.Id == eventEdit.Id).FirstOrDefaultAsync()).Image;
                    }

                    _context.Update(_event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventEdit.Id))
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
            return View(eventEdit);
        }

        // GET: Admin/Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Admin/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            //Delete file from S3
            await _s3Service.RemoveFile(@event.Image);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
