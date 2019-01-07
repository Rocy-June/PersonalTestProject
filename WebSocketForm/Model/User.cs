using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model
{
    public class User : Menu
    {
        public IPAddress IP { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public OnlineStatus OnlineStatus { get; set; }

        public DateTime LastRespondTime { get; set; }

        public char GetStatusIconFont()
        {
            return IconFontChar.Get(
                new Dictionary<OnlineStatus, IconFont>() {
                    { OnlineStatus.Unknow, IconFont.unknow },
                    { OnlineStatus.Offline, IconFont.unknow },
                    { OnlineStatus.Online, IconFont.unknow },
                    { OnlineStatus.Hiding, IconFont.unknow },
                    { OnlineStatus.Leaving, IconFont.clock_fill },
                    { OnlineStatus.Busy, IconFont.clock_fill },
                }[OnlineStatus]
            );
        }
    }

    public class User_Post
    {
        public IPAddress IP { get; set; }

        public Bitmap HeadImage { get; set; }

        public string Name { get; set; }

        public OnlineStatus Status { get; set; }
    }
}
