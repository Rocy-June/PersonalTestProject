using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static cTest.Tools;

namespace cTest
{
    class Program
    {

        readonly static IPAddress ip = IPAddress.Parse(GetLocalIp());
        readonly static int port = 8009;
        private static bool isInputError = false;
        private static string lastInput = "";
        static void Main(string[] args)
        {
            while (true)
            {
                WS(new List<string> {
                    "1. 建立主机",
                    "2. 连接主机",
                    "*. 退出"
                });

                switch (lastInput.Int())
                {
                    case 1:
                        CreatServer();
                        break;
                    case 2:
                        SelectLinkWay();
                        break;
                    default:
                        return;
                }
            }


        }
        static void CreatServer()
        {
            while (true)
            {
                Server server = new Server();
                var isSuccessed = server.StartUp(ip, port);

                if (isSuccessed)
                {
                    WS(new List<string> { "主机建立完成, 等待客户端连接。" }, false);

                    Readiness(server);
                }
                else
                {
                    WS(new List<string> {
                        "主机建立失败, 报出以上错误。",
                        "",
                        "是否重试?",
                        "1. 是",
                        "*. 否"
                    }, true, true, false);

                    if (lastInput.Int() != 1)
                    {
                        break;
                    }
                }
            }
        }

        static void SelectLinkWay()
        {
            WS(new List<string> {
                "1. 手动输入IP地址强制连接",
                "*. 搜索连接"
            });
            if (lastInput.Int() == 1)
            {
                InputServer();
            }
            else
            {
                SelectServer();
            }
        }

        static void SelectServer()
        {
            while (true)
            {
                WS(new List<string> { "正在搜索同一网段内的在线IP..." }, false);

                var ipList = GetIpList();
                if (ipList.Count == 0)
                {
                    WS(new List<string> {
                        "没有在线的主机, 是否建立主机?",
                        "1. 是",
                        "*. 否"
                    });
                    if (lastInput.Int() == 1)
                    {
                        CreatServer();
                        break;
                    }
                }
                else
                {
                    IPAddress connectToIp;
                    if (ipList.Count == 1)
                    {
                        connectToIp = IPAddress.Parse(ipList[0]);
                    }
                    else
                    {
                        while (true)
                        {
                            var manyIpStrs = new List<string> { "搜索到多个可连接IP" };
                            var i = 0;
                            manyIpStrs.AddRange(ipList.Select(e => { return $"{++i}. {e}"; }));
                            WS(manyIpStrs, true, false);
                            var ipSelectedIndex = lastInput.Int();
                            if (ipSelectedIndex > 0 && ipSelectedIndex <= ipList.Count)
                            {
                                connectToIp = IPAddress.Parse(ipList[ipSelectedIndex - 1]);
                                break;
                            }
                            else
                            {
                                isInputError = true;
                            }
                        }
                    }

                    LinkToServer(connectToIp);
                    break;
                }
            }

        }

        static void InputServer()
        {
            while (true)
            {
                WS(new List<string> { "请输入您要连接的IP地址" }, true, false);
                var parts = lastInput.Split('.');
                if (parts.Length != 4)
                {
                    isInputError = true;
                    continue;
                }
                var ipUseful = true;
                foreach (var part in parts)
                {
                    var partInt = part.Int();
                    if (partInt < 0 || partInt > 255)
                    {
                        ipUseful = false;
                    }
                }
                if (ipUseful)
                {
                    LinkToServer(IPAddress.Parse(lastInput));
                    break;
                }
                else
                {
                    isInputError = true;
                }
            }
        }

        static void LinkToServer(IPAddress ip)
        {
            while (true)
            {
                Client client = new Client();
                var isSuccessed = client.StarUp(ip, port);

                if (isSuccessed)
                {
                    WS(new List<string> { "与主机的连接建立完毕。" }, false);

                    Readiness(client);
                    return;
                }
                else
                {
                    WS(new List<string> {
                        "与主机建立连接失败, 报出以上错误。",
                        "",
                        "1. 重试连接",
                        "2. 重新搜索",
                        "3. 强制连接",
                        "*. 取消"
                    }, true, true, false);

                    switch (lastInput)
                    {
                        case "1":
                            break;
                        case "2":
                            SelectServer();
                            return;
                        case "3":
                            InputServer();
                            return;
                        default:
                            isInputError = true;
                            break;
                    }
                }
            }
        }

        static void Readiness(IConnection connection)
        {
            while (true)
            {
                var strSend = ReadLine();
                WriteLine($"您说 : {strSend}");
                if (strSend.ToLower() == ":exit") break;
                connection.Send(strSend);
            }

            ReadKey();
        }

        static void WS(List<string> strs, bool isNeedInput = true, bool isReadKey = true, bool isShowTitle = true)
        {
            if (isShowTitle)
            {
                Clear();
                WriteLine($"本机IP : {ip}, 使用端口 : {port}");
                WriteLine("");
            }
            if (isInputError)
            {
                WriteLine("输入错误, 请重试");
                WriteLine("");
                isInputError = false;
            }
            foreach (var str in strs)
            {
                WriteLine(str);
            }
            WriteLine("");
            if (isNeedInput)
            {
                Write("请输入 : ");
                if (isReadKey)
                {
                    lastInput = ReadKey().KeyChar.ToString();
                    WriteLine("\r\n");
                }
                else
                {
                    lastInput = ReadLine();
                    WriteLine("");
                }
            }
        }
    }

    public interface IConnection
    {
        void Send(string str);
    }

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
                WriteLine(e.Message);

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
                    WriteLine(e.Message);
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
                WriteLine("尚未有客户端连接。\r\n");
                return;
            }
            clientSocket.Send(strByteArr);
        }
    }

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
                WriteLine(e.Message);

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

    public class Tools
    {
        /// <summary>
        /// 获取本地ip地址,优先取内网ip
        /// </summary>
        public static string GetLocalIp()
        {
            var Ips = GetLocalIpAddress();

            foreach (string ip in Ips) if (ip.StartsWith("10.80.")) return ip;
            foreach (string ip in Ips) if (ip.Contains(".")) return ip;

            return "127.0.0.1";
        }

        /// <summary>
        /// 获取本地ip地址。多个ip
        /// </summary>
        public static string[] GetLocalIpAddress()
        {
            var hostName = Dns.GetHostName();                   //获取主机名称  
            var addresses = Dns.GetHostAddresses(hostName);     //解析主机IP地址  

            var IP = new string[addresses.Length];              //转换为字符串形式  
            for (var i = 0; i < addresses.Length; i++) IP[i] = addresses[i].ToString();
            return IP;
        }

        public static List<string> GetIpList()
        {
            var ipList = new List<int>();
            var thisIp = GetLocalIp();
            var ipPart = thisIp.Remove(thisIp.LastIndexOf("."));

            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var ipPingedCount = 0;
            Parallel.For(1, 255, new Action<int>((i) =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                var ping = new Ping();

                var pingIp = $"{ipPart}.{i}";
                var pingRelpy = ping.Send(pingIp, 50);

                WriteLine($"进度: {++ipPingedCount}/255, {decimal.Round(ipPingedCount / 2.55m, 2)}%");
                if (pingRelpy.Status == IPStatus.Success)
                {
                    ipList.Add(i);
                }

            }));

            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            ipList.Sort();
            return ipList.Select(e => $"{ipPart}.{e}").ToList();
        }
    }

    public static class ExtendFunctions
    {
        public static int Int(this object obj)
        {
            try { return Convert.ToInt32(obj); } catch { return 0; }
        }
    }
}
