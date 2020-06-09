using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebSocketForm.Enum;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Function
{
    class SocketTool
    {
        public static readonly int port = 8009;

        /// <summary>
        /// 获取本地ip地址,优先取内网ip
        /// </summary>
        /// <returns>本地IP字符串</returns>
        public static IPAddress GetLocalIp(bool getIpv6 = false)
        {
            var ips = GetLocalIpAddreses();

            foreach (var ip in ips) if (ip.IsIPv6LinkLocal == getIpv6) return ip;

            return null;
        }

        /// <summary>
        /// 获取本地ip地址。多个ip
        /// </summary>
        /// <returns>本地IP字符串列表</returns>
        public static IPAddress[] GetLocalIpAddreses()
        {
            var hostName = Dns.GetHostName();                   //获取主机名称  
            var addresses = Dns.GetHostAddresses(hostName);     //解析主机IP地址  

            return Dns.GetHostAddresses(hostName);
        }

        /// <summary>
        /// 上线广播
        /// </summary>
        public static void OnlineBroadcasting()
        {
            var postData = new PostInfo()
            {
                Action = PostActionType.login,
                Data = ModelHelper.FileUserToDataUser(Setting.UserConfig),
                IP = new IPAddress(Setting.UserConfig.IP)
            };

            CommunicationHelper.UDP_Send(IPAddress.Broadcast, postData);
        }

        /// <summary>
        /// 持续在线广播
        /// </summary>
        public static void StillOnlineBroadcasting()
        {
            var udpClient = new UdpClient();
            var ipep = new IPEndPoint(IPAddress.Broadcast, port);

            var postData = new PostInfo()
            {
                Action = PostActionType.stillOnline
            };
            var bytesData = postData.ToBytes();

            while (true)
            {
                Thread.Sleep(30000);
                udpClient.SendAsync(bytesData, bytesData.Length, ipep);
            }
        }

        /// <summary>
        /// 离线广播
        /// </summary>
        public static void OfflineBroadcasting()
        {
            var udpClient = new UdpClient();
            var ipep = new IPEndPoint(IPAddress.Broadcast, port);

            var postData = new PostInfo()
            {
                Action = PostActionType.logout
            };
            var bytesData = postData.ToBytes();

            udpClient.SendAsync(bytesData, bytesData.Length, ipep);
        }
    }
}
