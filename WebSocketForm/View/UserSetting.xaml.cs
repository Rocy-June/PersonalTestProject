using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebSocketForm.Function;
using WebSocketForm.Model;

namespace WebSocketForm.View
{
    /// <summary>
    /// UserSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UserSetting : Window
    {
        Bitmap userHeadImage = null;

        public UserSetting()
        {
            InitializeComponent();

            if (Setting.Config == null)
            {
                CloseFormButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SelectHeadImage(object sender, MouseButtonEventArgs e)
        {
            var selectedHeadImage = new OpenFileDialog()
            {
                Filter = "所有图片|*.bmp;*.jpeg;*.jpg;*.png|Windows位图(*.bmp)|*.bmp|JPEG格式(*.jpg)|*.jpg;*.jpeg|便携式网络图形(*.png)|*.png|",
                RestoreDirectory = false
            };
        }

        private void UserNameBorderMouseLeave(object sender, MouseEventArgs e)
        {
            var obj = (Border)sender;
            if (!obj.Child.IsFocused)
            {
                var storyBoard = (Storyboard)FindResource("MouseLeaveResume");
                obj.BeginStoryboard(storyBoard);
            }
        }

        private void SaveUserInfoClick(object sender, RoutedEventArgs e)
        {
            if (Setting.Config == null)
            {
                Setting.Config = new User();
            }

            Setting.Config.IP = IPAddress.Parse(SocketTool.GetLocalIp());
            Setting.Config.HeadImage = userHeadImage;
            Setting.Config.Name = UserName.Text;
        }
    }
}
