using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Principal;
using dancelog.Models;

namespace dancelog.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name ?? Context.ConnectionId;
            OnlineUsers[Context.ConnectionId] = userName;

            await Clients.All.SendAsync("UserListUpdate", OnlineUsers.Values.Distinct().ToList());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            OnlineUsers.TryRemove(Context.ConnectionId, out _); 
            await Clients.All.SendAsync("UserListUpdate", OnlineUsers.Values.Distinct().ToList());
            await base.OnDisconnectedAsync(exception); 
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("Receive", user, message);
        }

        public async Task SendCourseUpdate(Course course)
        {
            await Clients.All.SendAsync("CourseUpdated", course);
        }

        public async Task SendCourseDelete(int courseId)
        {
            await Clients.All.SendAsync("CourseDeleted", courseId);
        }
    }
}
