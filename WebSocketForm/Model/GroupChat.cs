using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.View;

namespace WebSocketForm.Model
{
    public class GroupChat : Menu
    {
        public IPAddress OwnerID { get; set; }

        public DateTime ID { get; set; }

        public List<User> Members { get; set; }

        public string Name { get; set; }

        public override string LastSay => Setting.GetLastChat(ID).Message;

        public override DateTime LastChatTime => Setting.GetLastChat(ID).SendTime;

        public override string LastTimeStr => LastChatTime.ToString("MM-DD HH:mm");
    }
}
