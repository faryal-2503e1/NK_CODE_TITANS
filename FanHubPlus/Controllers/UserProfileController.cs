using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FanHubContext _context;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public UserProfileController(UserManager<ApplicationUser> userManager, FanHubContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }

        [Route("/profile")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            return View(currentUser);
        }

        [Route("/profile/edit")]
        public async Task<IActionResult> Edit()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileEditViewModel
            {
                FullName = currentUser.FullName,
                Address = currentUser.Address,
                CurrentProfilePictureUrl = currentUser.ProfilePictureUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("/profile/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel updatedProfile, IFormFile? profilePicture)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                updatedProfile.CurrentProfilePictureUrl = currentUser.ProfilePictureUrl;
                return View(updatedProfile);
            }

            currentUser.FullName = updatedProfile.FullName;
            currentUser.Address = updatedProfile.Address;

            if (profilePicture != null && profilePicture.Length > 0)
            {
                var extension = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();

                if (!AllowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError(string.Empty, "Only JPG, PNG or WEBP images are allowed.");
                    updatedProfile.CurrentProfilePictureUrl = currentUser.ProfilePictureUrl;
                    return View(updatedProfile);
                }

                if (profilePicture.Length > 2 * 1024 * 1024) // 2 MB limit
                {
                    ModelState.AddModelError(string.Empty, "Image must be smaller than 2 MB.");
                    updatedProfile.CurrentProfilePictureUrl = currentUser.ProfilePictureUrl;
                    return View(updatedProfile);
                }

                var avatarsFolder = Path.Combine(_environment.WebRootPath, "images", "avatars");
                Directory.CreateDirectory(avatarsFolder);

                var uniqueFileName = $"{currentUser.Id}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(avatarsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                // Clean up the old uploaded picture so we don't pile up unused files
                if (!string.IsNullOrEmpty(currentUser.ProfilePictureUrl) &&
                    currentUser.ProfilePictureUrl.StartsWith("/images/avatars/"))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, currentUser.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                currentUser.ProfilePictureUrl = $"/images/avatars/{uniqueFileName}";
            }

            var result = await _userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                updatedProfile.CurrentProfilePictureUrl = currentUser.ProfilePictureUrl;
                return View(updatedProfile);
            }

            TempData["SuccessMessage"] = "Your profile has been updated.";
            return RedirectToAction(nameof(Index));
        }

        [Route("/profile/favorites")]
        public async Task<IActionResult> Favorites()
        {
            var userId = _userManager.GetUserId(User);

            var allCategories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            var favoritedCategoryIds = await _context.UserFavoriteCategories
                .Where(f => f.UserId == userId)
                .Select(f => f.CategoryId)
                .ToListAsync();

            ViewBag.FavoritedCategoryIds = favoritedCategoryIds;
            return View(allCategories);
        }

        [HttpPost]
        [Route("/profile/favorites/toggle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int categoryId)
        {
            var userId = _userManager.GetUserId(User);

            var existingFavorite = await _context.UserFavoriteCategories
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CategoryId == categoryId);

            if (existingFavorite != null)
            {
                _context.UserFavoriteCategories.Remove(existingFavorite);
            }
            else
            {
                _context.UserFavoriteCategories.Add(new UserFavoriteCategory
                {
                    UserId = userId,
                    CategoryId = categoryId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Favorites));
        }

        [Route("/profile/preferences")]
        public async Task<IActionResult> Preferences()
        {
            var userId = _userManager.GetUserId(User);

            var preference = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            // First time here - give sensible defaults instead of an empty form
            preference ??= new UserPreference
            {
                UserId = userId,
                ThemeMode = "Light",
                FontSize = "Medium",
                EmailNotifications = true
            };

            return View(preference);
        }

        [HttpPost]
        [Route("/profile/preferences")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Preferences(UserPreference updatedPreference)
        {
            var userId = _userManager.GetUserId(User);

            ModelState.Remove(nameof(UserPreference.UserId));

            if (!ModelState.IsValid)
            {
                return View(updatedPreference);
            }

            var preference = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preference == null)
            {
                preference = new UserPreference { UserId = userId };
                _context.UserPreferences.Add(preference);
            }

            preference.ThemeMode = updatedPreference.ThemeMode;
            preference.FontSize = updatedPreference.FontSize;
            preference.EmailNotifications = updatedPreference.EmailNotifications;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Preferences saved.";
            return RedirectToAction(nameof(Preferences));
        }
    }
}
