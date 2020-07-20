using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using WebSocketForm.Helper;
using Model.File;

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

            if (Setting.UserConfig == null)
            {
                Setting.UserConfig = new File_User();
            }

            Setting.UserConfig.IP = NetHelper.GetLocalIp().GetAddressBytes();

            if (string.IsNullOrWhiteSpace(Setting.UserConfig.Name))
            {
                CloseFormButton.Visibility = Visibility.Collapsed;
            }

            UserName.Text = Setting.UserConfig.Name;

            if (Setting.UserConfig?.HeadImage != null)
            {
                userHeadImage = ImageHelper.BytesToBitmap(Setting.UserConfig.HeadImage);
            }

            if (userHeadImage != null)
            {
                RefreshHeadImage();
            }
        }

        private void SelectHeadImage(object sender, MouseButtonEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "所有图片|*.bmp;*.jpeg;*.jpg;*.png|Windows位图(*.bmp)|*.bmp|JPEG格式(*.jpg)|*.jpg;*.jpeg|便携式网络图形(*.png)|*.png",
                RestoreDirectory = false
            };
            ofd.ShowDialog();
            if (string.IsNullOrWhiteSpace(ofd.FileName))
            {
                return;
            }

            var headImagePath = $@"{AppData.PATH}setting\\HeadImage.png";
            using (var image = new Bitmap(ofd.FileName))
            {
                userHeadImage?.Dispose();
                image.Save(headImagePath, ImageFormat.Png);
                userHeadImage = new Bitmap(headImagePath);

            }

            RefreshHeadImage();
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

            if (string.IsNullOrWhiteSpace(UserName.Text))
            {
                if (MessageBox.Show("用户名为空时会默认使用IP作为用户名, 是否继续?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                {
                    return;
                }
                Setting.UserConfig.Name = new IPAddress(Setting.UserConfig.IP).ToString();
            }
            else
            {
                Setting.UserConfig.Name = UserName.Text;
            }

            if (userHeadImage != null)
            {
                Setting.UserConfig.HeadImage = ImageHelper.BitmapToBytes(userHeadImage);
            }

            var ex = AppData.Save();

            if (ex != null)
            {
                MessageBox.Show($@"{ex.Message}
{ex.StackTrace}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Close();
            }
        }

        private void RefreshHeadImage()
        {
            UserHeadImage.Background = new ImageBrush(ImageHelper.BitmapToBitmapImage(userHeadImage));
        }
    }
}
