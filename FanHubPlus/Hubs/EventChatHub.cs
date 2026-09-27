using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using FanHubPlus.Data;

namespace FanHubPlus.Hubs
{
    [Authorize]
    public class EventChatHub : Hub
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventChatHub(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Har event ka apna group hai, taake ek event ke messages doosre event mein na dikhein
        public async Task JoinEventGroup(int eventId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"event-{eventId}");
        }

        public async Task LeaveEventGroup(int eventId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"event-{eventId}");
        }

        // Client sirf message aur eventId bhejta hai - naam aur ID hum khud server par
        // logged-in user se nikalte hain, taake koi client se fake naam ya email na bhej sake
        public async Task SendMessage(int eventId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var user = await _userManager.GetUserAsync(Context.User);
            if (user == null)
            {
                return;
            }

            var displayName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : user.UserName;

            var chatMessage = new EventChatMessage
            {
                EventId = eventId,
                UserId = user.Id,
                UserName = displayName,
                Message = message.Trim()
            };

            _context.EventChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group($"event-{eventId}").SendAsync("ReceiveMessage", displayName, chatMessage.Message, chatMessage.SentAt.ToString("hh:mm tt"));
        }
    }
}

