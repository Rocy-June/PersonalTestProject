using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using Model.Data;
using Model.Enum;
using WebSocketForm.View;
using NetworkHandler;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {
        #region Server

        public static Server_TCP tcpServer = new Server_TCP(Setting.DATA_PORT, Setting.BUFFER_SIZE, TcpMessageReceived);

        public static Server_UDP broadcastServer = new Server_UDP(Setting.BROADCAST_PORT, BroadcastEventRecived);

        #endregion

        #region DataReceivedHandler

        public delegate void TcpMessageReceivedHandler<T>(T data, IPAddress ip);


        public static event TcpMessageReceivedHandler<Data_User> UserProfileDataReceived;

        #endregion

        #region BroadcastReceivedHandler

        public delegate void BroadcastReceivedHandler(IPAddress ip);


        public static event BroadcastReceivedHandler UnknownReceived;

        public static event BroadcastReceivedHandler LoginReceived;

        public static event BroadcastReceivedHandler LogoutReceived;

        public static event BroadcastReceivedHandler StillOnlineReceived;

        #endregion

        public static void OpenLocalServer()
        {
            new Thread(TCP_ServerReceive)
            {
                IsBackground = true
            }.Start();

            new Thread(UDP_BroadcastServerReceive)
            {
                IsBackground = true
            }.Start();
        }

        private static void TCP_ServerReceive()
        {
            tcpServer.Start();
        }

        private static void UDP_BroadcastServerReceive()
        {
            broadcastServer.Start();
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
                    TcpEventReceived(bytes.ToObject<TcpData>());
                }
            }
            else
            {
                TcpFileReceived(filePath);
            }

        }

        private static void TcpEventReceived(TcpData data)
        {
            switch (data.ActionType)
            {
                case TcpMessageType.EventMessage:
                    UserProfileDataReceived((Data_User)data.Data, data.SenderIP);
                    break;
                default:
                    break;
            }
        }

        private static void TcpFileReceived(string filePath)
        {
            //todo...
        }

        private static void BroadcastEventRecived(byte[] dataBytes)
        {
            var data = dataBytes.ToObject<BroadcastData>();

            switch (data.Action)
            {
                case BroadcastType.Unknown:
                    UnknownReceived.Invoke(data.IP);
                    break;
                case BroadcastType.Login:
                    LoginReceived.Invoke(data.IP);
                    break;
                case BroadcastType.Logout:
                    LogoutReceived.Invoke(data.IP);
                    break;
                case BroadcastType.StillOnline:
                    StillOnlineReceived.Invoke(data.IP);
                    break;
                default:
                    break;
            }
        }
    }
}
