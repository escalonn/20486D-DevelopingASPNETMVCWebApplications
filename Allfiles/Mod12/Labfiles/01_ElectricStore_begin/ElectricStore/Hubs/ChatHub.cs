﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ElectricStore.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAll(string user, string message)
        {
            await Clients.All.SendAsync(method: "NewMessage", arg1: user, arg2: message);
        }
    }
}
