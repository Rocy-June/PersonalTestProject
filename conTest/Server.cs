using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketForm.Model.Interface;
using static System.Console;

namespace WebSocketForm.Model
{
    public class Server : IConnection
    {
        Socket clientSocket;
        Socket server;
        Thread serverThread;

        /// <summary>
        /// 启动服务器
        /// </summary>
        public bool StartUp(IPAddress ip, int port)
        {
            try
            {
                //建立套接字  ： 寻址方案，套接字类型，协议类型
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //配置本地地址
                EndPoint endPoint = new IPEndPoint(ip, port);
                server.Bind(endPoint);
                //监听和接受客户端请求（30为接受客户端的数量）
                server.Listen(30);
                //开启接受客户端连接的 线程
                serverThread = new Thread(AcceptClientConnect);
                serverThread.Start();

                return true;
            }
            catch (Exception e)
            {
                WriteLine(e);

                return false;
            }
        }

        /// <summary>
        /// 接受客户端的连接
        /// </summary>
        private void AcceptClientConnect()
        {
            while (true)
            {
                try
                {
                    //接受客户端的连接
                    Socket clientSocket = server.Accept();
                    //发消息所需参数（可以针对发送消息扩展，此处只是简单对一个固定客户端发送消息）
                    this.clientSocket = clientSocket;
                    //获取客户端的网络地址标识
                    IPEndPoint clientEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                    //打印下谁连接上了服务器
                    WriteLine(clientEndPoint.Address.ToString() + "连接上了服务器。\r\n");
                    //开一个接受客户端消息的线程
                    Thread accrptClientMsg = new Thread(AcceptMsg);
                    accrptClientMsg.Start(clientSocket);
                }
                catch (Exception e)
                {
                    WriteLine(e);
                }
            }
        }

        /// <summary>
        /// 接收客户端消息
        /// </summary>       
        private void AcceptMsg(object obj)
        {
            //由于线程只能传递object参数，所以此处需要把object强转成Socket
            var client = obj as Socket;
            //声明一个字节数组 用来接收消息
            var buffer = new byte[client.ReceiveBufferSize];
            //获取客户端的网络地址标识
            var clientEndPoint = client.RemoteEndPoint as IPEndPoint;

            while (true)
            {
                //接收消息
                var len = client.Receive(buffer);
                var str = Encoding.UTF8.GetString(buffer, 0, len);
                WriteLine(clientEndPoint.Address.ToString() + " 说 : " + str);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>       
        public void Send(string str)
        {
            var strByteArr = Encoding.UTF8.GetBytes(str);
            if (clientSocket == null)
            {
                WriteLine("尚未有客户端连接。");
                return;
            }
            clientSocket.Send(strByteArr);
        }
    }
}
