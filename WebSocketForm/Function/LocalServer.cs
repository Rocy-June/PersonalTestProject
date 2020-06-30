using NetworkHandler;
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
using WebSocketForm.Enum;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.View;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {
        #region Server

        /// <summary>
        /// 单包大小
        /// </summary>
        private const int BUFFER_SIZE = 1024;

        public static Server_TCP tcpServer = new Server_TCP(Setting.DATA_PORT, BUFFER_SIZE, TcpMessageReceived);

        public static UdpClient broadcastServer = new UdpClient(new IPEndPoint(IPAddress.Broadcast, Setting.BROADCAST_PORT));

        #endregion

        #region DataReceivedHandler

        public delegate void DataReceivedHandler<T>(T data, IPAddress ip);


        private static event DataReceivedHandler<UdpPackage> _UnknownReceived;

        private static event DataReceivedHandler<UdpPackage> _LoginReceived;

        private static event DataReceivedHandler<UdpPackage> _LogoutReceived;

        private static event DataReceivedHandler<UdpPackage> _StillOnlineReceived;


        public static event DataReceivedHandler<UdpPackage> UnknownReceived
        {
            add => _UnknownReceived += value;
            remove => _UnknownReceived -= value;
        }

        public static event DataReceivedHandler<UdpPackage> LoginReceived
        {
            add => _LoginReceived += value;
            remove => _LoginReceived -= value;
        }

        public static event DataReceivedHandler<UdpPackage> LogoutReceived
        {
            add => _LogoutReceived += value;
            remove => _LogoutReceived -= value;
        }

        public static event DataReceivedHandler<UdpPackage> StillOnlineReceived
        {
            add => _LogoutReceived += value;
            remove => _LogoutReceived -= value;
        }

        #endregion


        public static void OpenLocalServer()
        {
            new Thread(TCP_ServerReceive)
            {
                IsBackground = true
            }.Start();

            new Thread(UDP_ServerReceive)
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

            while (true)
            {
                using (var tcpClient = tcpServer.AcceptTcpClient())
                using (var ns = tcpClient.GetStream())
                using (var sr = new StreamReader(ns))
                {
                    List<byte> bufferList = new List<byte>(tcpClient.ReceiveBufferSize);
                    var i = 0;

                }
            }
        }

        private static void UDP_ServerReceive()
        {
            var receive_ipep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                var data = broadcastServer.Receive(ref receive_ipep).ToObject<BroadcastMessage>();
                if (data.IsRequest)
                {
                    if (data.NeedHandShake)
                    {
                        data.NeedHandShake = false;
                        data.IsRequest = false;
                        NetHelper.Send_UDP(receive_ipep.Address, data);
                    }
                }
                else
                {
                    BroadcastEventDataSend(data, receive_ipep);
                }
            }
        }

        private static void UDP_BroadcastServerReceive()
        {

        }

        private static void TcpMessageReceived(string filePath, int packageSize, int type)
        {
            var enumType = (TcpMessageType)type;
            switch (enumType)
            {
                case TcpMessageType.TextMessage:
                    break;
                case TcpMessageType.File:
                    break;
                default:
                    break;
            }
        }

        private static void BroadcastEventRecived(UdpPackage data, IPEndPoint ipep)
        {
            switch (data.Action)
            {
                case BroadcastType.Unknown:
                    _UnknownReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastType.Login:
                    _LoginReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastType.Logout:
                    _LogoutReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastType.StillOnline:
                    _StillOnlineReceived?.Invoke(data, ipep.Address);
                    break;
                default:
                    break;
            }
        }
    }
}
