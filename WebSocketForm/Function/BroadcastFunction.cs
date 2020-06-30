using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebSocketForm.Enum;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Function
{
    class BroadcastFunction
    {
        /// <summary>
        /// 上线广播
        /// </summary>
        public static void OnlineBroadcasting()
        {
            var postData = new BroadcastMessage()
            {
                Action = BroadcastActionType.Login,
                IP = new IPAddress(Setting.UserConfig.IP)
            };

            NetHelper.Send_UDP(IPAddress.Broadcast, postData);
        }

        /// <summary>
        /// 持续在线广播
        /// </summary>
        public static void StillOnlineBroadcasting()
        {
            while (true)
            {
                Thread.Sleep(30000);

                var postData = new BroadcastMessage()
                {
                    Action = BroadcastActionType.StillOnline,
                    IP = new IPAddress(Setting.UserConfig.IP)
                };

                NetHelper.Send_UDP(IPAddress.Broadcast, postData);
            }
        }

        /// <summary>
        /// 离线广播
        /// </summary>
        public static void OfflineBroadcasting()
        {
            var postData = new BroadcastMessage()
            {
                Action = BroadcastActionType.Logout,
                IP = new IPAddress(Setting.UserConfig.IP)
            };

            NetHelper.Send_UDP(IPAddress.Broadcast, postData);
        }
    }
}
