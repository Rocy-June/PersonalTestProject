using Core.Enum;
using Core.Model.Network;
using Extend;
using NetworkHandler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Core.Server
{
    public static class BoardcastServer
    {

        public static Server_UDP boardcastServer;

        public delegate void BroadcastReceivedHandler(IPAddress ip);

        public static event BroadcastReceivedHandler UnknownReceived;

        public static event BroadcastReceivedHandler LoginReceived;

        public static event BroadcastReceivedHandler LogoutReceived;

        public static event BroadcastReceivedHandler StillOnlineReceived;

        public static void Init(int port)
        {
            boardcastServer = new Server_UDP(port, BroadcastEventRecived);
        }

        public static void StartServer()
        {
            new Thread(() =>
            {
                boardcastServer.Start();
            })
            {
                IsBackground = true
            }
            .Start();
        }

        private static void BroadcastEventRecived(byte[] dataBytes)
        {
            var data = dataBytes.ToObject<BoardcastPackage>();

            switch (data.Action)
            {
                case BoardcastMessageType.Unknown:
                    UnknownReceived.Invoke(data.IP);
                    break;
                case BoardcastMessageType.Login:
                    LoginReceived.Invoke(data.IP);
                    break;
                case BoardcastMessageType.Logout:
                    LogoutReceived.Invoke(data.IP);
                    break;
                case BoardcastMessageType.StillOnline:
                    StillOnlineReceived.Invoke(data.IP);
                    break;
                default:
                    break;
            }
        }
    }
}
