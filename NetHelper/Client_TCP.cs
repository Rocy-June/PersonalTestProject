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
        TcpClient client;

        public Client_TCP()
        {
            client = new TcpClient();
        }

        ~Client_TCP()
        {
            client.Close();
        }

        public void SendData(IPAddress ip, int port, byte[] dataBytes, int dataType)
        {
            client.Connect(ip, port);
            using (var stream = client.GetStream())
            {
                stream.Write(BitConverter.GetBytes(dataBytes.Length), 0, 4);
                stream.Write(BitConverter.GetBytes(dataType), 0, 4);
                stream.Write(dataBytes, 0, dataBytes.Length);
            }
            client.Close();
        }

    }
}
