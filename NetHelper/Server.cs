using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetHelper
{
    public class Server
    {
        private readonly TcpListener listener;

        private readonly int bufferSize;

        private readonly Action<string, int, int> messageCallback;

        private Socket socket;

        public Server(int port, int bufferSize, Action<string, int, int> messageCallback)
        {
            //初始化
            this.bufferSize = bufferSize;
            this.messageCallback = messageCallback;

            //启用服务器
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("ServerStarted.");
        }

        ~Server()
        {
            Stop();
        }

        public void Start()
        {
            Console.WriteLine("Listener Started.");

            listener.BeginAcceptSocket(ListenerAcceptedCallback, null);
        }

        public void Stop()
        {
            Console.WriteLine("Listener Stoped.");

            listener.Stop();
        }

        private void ListenerAcceptedCallback(IAsyncResult result)
        {
            Console.WriteLine("Client Connected.");

            try
            {
                socket = listener.EndAcceptSocket(result);

                //获取临时目录
                var tempPath = Environment.GetEnvironmentVariable("TEMP");
                var id = string.Empty;
                var filePath = string.Empty;

                //随机Guid临时文件名称
                do
                {
                    id = Guid.NewGuid().ToString();
                    filePath = $@"{tempPath}\\{id}.dat";
                }
                while (File.Exists(filePath));

                /* 
                 * 4byte => (int) 数据包总大小
                 * 4byte => (int) 数据包类型
                 * xbyte => (object) 数据
                 * 
                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

                //数据包总大小
                var packageSizeByte = new byte[4];
                socket.Receive(packageSizeByte);
                var packageSize = BitConverter.ToInt32(packageSizeByte, 0);

                //数据包类型
                var packageTypeByte = new byte[4];
                socket.Receive(packageTypeByte);
                var packageType = BitConverter.ToInt32(packageTypeByte, 0);

                //数据
                using (var fs = new FileStream(filePath, FileMode.CreateNew))
                {
                    var buffer = new byte[bufferSize];
                    for (var i = 0; i < packageSize / buffer.Length; ++i)
                    {
                        socket.Receive(buffer);
                        fs.Write(buffer, 0, buffer.Length);
                    }
                    var surBytesCount = packageSize % buffer.Length;
                    if (surBytesCount != 0)
                    {
                        var surBuffer = new byte[surBytesCount];
                        socket.Receive(surBuffer);
                        fs.Write(surBuffer, 0, surBuffer.Length);
                    }
                }

                //执行回调
                messageCallback(filePath, packageSize, packageType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"
=== ↓ Exception ↓ === 

Disconnected.

Exception Message:
{ex.Message}

Exception Stack Trace:
{ex.StackTrace}

=== ↑ Exception ↑ ===
");
            }
            finally
            {
                GC.Collect();
                Start();
            }
        }

    }
}
