using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static System.Console;

namespace WebSocketForm.Model
{
    public class Client : IConnection
    {
        //客户端的套接字
        private Socket thisClient;
        //接收服务器消息的线程
        private Thread clientThread;
        /// <summary>
        /// 启动客户端
        /// </summary>
        public bool StarUp(IPAddress ip, int port)
        {
            try
            {
                //建立套接字
                thisClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //连接到服务器
                thisClient.Connect(ip, port);
                //开启接收消息的线程
                clientThread = new Thread(AcceptServerMsg);
                clientThread.Start();

                return true;
            }
            catch (Exception e)
            {
                WriteLine(e);

                return false;
            }
        }
        /// <summary>
        /// 接收服务器的消息
        /// </summary>
        public void AcceptServerMsg()
        {
            //声明一个接收消息的字节数组
            var buffer = new byte[1024 * 64];
            //获取服务器的网络地址标识
            var serverEndPoint = thisClient.RemoteEndPoint as IPEndPoint;

            while (true)
            {
                try
                {
                    //接收消息
                    var len = thisClient.Receive(buffer);
                    var str = Encoding.UTF8.GetString(buffer, 0, len);
                    WriteLine(serverEndPoint.Address.ToString() + " 说 : " + str);
                }
                catch (Exception e)
                {
                    WriteLine(e);
                }
            }
        }
        /// <summary>
        /// 发消息
        /// </summary>        
        public void Send(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            thisClient.Send(buffer);
        }
    }
}
