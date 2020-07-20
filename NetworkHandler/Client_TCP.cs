using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkHandler
{
    public class Client_TCP
    {
        private readonly TcpClient client;

        private readonly int serverBufferSize;

        //Socket sclient;

        public Client_TCP(int serverBufferSize)
        {
            this.serverBufferSize = serverBufferSize;

            client = new TcpClient();
            //sclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        ~Client_TCP()
        {
            client.Close();
        }

        public void SendData(IPAddress ip, int port, byte[] dataBytes, int dataType)
        {
            //using (sclient)
            //{
            //    sclient.Connect(ip, port);
            //    bytes = dataBytes;
            //    sclient.Send(BitConverter.GetBytes(dataBytes.Length), SocketFlags.None);
            //    sclient.Send(BitConverter.GetBytes(dataType), SocketFlags.None);

            //    var buffer = new byte[1024];
            //    var bufferCount = dataBytes.Length / 1024;
            //    for (var i = 0; i < bufferCount; ++i)
            //    {
            //        Array.Copy(dataBytes, i * 1024, buffer, 0, 1024);
            //        sclient.Send(buffer, SocketFlags.None);
            //    }
            //    var lastBuffer = dataBytes.Length % 1024;
            //    if (lastBuffer != 0)
            //    {
            //        buffer = new byte[dataBytes.Length % 1024];
            //        Array.Copy(dataBytes, bufferCount * 1024, buffer, 0, lastBuffer);
            //        sclient.Send(buffer, SocketFlags.None);
            //    }
            //}

            client.Connect(ip, port);
            using (var stream = client.GetStream())
            {
                stream.Write(BitConverter.GetBytes(dataBytes.Length), 0, 4);
                stream.Write(BitConverter.GetBytes(dataType), 0, 4);

                var buffer = new byte[serverBufferSize];
                var bufferCount = dataBytes.Length / serverBufferSize;
                for (var i = 0; i < bufferCount; ++i)
                {
                    Array.Copy(dataBytes, i * serverBufferSize, buffer, 0, serverBufferSize);
                    stream.Write(buffer, 0, buffer.Length);
                }
                var lastBuffer = dataBytes.Length % 1024;
                if (lastBuffer != 0)
                {
                    buffer = new byte[lastBuffer];
                    Array.Copy(dataBytes, bufferCount * 1024, buffer, 0, lastBuffer);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

    }
}
