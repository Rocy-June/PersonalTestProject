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
using WebSocketForm.Model;

namespace WebSocketForm.Function
{
    public static class LocalServer
    {
        public static UdpClient server = new UdpClient(new IPEndPoint(IPAddress.Any, SocketTool.port));

        public delegate void RequestReceivedHandler<T>(T data, IPAddress ip);

        private static readonly List<RequestReceivedHandler<PostInfo<object>>> UnknownReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();
        private static readonly List<RequestReceivedHandler<PostInfo<object>>> LoginReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();
        private static readonly List<RequestReceivedHandler<PostInfo<object>>> LogoutReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();
        private static readonly List<RequestReceivedHandler<PostInfo<object>>> StillOnlineReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();
        private static readonly List<RequestReceivedHandler<PostInfo<object>>> MessageSendReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();
        private static readonly List<RequestReceivedHandler<PostInfo<object>>> FileSendReceivedList = new List<RequestReceivedHandler<PostInfo<object>>>();

        public static event RequestReceivedHandler<PostInfo<object>> UnknownReceived
        {
            add
            {
                if (UnknownReceivedList.Find(e => e == value) == null)
                {
                    UnknownReceivedList.Add(value);
                }
            }
            remove
            {
                if (UnknownReceivedList.Exists(e => e == value))
                {
                    UnknownReceivedList.Remove(value);
                }
            }
        }
        public static event RequestReceivedHandler<PostInfo<object>> LoginReceived
        {
            add
            {
                if (LoginReceivedList.Find(e => e == value) == null)
                {
                    LoginReceivedList.Add(value);
                }
            }
            remove
            {
                if (LoginReceivedList.Exists(e => e == value))
                {
                    LoginReceivedList.Remove(value);
                }
            }
        }
        public static event RequestReceivedHandler<PostInfo<object>> LogoutReceived
        {
            add
            {
                if (LogoutReceivedList.Find(e => e == value) == null)
                {
                    LogoutReceivedList.Add(value);
                }
            }
            remove
            {
                if (LogoutReceivedList.Exists(e => e == value))
                {
                    LogoutReceivedList.Remove(value);
                }
            }
        }
        public static event RequestReceivedHandler<PostInfo<object>> StillOnlineReceived
        {
            add
            {
                if (StillOnlineReceivedList.Find(e => e == value) == null)
                {
                    StillOnlineReceivedList.Add(value);
                }
            }
            remove
            {
                if (StillOnlineReceivedList.Exists(e => e == value))
                {
                    StillOnlineReceivedList.Remove(value);
                }
            }
        }
        public static event RequestReceivedHandler<PostInfo<object>> MessageSendReceived
        {
            add
            {
                if (MessageSendReceivedList.Find(e => e == value) == null)
                {
                    MessageSendReceivedList.Add(value);
                }
            }
            remove
            {
                if (MessageSendReceivedList.Exists(e => e == value))
                {
                    MessageSendReceivedList.Remove(value);
                }
            }
        }
        public static event RequestReceivedHandler<PostInfo<object>> FileSendReceived
        {
            add
            {
                if (FileSendReceivedList.Find(e => e == value) == null)
                {
                    FileSendReceivedList.Add(value);
                }
            }
            remove
            {
                if (FileSendReceivedList.Exists(e => e == value))
                {
                    FileSendReceivedList.Remove(value);
                }
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
            var thisDispatcher = Dispatcher.FromThread(MainWindow.currentThread);

            var receive_ipep = new IPEndPoint(IPAddress.Any, 0);
            
            while (true)
            {
                var data = server.Receive(ref receive_ipep).ToObject<PostInfo<object>>();

                switch (data.Action)
                {
                    case PostActionType.unknown:
                        foreach (var @event in UnknownReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    case PostActionType.login:
                        foreach (var @event in LoginReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    case PostActionType.logout:
                        foreach (var @event in LogoutReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    case PostActionType.stillOnline:
                        foreach (var @event in StillOnlineReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    case PostActionType.messageSend:
                        foreach (var @event in MessageSendReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    case PostActionType.fileSend:
                        foreach (var @event in FileSendReceivedList)
                        {
                            thisDispatcher.Invoke(@event, data, receive_ipep.Address);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
