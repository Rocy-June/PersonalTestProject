using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocketForm.Enum;
using WebSocketForm.Function;
using WebSocketForm.Model;

namespace WebSocketForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            LocalServer.LoginRequestReceived += LocalServer_LoginRequestReceived;
            LocalServer.OpenLocalServer();

            new Thread(() =>
            {
                SocketTool.OnlineBroadcasting();
            })
            { IsBackground = true }.Start();

            OnlineUserList.Items.Clear();
        }

        #region 服务器事件
        private void LocalServer_LoginRequestReceived(PostInfo<object> data, IPAddress ip)
        {
            Setting.AddMessageMenu(new MessageListModel()
            {
                IP = ip,
                IsTop = false,
                LastSay = "我上线了",
                LastTime = new DateTime(),
                Status = new List<IconFont>(),
                Title = "测试"
            });
            OnlineUserList.DataContext = Setting.GetMessageList();
        }
        #endregion

        private void RequestReceived(PostInfo<object> data, IPEndPoint ip)
        {
            MessageBox.Show(ip.ToString());
        }

        private void Window_Drag(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement c = (FrameworkElement)sender;
            if (c.Name != "ControlBox")
            {
                DragMove();
            }
        }

        private void HideApplication(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FullSizeOrMinApplication(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Thread(() =>
            {
                SocketTool.OfflineBroadcasting();
            }).Start();
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuButton_Enter(object sender, MouseEventArgs e)
        {
            var label = (Label)sender;

            if (label.Opacity != 1)
            {
                var storyBord = new Storyboard();
                var opacity = new DoubleAnimation(0.8, new TimeSpan(0, 0, 0, 0, 50));
                Storyboard.SetTarget(opacity, label);
                Storyboard.SetTargetProperty(opacity, new PropertyPath(OpacityProperty));
                storyBord.Children.Add(opacity);

                storyBord.Begin();
            }
        }

        private void MenuButton_Leave(object sender, MouseEventArgs e)
        {
            var label = (Label)sender;

            if (label.Opacity != 1)
            {
                var storyBord = new Storyboard();
                var opacity = new DoubleAnimation(0.6, new TimeSpan(0, 0, 0, 0, 50));
                Storyboard.SetTarget(opacity, label);
                Storyboard.SetTargetProperty(opacity, new PropertyPath(OpacityProperty));
                storyBord.Children.Add(opacity);

                storyBord.Begin();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineUserList.Items.Clear();

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var tcpClient = new TcpClient();
            //var ipep = new IPEndPoint(IPAddress.Broadcast, SocketTool.port);

            var postData = new PostInfo<object>()
            {
                Action = PostActionType.login
            };
            var bytesData = postData.ToBytes();

            tcpClient.ConnectAsync(IPAddress.Broadcast, SocketTool.port);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SocketTool.OnlineBroadcasting();
        }
    }
}
