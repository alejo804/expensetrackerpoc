using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ExpnenseTrackerApp.SignalR
{
    public class ExpenseHub : Hub
    {
        public Task BroadcastMessage(string name, string message) =>
            Clients.All.SendAsync("ExpenseRegistered", name, message);

        public Task Echo(string name, string message) =>
            Clients.Client(Context.ConnectionId)
                .SendAsync("echo", name, $"{message} (echo from server)");
    }
}
