using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Model.Enum;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using Model.Data;

namespace WebSocketForm.Function
{
    public static class Broadcast
    {
        /// <summary>
        /// 上线广播
        /// </summary>
        public static void SendOnlineBroadcasting()
        {
            NetHelper.Send_Broadcast(new BroadcastData()
            {
                Action = BroadcastType.Login,
                IP = new IPAddress(Setting.UserConfig.IP)
            });
        }

        /// <summary>
        /// 持续在线广播
        /// </summary>
        public static void SendStillOnlineBroadcasting()
        {
            while (true)
            {
                Thread.Sleep(30000);
                
                NetHelper.Send_Broadcast(new BroadcastData()
                {
                    Action = BroadcastType.StillOnline,
                    IP = new IPAddress(Setting.UserConfig.IP)
                });
            }
        }

        /// <summary>
        /// 离线广播
        /// </summary>
        public static void SendOfflineBroadcasting()
        {
            NetHelper.Send_Broadcast(new BroadcastData()
            {
                Action = BroadcastType.Logout,
                IP = new IPAddress(Setting.UserConfig.IP)
            });
        }
    }
}
