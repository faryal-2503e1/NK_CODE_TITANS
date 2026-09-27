using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private const int PageSize = 10;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly FanHubContext _context;

        public AdminUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, FanHubContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: AdminUser
        public async Task<IActionResult> Index(string? q, string? role, string? status, int page = 1)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(u => (u.UserName != null && u.UserName.Contains(q))
                    || (u.Email != null && u.Email.Contains(q))
                    || (u.FullName != null && u.FullName.Contains(q)));
            }

            if (!string.IsNullOrEmpty(role))
            {
                var roleId = await _roleManager.Roles.Where(r => r.Name == role).Select(r => r.Id).FirstOrDefaultAsync();
                query = query.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId));
            }

            var now = DateTimeOffset.UtcNow;
            if (status == "blocked") query = query.Where(u => u.LockoutEnd != null && u.LockoutEnd > now);
            else if (status == "active") query = query.Where(u => u.LockoutEnd == null || u.LockoutEnd <= now);

            var users = await PagedList<ApplicationUser>.CreateAsync(query.OrderBy(u => u.UserName), page, PageSize);

            // Roles for the users on this page, in one query
            var ids = users.Select(u => u.Id).ToList();
            var roleRows = await (from ur in _context.UserRoles
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  where ids.Contains(ur.UserId)
                                  select new { ur.UserId, r.Name }).ToListAsync();
            ViewBag.Roles = roleRows
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Name ?? "").ToList());

            ViewBag.RoleNames = await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync();
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            return View(users);
        }

        // GET: AdminUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = await _userManager.GetRolesAsync(user);
            ViewBag.IsBlocked = await _userManager.IsLockedOutAsync(user);
            ViewBag.IsSelf = user.Id == _userManager.GetUserId(User);
            ViewBag.BookmarkCount = await _context.Bookmarks.CountAsync(b => b.UserId == id);
            ViewBag.FeedbackCount = await _context.Feedbacks.CountAsync(f => f.UserId == id);
            ViewBag.SubmissionCount = await _context.FanSubmissions.CountAsync(s => s.UserId == id);
            ViewBag.ChatbotCount = await _context.ChatbotQueries.CountAsync(c => c.UserId == id);

            return View(user);
        }

        // GET: AdminUser/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: AdminUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,FullName,Address,ProfilePictureUrl")] ApplicationUser model)
        {
            if (id != model.Id) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.ProfilePictureUrl = model.ProfilePictureUrl;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"{user.UserName} was updated.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // POST: AdminUser/Block/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["ErrorMessage"] = "You cannot block your own account.";
                return RedirectToAction(nameof(Index));
            }

            // Lockout to the maximum date is the built-in way to block a sign-in; the stamp update ends any open session
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateSecurityStampAsync(user);

            TempData["SuccessMessage"] = $"{user.UserName} is now blocked.";
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminUser/Unblock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.ResetAccessFailedCountAsync(user);

            TempData["SuccessMessage"] = $"{user.UserName} can sign in again.";
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminUser/ManageRoles/5
        public async Task<IActionResult> ManageRoles(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = allRoles.Select(role => new UserRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name)
            }).ToList();

            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;

            return View(model);
        }

        // POST: AdminUser/ManageRoles/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(string userId, List<UserRoleViewModel> model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = model.Where(x => x.IsSelected && !userRoles.Contains(x.RoleName)).Select(x => x.RoleName).ToList();
            var rolesToRemove = model.Where(x => !x.IsSelected && userRoles.Contains(x.RoleName)).Select(x => x.RoleName).ToList();

            // Guard: an admin must not be able to lock the admin panel by removing Admin
            // from themselves, and the last remaining Admin can never be demoted.
            if (rolesToRemove.Contains("Admin"))
            {
                if (user.Id == _userManager.GetUserId(User))
                {
                    TempData["ErrorMessage"] = "You cannot remove the Admin role from your own account.";
                    return RedirectToAction(nameof(ManageRoles), new { id = userId });
                }

                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                if (admins.Count <= 1)
                {
                    TempData["ErrorMessage"] = "At least one Admin account must remain.";
                    return RedirectToAction(nameof(ManageRoles), new { id = userId });
                }
            }

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            TempData["SuccessMessage"] = $"Roles updated for {user.UserName}.";
            return RedirectToAction(nameof(Index));
        }
    }

    public class UserRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
