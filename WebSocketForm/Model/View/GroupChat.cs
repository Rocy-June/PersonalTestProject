using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.View;

namespace WebSocketForm.Model.View
{
    public class GroupChat : Menu
    {
        public IPAddress OwnerID { get; set; }

        public DateTime ID { get; set; }

        public List<User> Members { get; set; }

        public string Name { get; set; }

        public override string LastSay => AppData.GetLastChat(ID).Message;

        public override DateTime LastChatTime => AppData.GetLastChat(ID).SendTime;

        public override string LastTimeStr => LastChatTime.ToString("MM-DD HH:mm");

        public override string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return Name;
                }
                return OwnerID.ToString();
            }
        }
    }
}
