using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model
{
    public class Chat
    {
        public IPAddress SenderID { get; set; }

        public ChatType ChatType { get; set; }

        public IPAddress ToUserChatID { get; set; }

        public DateTime ToGroupChatID { get; set; }

        public string Message { get; set; }

        public DateTime SendTime { get; set; }

        public ChatStatus ChatStatus { get; set; }
    }
}
