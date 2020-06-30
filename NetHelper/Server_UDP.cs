using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkHandler
{
    class Server_UDP<T> where T : new()
    {
        private readonly UdpClient listener;

        private readonly Action<byte[]> messageCallback;

        private bool isDisposed;

        public Server_UDP(int port, Action<byte[]> messageCallback)
        {
            this.messageCallback = messageCallback;

            listener = new UdpClient(new IPEndPoint(IPAddress.Any, port));
        }

        ~Server_UDP()
        {
            Stop();
        }

        public void Start()
        {
            var ipe = (IPEndPoint)null;

            while (!isDisposed)
            {
                try
                {
                    Debug.UDP_Log("Listener Accept Broadcast.");

                    var data = listener.Receive(ref ipe);
                    messageCallback(data);
                }
                catch (Exception ex)
                {
                    Debug.UDP_Log($@"
=== ↓ Exception ↓ === 

Disconnected?

Exception Message:
{ex.Message}

Exception Stack Trace:
{ex.StackTrace}

=== ↑ Exception ↑ ===
");
                }
            }
        }

        public void Stop()
        {
            Debug.UDP_Log("Listener Stoped.");

            isDisposed = true;
            listener.Close();
        }
    }
}