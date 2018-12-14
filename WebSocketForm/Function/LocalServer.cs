using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketForm.Model;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {
        public static UdpClient server = new UdpClient(new IPEndPoint(IPAddress.Any, SocketTool.port));

        public delegate void RequestReceivedHandler<T>(T data, IPAddress ip);

        public static event RequestReceivedHandler<PostInfo<object>> UnknownRequestReceived;
        public static event RequestReceivedHandler<PostInfo<object>> LoginRequestReceived;
        public static event RequestReceivedHandler<PostInfo<object>> LogoutRequestReceived;
        public static event RequestReceivedHandler<PostInfo<object>> StillOnlineRequestReceived;
        public static event RequestReceivedHandler<PostInfo<object>> MessageSendRequestReceived;
        public static event RequestReceivedHandler<PostInfo<object>> FileSendRequestReceived;

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
                var data = server.Receive(ref receive_ipep).ToObject<PostInfo<object>>();

                switch (data.Action)
                {
                    case Enum.PostActionType.unknown:
                        UnknownRequestReceived(data, receive_ipep.Address);
                        break;
                    case Enum.PostActionType.login:
                        LoginRequestReceived(data, receive_ipep.Address);
                        break;
                    case Enum.PostActionType.logout:
                        LogoutRequestReceived(data, receive_ipep.Address);
                        break;
                    case Enum.PostActionType.stillOnline:
                        StillOnlineRequestReceived(data, receive_ipep.Address);
                        break;
                    case Enum.PostActionType.messageSend:
                        MessageSendRequestReceived(data, receive_ipep.Address);
                        break;
                    case Enum.PostActionType.fileSend:
                        FileSendRequestReceived(data, receive_ipep.Address);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
