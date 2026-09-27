using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Hubs
{
    // Smart Category Notifications: jab connect hota hai, user ki UserFavoriteCategory
    // list nikal kar us hisaab se "category-{id}" groups join kara dete hain. Jab
    // AdminContentController koi naya content publish karta hai, sirf usi category ke
    // groups ko toast broadcast hota hai — baaki fans ko wo notification nahi dikhta.
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationHub(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User);

            if (user != null)
            {
                var favoriteCategoryIds = await _context.UserFavoriteCategories
                    .Where(f => f.UserId == user.Id)
                    .Select(f => f.CategoryId)
                    .ToListAsync();

                foreach (var categoryId in favoriteCategoryIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"category-{categoryId}");
                }
            }

            await base.OnConnectedAsync();
        }

        // Agar user profile se ek naya favorite category add karta hai, connection band
        // kiye baghair bhi turant us category ke liye subscribe ho jaye.
        public async Task JoinCategoryGroup(int categoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"category-{categoryId}");
        }

        public async Task LeaveCategoryGroup(int categoryId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"category-{categoryId}");
        }
    }
}
