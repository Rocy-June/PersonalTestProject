using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WebSocketForm.Model
{
    class Function
    {
        /// <summary>
        /// 获取本地ip地址,优先取内网ip
        /// </summary>
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
        public static string[] GetLocalIpAddress()
        {
            var hostName = Dns.GetHostName();                   //获取主机名称  
            var addresses = Dns.GetHostAddresses(hostName);     //解析主机IP地址  

            return addresses.Select(e => e.ToString()).ToArray();
        }
    }
}
