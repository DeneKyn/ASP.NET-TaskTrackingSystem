using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskTrackingSystem.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        public async Task Send(string username, string text, string chatid)
        {
            int id = Convert.ToInt16(chatid);
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            var chat = db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);

            var message = new ChatMessage { Text = text, UserId = user.Id, ChatId = chat.Id };

            db.ChatMessages.Add(message);
            db.SaveChanges(); 

             await Clients.All.SendAsync("ReceiveMessage", message.Id.ToString(), username, message.Text, chat.Id);

        }
    }
}
