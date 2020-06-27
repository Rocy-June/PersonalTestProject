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
using WebSocketForm.View;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {
        #region Server

        public static TcpListener tcpServer = new TcpListener(IPAddress.Any, Setting.PORT);

        public static UdpClient udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, Setting.PORT));

        #endregion

        #region GetDataRequestReceivedHandler

        public delegate void GetDataRequestReceivedHandler<T>(T data, IPAddress ip);


        private static event GetDataRequestReceivedHandler<BroadcastMessage> _GetUnknownDataReceived;

        private static event GetDataRequestReceivedHandler<BroadcastMessage> _GetLoginDataReceived;

        private static event GetDataRequestReceivedHandler<BroadcastMessage> _GetLogoutDataReceived;

        private static event GetDataRequestReceivedHandler<BroadcastMessage> _GetStillOnlineDataReceived;


        public static event GetDataRequestReceivedHandler<BroadcastMessage> GetUnknownDataReceived
        {
            add => _GetUnknownDataReceived += value;
            remove => _GetUnknownDataReceived -= value;
        }

        public static event GetDataRequestReceivedHandler<BroadcastMessage> GetLoginDataReceived
        {
            add => _GetLoginDataReceived += value;
            remove => _GetLoginDataReceived -= value;
        }

        public static event GetDataRequestReceivedHandler<BroadcastMessage> GetLogoutDataReceived
        {
            add => _GetLogoutDataReceived += value;
            remove => _GetLogoutDataReceived -= value;
        }

        public static event GetDataRequestReceivedHandler<BroadcastMessage> GetStillOnlineDataReceived
        {
            add => _GetStillOnlineDataReceived += value;
            remove => _GetStillOnlineDataReceived -= value;
        }

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
                var data = udpServer.Receive(ref receive_ipep).ToObject<BroadcastMessage>();
                if (data.IsRequest)
                {
                    if (data.NeedHandShake)
                    {
                        data.NeedHandShake = false;
                        data.IsRequest = false;
                        NetHelper.SendData_UDP(receive_ipep.Address, data);
                    }
                }
                else
                {
                    BroadcastEventDataSend(data, receive_ipep);
                }
            }
        }

        private static void BroadcastEventDataSend(BroadcastMessage data, IPEndPoint ipep)
        {
            switch (data.Action)
            {
                case BroadcastActionType.Unknown:
                    _GetUnknownDataReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.Login:
                    _GetLoginDataReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.Logout:
                    _GetLogoutDataReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.StillOnline:
                    _GetStillOnlineDataReceived?.Invoke(data, ipep.Address);
                    break;
                default:
                    break;
            }
        }

        private static void BroadcastEventRecived(UdpPackage data, IPEndPoint ipep)
        {
            switch (data.Action)
            {
                case BroadcastActionType.Unknown:
                    _UnknownReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.Login:
                    _LoginReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.Logout:
                    _LogoutReceived?.Invoke(data, ipep.Address);
                    break;
                case BroadcastActionType.StillOnline:
                    _StillOnlineReceived?.Invoke(data, ipep.Address);
                    break;
                default:
                    break;
            }
        }
    }
}
