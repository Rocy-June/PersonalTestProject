using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WebSocketForm.View;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {

        public static UdpClient server = new UdpClient(new IPEndPoint(IPAddress.Any, SocketTool.port));




        public delegate void RequestReceivedHandler<T>(T data, IPAddress ip);

        private static event RequestReceivedHandler<PostInfo> _UnknownReceived;

        private static event RequestReceivedHandler<PostInfo> _LoginReceived;

        private static event RequestReceivedHandler<PostInfo> _LogoutReceived;

        private static event RequestReceivedHandler<PostInfo> _StillOnlineReceived;

        private static event RequestReceivedHandler<PostInfo> _MessageSendReceived;

        private static event RequestReceivedHandler<PostInfo> _FileSendReceived;




        public static event RequestReceivedHandler<PostInfo> UnknownReceived
        {
            add
            {
                _UnknownReceived += value;
            }

            remove
            {
                _UnknownReceived -= value;
            }
        }

        public static event RequestReceivedHandler<PostInfo> LoginReceived
        {
            add
            {
                _LoginReceived += value;
            }

            remove
            {
                _LoginReceived -= value;
            }
        }

        public static event RequestReceivedHandler<PostInfo> LogoutReceived
        {
            add
            {
                _LogoutReceived += value;
            }

            remove
            {
                _LogoutReceived -= value;
            }
        }

        public static event RequestReceivedHandler<PostInfo> StillOnlineReceived
        {
            add
            {
                _LogoutReceived += value;
            }

            remove
            {
                _LogoutReceived -= value;
            }
        }

        public static event RequestReceivedHandler<PostInfo> MessageSendReceived
        {
            add
            {
                _MessageSendReceived += value;
            }

            remove
            {
                _MessageSendReceived -= value;
            }
        }

        public static event RequestReceivedHandler<PostInfo> FileSendReceived
        {
            add
            {
                _FileSendReceived += value;
            }
            remove
            {
                _FileSendReceived -= value;
            }
        }




        public static void OpenLocalServer()
        {
            new Thread(ServerReceive)
            {
                IsBackground = true
            }.Start();
        }

        private static void ServerReceive()
        {
            var receive_ipep = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                var data = server.Receive(ref receive_ipep).ToObject<PostInfo>();

                switch (data.Action)
                {
                    case PostActionType.unknown:
                        _UnknownReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    case PostActionType.login:
                        _LoginReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    case PostActionType.logout:
                        _LogoutReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    case PostActionType.stillOnline:
                        _StillOnlineReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    case PostActionType.messageSend:
                        _MessageSendReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    case PostActionType.fileSend:
                        _FileSendReceived?.Invoke(data, receive_ipep.Address);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
