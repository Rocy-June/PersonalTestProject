using NetworkHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Server
{
    public static class TcpServer
    {

        public static Server_TCP tcpServer = new Server_TCP(Setting.DATA_PORT, Setting.BUFFER_SIZE, TcpMessageReceived);

        public static void Init(int port, int bufferSize)
        {

        }

    }
}
