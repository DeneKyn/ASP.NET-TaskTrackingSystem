using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class Chat
    {
        public int Id { get; set; }        
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Text { get; set; }
        //public DateTime CreationTime { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
