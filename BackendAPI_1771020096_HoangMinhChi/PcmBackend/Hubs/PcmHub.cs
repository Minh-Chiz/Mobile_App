using Microsoft.AspNetCore.SignalR;

namespace PcmBackend.Hubs
{
    public class PcmHub : Hub
    {
        // Client sẽ lắng nghe các hàm này: "ReceiveNotification", "UpdateCalendar", "UpdateMatchScore"

        // Ví dụ: Join vào group của một trận đấu cụ thể để xem tỉ số live
        public async Task JoinMatchGroup(string matchId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Match_{matchId}");
        }

        public async Task LeaveMatchGroup(string matchId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Match_{matchId}");
        }
    }
}