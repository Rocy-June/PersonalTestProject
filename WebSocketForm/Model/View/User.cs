using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model.View
{
    public class User : Menu
    {
        public IPAddress IP { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public OnlineStatus OnlineStatus { get; set; }

        public DateTime LastResponsedTime { get; set; }

        public override string LastSay => AppData.GetLastChat(IP)?.Message ?? "";

        public override DateTime LastChatTime => AppData.GetLastChat(IP)?.SendTime ?? new DateTime(0);

        public override string LastTimeStr => LastChatTime.ToString("MM-dd HH:mm");

        public override string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NickName))
                {
                    return NickName;
                }
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return Name;
                }
                return IP.ToString();
            }
        }

        public char GetStatusIconFont()
        {
            return IconFontChar.Get(
                new Dictionary<OnlineStatus, IconFont>() {
                    { OnlineStatus.Unknow, IconFont.unknow },
                    { OnlineStatus.Offline, IconFont.unknow },
                    { OnlineStatus.Online, IconFont.unknow },
                    //{ OnlineStatus.Hiding, IconFont.unknow },
                    { OnlineStatus.Leaving, IconFont.clock_fill },
                    { OnlineStatus.Busy, IconFont.clock_fill },
                }[OnlineStatus]
            );
        }
    }
}
