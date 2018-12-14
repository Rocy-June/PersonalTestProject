using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebSocketForm.Enum;
using WebSocketForm.Model;

namespace WebSocketForm.Function
{
    class SocketTool
    {
        public static readonly int port = 8009;

        /// <summary>
        /// 获取本地ip地址,优先取内网ip
        /// </summary>
        /// <returns>本地IP字符串</returns>
        public static string GetLocalIp()
        {
            var ips = GetLocalIpAddress();

            foreach (string ip in ips) if (ip.StartsWith("10.80.")) return ip;
            foreach (string ip in ips) if (ip.Contains(".")) return ip;

            return "127.0.0.1";
        }

        /// <summary>
        /// 获取本地ip地址。多个ip
        /// </summary>
        /// <returns>本地IP字符串列表</returns>
        public static string[] GetLocalIpAddress()
        {
            var hostName = Dns.GetHostName();                   //获取主机名称  
            var addresses = Dns.GetHostAddresses(hostName);     //解析主机IP地址  

            return addresses.Select(e => e.ToString()).ToArray();
        }

        /// <summary>
        /// 上线广播
        /// </summary>
        public static void OnlineBroadcasting()
        {
            var udpClient = new UdpClient();
            var ipep = new IPEndPoint(IPAddress.Broadcast, port);

            var postData = new PostInfo<object>()
            {
                Action = PostActionType.login
            };
            var bytesData = postData.ToBytes();

            udpClient.SendAsync(bytesData, bytesData.Length, ipep);
        }

        /// <summary>
        /// 离线广播
        /// </summary>
        public static void OfflineBroadcasting()
        {
            var udpClient = new UdpClient();
            var ipep = new IPEndPoint(IPAddress.Broadcast, port);

            var postData = new PostInfo<object>()
            {
                Action = PostActionType.logout
            };
            var bytesData = postData.ToBytes();

            udpClient.SendAsync(bytesData, bytesData.Length, ipep);
        }
    }
}
