using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortPinger
{
    class Ping_Handler
    {
        public const int TIMEOUT = 3000;

        public static long PingIP(string strIP, int timeout = TIMEOUT)
        {
            try
            {
                using (var pingSend = new Ping())
                {
                    var reply = pingSend.Send(strIP, timeout);
                    if (reply.Status == IPStatus.Success)
                        return reply.RoundtripTime;
                    return TIMEOUT + 1;
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}
