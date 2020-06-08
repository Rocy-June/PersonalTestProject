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
    static class CommunicationHelper
    {
        private const int PORT = 8009;

        private const int PACKAGE_SIZE = 1024;

        private const int UDP_REPEAT_COUNT = 2;

        public static void UDP_Send(IPAddress ip, object data)
        {
            new Thread(() =>
            {
                var udp = new UdpClient();
                var ipep = new IPEndPoint(ip, PORT);

                var dataPackes = SplitData(data.ToBytes(), PACKAGE_SIZE);
                for (var i = 0; i < UDP_REPEAT_COUNT; ++i)
                {
                    for (var j = 0; j < dataPackes.Length; ++j)
                    {
                        var packageBytes = dataPackes.ToBytes();
                        udp.Send(packageBytes, packageBytes.Length, ipep);
                    }
                }
            })
            {
                IsBackground = true
            }
            .Start();
        }

        private static Data_Package[] SplitData(byte[] data, int packageSize)
        {
            var id = DateTime.Now.Ticks;
            var splitPackagesCount = data.Length / packageSize + (data.Length % packageSize != 0 ? 1 : 0);
            var result = new Data_Package[splitPackagesCount];

            for (int i = 0; i < splitPackagesCount; i++)
            {
                result[0] = new Data_Package()
                {
                    ID = id,
                    Index = i,
                    DataLength = data.Length
                };
                Buffer.BlockCopy(data, packageSize * i, result[0].Data, 0, packageSize);
            }

            return result;
        }
    }
}
