using Core.Model;
using Core.Enum;
using Extend;
using NetworkHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using Core.Model.Network;
using System.Threading;

namespace Core.Server
{
    public static class TcpServer
    {

        public static Server_TCP tcpServer;

        public delegate void TcpMessageReceivedHandler<T>(T data, IPAddress ip);

        public static event TcpMessageReceivedHandler<User> UserProfileDataReceived;


        public static void Init(int port, int bufferSize)
        {
            tcpServer = new Server_TCP(port, bufferSize, TcpMessageReceived);
        }

        public static void StartServer()
        {
            new Thread(() => 
            {
                tcpServer.Start();
            })
            {
                IsBackground = true
            }
            .Start();
        }

        private static void TcpMessageReceived(string filePath, int packageSize, int type)
        {
            var enumType = (TcpMessageType)type;

            if ((type & 0x40000000) == 0)
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var bytes = new byte[packageSize];
                    fs.Read(bytes, 0, packageSize);
                    TcpEventReceived(bytes.ToObject<TcpPackage>());
                }
            }
            else
            {
                TcpFileReceived(filePath);
            }

        }

        private static void TcpEventReceived(TcpPackage data)
        {
            switch (data.ActionType)
            {
                case TcpMessageType.UserProfileDataReceived:
                    UserProfileDataReceived((User)data.Data, data.SenderIP);
                    break;
                default:
                    break;
            }
        }

        private static void TcpFileReceived(string filePath)
        {
            //todo...
        }

    }
}
