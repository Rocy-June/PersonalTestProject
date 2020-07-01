using NetworkHandler;
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
        /// <summary>
        /// 获取本地ip地址,优先取内网ip
        /// </summary>
        /// <returns>本地IP</returns>
        public static IPAddress GetLocalIp(bool getIpv6 = false)
        {
            var hosts = GetLocalIpAddreses();
            foreach (var ip in hosts)
            {
                if (ip.AddressFamily == (getIpv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork))
                {
                    return ip;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取本地ip地址。多个ip
        /// </summary>
        /// <returns>本地IP字符串列表</returns>
        public static IPAddress[] GetLocalIpAddreses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList;  //解析主机IP地址
        }

        public static void Send_TCP(IPAddress ip, TcpData data)
        {
            new Thread(() =>
            {
                var client = new Client_TCP();
                client.SendData(ip, Setting.DATA_PORT, data.ToBytes(), (int)data.ActionType);
            })
            {
                IsBackground = true
            }
            .Start();
        }

        public static void Send_Broadcast(BroadcastData message)
        {
            new Thread(() =>
            {
                var client = new Client_UDP();
                client.Broadcast(Setting.BROADCAST_PORT, message.ToBytes());
            })
            {
                IsBackground = true
            }
            .Start();
        }

    }
}
