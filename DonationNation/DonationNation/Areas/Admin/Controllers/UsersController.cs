using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DonationNation.Areas.Admin.Models;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.Select(c => new UsersViewModel()
            {
                FullName = $"{c.FirstName} {c.LastName}",
                Email = c.Email,
                Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();

            return View(users);
        }


        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var rolesForSelectList = roleStore.Roles.Select(x => new
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList();

            ViewData["Roles"] = new SelectList(rolesForSelectList, "DisplayName", "DisplayName");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,PhoneNumber,Password,ConfirmPassword,Role")] UserCreate userCreate)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userCreate.Email, Email = userCreate.Email, FirstName = userCreate.FirstName, LastName = userCreate.LastName, PhoneNumber = userCreate.PhoneNumber };

                var result = await _userManager.CreateAsync(user, userCreate.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(userCreate.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _userManager.AddToRoleAsync(user, userCreate.Role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            var roleStore = new RoleStore<IdentityRole>(_context);
            var rolesForSelectList = roleStore.Roles.Select(x => new
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList();

            ViewData["Roles"] = new SelectList(rolesForSelectList, "DisplayName", "DisplayName");
            return View(userCreate);
        }

    }
}

