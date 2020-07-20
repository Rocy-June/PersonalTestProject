using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Helper;
using Model.Data;
using Model.Enum;

namespace WebSocketForm.Function
{
    public static class TcpMessage
    {
        public static void SendUserProfile(IPAddress ip)
        {
            NetHelper.Send_TCP(ip, new TcpData()
            {
                ActionType = TcpMessageType.EventMessage,
                Data = ModelHelper.FileUserToDataUser(Setting.UserConfig),
                SenderIP = NetHelper.GetLocalIp()
            });
        }
    }
}
