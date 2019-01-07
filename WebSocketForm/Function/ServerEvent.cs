using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;
using WebSocketForm.Model;

namespace WebSocketForm.Function
{
    class ServerEvent
    {
        public static void LocalServer_LoginReceived(PostInfo data, IPAddress ip)
        {
            var userData = (User)data.Data;

            Setting.AddMessageMenu(new IMenu()
            {
                IP = ip,
                LastSay = "我上线了",
                LastTime = DateTime.Now,
                Status = new List<IconFont>(),
                Title = "测试"
            });


        }

        public static void LocalServer_LogoutReceived(PostInfo data, IPAddress ip)
        {
            var userData = (User)data.Data;

            Setting.AddMessageMenu(new IMenu()
            {
                IP = ip,
                IsTop = false,
                LastSay = "我下线了",
                LastTime = DateTime.Now,
                Status = new List<IconFont>(),
                Title = "测试"
            });


        }
    }
}
