using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketForm.Model.Data;

namespace WebSocketForm.Helper
{
    static class NetHelper
    {
        private const int PACKAGE_SIZE = 1024;

        private const int UDP_REPEAT_COUNT = 2;

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

        public static void UDP_Send(IPAddress ip, BroadcastInfo data)
        {
            new Thread(() =>
            {
                using (var udp = new UdpClient(new IPEndPoint(IPAddress.Any, 0)))
                {
                    var ipep = new IPEndPoint(ip, Setting.PORT);
                    var bytes = data.ToBytes();
                    udp.Send(bytes, bytes.Length, ipep);
                }
            })
            {
                IsBackground = true
            }
            .Start();
        }

        public static void TCP_Send(IPAddress ip, PostInfo data)
        {
            new Thread(() =>
            {
                using (var tcp = new TcpClient())
                {
                    tcp.Connect(ip, Setting.PORT);
                    using (var ns = tcp.GetStream())
                    {
                        var bytes = data.ToBytes();
                        ns.Write(bytes, 0, bytes.Length);
                    }
                };
            })
            {
                IsBackground = true
            }
            .Start();
        }
    }
}
