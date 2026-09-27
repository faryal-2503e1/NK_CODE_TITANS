using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Helpers
{
    public static class UserLookup
    {
        // Resolves user names for just the ids that are on screen, instead of loading every user.
        public static async Task<Dictionary<string, string>> GetUserNamesAsync(
            this UserManager<ApplicationUser> userManager, IEnumerable<string?> ids)
        {
            var wanted = ids.Where(i => !string.IsNullOrEmpty(i)).Select(i => i!).Distinct().ToList();
            if (wanted.Count == 0) return new Dictionary<string, string>();

            return await userManager.Users
                .Where(u => wanted.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName ?? u.Email ?? u.Id);
        }
    }
}
