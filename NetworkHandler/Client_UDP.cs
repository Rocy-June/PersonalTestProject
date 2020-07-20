using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkHandler
{
    public class Client_UDP
    {
        UdpClient client;

        public Client_UDP()
        {
            client = new UdpClient();
        }

        ~Client_UDP()
        {
            client.Close();
        }

        public void Broadcast(int port, byte[] dataBytes)
        {
            client.Send(dataBytes, dataBytes.Length, new IPEndPoint(IPAddress.Broadcast, port));
        }
    }
}
