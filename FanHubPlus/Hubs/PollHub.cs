using Microsoft.AspNetCore.SignalR;

namespace FanHubPlus.Hubs
{
    // Har poll page apna group join kar leta hai, taake sirf usi poll ko dekhne
    // waale users ko us poll ke naye results ka broadcast mile.
    public class PollHub : Hub
    {
        public async Task JoinPollGroup(int pollId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"poll-{pollId}");
        }

        public async Task LeavePollGroup(int pollId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"poll-{pollId}");
        }
    }
}
