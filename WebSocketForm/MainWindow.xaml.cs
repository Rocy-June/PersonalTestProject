using System;
using System.Collections.Generic;
using System.Linq;
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
using WebSocketForm.Model;

namespace WebSocketForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<OnlineUser> olusers = new List<OnlineUser>();

        public MainWindow()
        {
            InitializeComponent();

            OnlineUserList.Items.Clear();
            OnlineUserList.Items.Add(new { });
            OnlineUserList.Items.Add(new { });
            OnlineUserList.Items.Add(new { });

            for (int i = 0; i < 10; i++)
            {
                Button_Click(addOne, null);
            }
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

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            Thread.Sleep(10);
            var rd = new Random();
            var lif = new List<IconFont>();
            foreach (var _if in Enum.GetValues(typeof(IconFont)))
            {
                if (rd.Next(100) % 2 == 0) lif.Add((IconFont)_if);
            }
            OnlineUserList.Items.Add(new OnlineUser()
            {
                IsTop = rd.Next(100) % 2 == 0 ? true : false,
                Title = $"测试用户{OnlineUserList.Items.Count + 1}",
                LastTime = new DateTime(1, 1, 1, rd.Next(23), rd.Next(59), 0),
                LastSay = $"测试消息来自于 192.168.4.{OnlineUserList.Items.Count + 1}",
                Status = lif
            });
        }

        private void GetListBoxSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                var olUser = (OnlineUser)OnlineUserList.SelectedItem;
                MessageBox.Show(olUser.Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
