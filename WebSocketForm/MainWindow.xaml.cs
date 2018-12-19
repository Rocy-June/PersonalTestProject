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
        public static Thread currentThread;

        public MainWindow()
        {
            InitializeComponent();

            currentThread = Thread.CurrentThread;

            LocalServer.LoginReceived += LocalServer_LoginReceived;
            LocalServer.LogoutReceived += LocalServer_LogoutReceived;
            LocalServer.OpenLocalServer();

            new Thread(() =>
            {
                SocketTool.OnlineBroadcasting();
            })
            { IsBackground = true }.Start();

            OnlineUserList.Items.Clear();
            OnlineUserList.ItemsSource = Setting.GetMessageList();
        }

        #region 服务器事件
        private void LocalServer_LoginReceived(PostInfo<object> data, IPAddress ip)
        {
            Setting.AddMessageMenu(new MessageListModel()
            {
                IP = ip,
                IsTop = false,
                LastSay = "我上线了",
                LastTime = DateTime.Now,
                Status = new List<IconFont>(),
                Title = "测试"
            });

            OnlineUserList.ItemsSource = Setting.GetMessageList();
            OnlineUserList.Items.Refresh();
        }

        private void LocalServer_LogoutReceived(PostInfo<object> data, IPAddress ip)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 窗体基础事件
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
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Setting.SettingSave();
            Setting.SettingLoad();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random();

            var ip = "192.168.4." + rd.Next(255);

            Setting.AddMessageMenu(new MessageListModel()
            {
                IP = IPAddress.Parse(ip),
                IsTop = rd.Next(100) % 2 == 0 ? true : false,
                LastSay = ip + " 我上线了",
                LastTime = DateTime.Now,
                Status = new List<IconFont>(),
                Title = "测试"
            });

            OnlineUserList.ItemsSource = Setting.GetMessageList();
            OnlineUserList.Items.Refresh();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SocketTool.OnlineBroadcasting();
        }
    }
}
