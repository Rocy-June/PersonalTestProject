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
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.Model.View;

namespace WebSocketForm.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 持续在线广播线程
        /// </summary>
        public static Thread StillOnlineBroadcastingThread;
        /// <summary>
        /// 刷新在线状态线程
        /// </summary>
        public static Thread UserStatusRefreshThread;

        public MainWindow()
        {
            InitializeComponent();
            Init();

            if (Setting.UserConfig == null)
            {
                var us = new UserSetting();
                us.ShowDialog();
            }

            StartOnlineThreads();
            LoadedInit();
        }

        #region 过程方法

        private void Init()
        {
            //清空测试用列表项目
            OnlineUserList.Items.Clear();

            //重载控件
            OnlineUserList.ItemsSource = AppData.GetMenuList();
        }

        private void StartOnlineThreads()
        {
            //上线广播
            Broadcast.SendOnlineBroadcasting();

            //持续在线广播线程
            StillOnlineBroadcastingThread = new Thread(Broadcast.SendStillOnlineBroadcasting)
            {
                IsBackground = true
            };
            StillOnlineBroadcastingThread.Start();

            //刷新在线状态线程
            UserStatusRefreshThread = new Thread(AppData.UserStatusRefresh)
            {
                IsBackground = true
            };
            UserStatusRefreshThread.Start();
        }

        private void LoadedInit()
        {
            UserHeadImage.Background = new ImageBrush(ImageHelper.BytesToBitmapImage(Setting.UserConfig.HeadImage));
        }

        #endregion

        #region 接口

        public void RefreshMenu()
        {
            OnlineUserList.ItemsSource = AppData.GetMenuList();
            OnlineUserList.Items.Refresh();
        }

        #endregion

        #region 服务器事件



        #endregion

        #region 窗体基础事件
        private void Window_Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();

            //FrameworkElement c = (FrameworkElement)sender;
            //if (c.Name != "ControlBox")
            //{
            //    DragMove();
            //}
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
            Broadcast.SendOfflineBroadcasting();
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

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random();

            var ip = "192.168.1." + rd.Next(255);

            AppData.AddUser(new User()
            {
                IP = IPAddress.Parse(ip),
                IsTop = rd.Next(100) % 2 == 0 ? true : false,
                NickName = "测试"
            });

            RefreshMenu();
        }

        //Thread t;

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
