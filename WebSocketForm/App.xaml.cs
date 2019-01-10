using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WebSocketForm
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void FormCloseButtonClick(object sender, RoutedEventArgs e)
        {
            var obj = (Button)sender;

            var window = Window.GetWindow(obj);

            window.Close();
        }
    }
}
