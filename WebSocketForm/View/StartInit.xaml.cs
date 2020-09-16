using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebSocketForm.Function;
using WebSocketForm.Helper;
using Model.Data;
using EventHandler;

namespace WebSocketForm.View
{
    /// <summary>
    /// StartInit 的交互逻辑
    /// </summary>
    public partial class StartInit : Window
    {
        private int loadingPart = 0;

        private MainWindow mainWindow;

        private InitFormEvent handler;

        private List<string> loadingText = new List<string>
        {
            "Loading APP settings.",
            "Initializing SQLite connection.",
            "Binding server events.",
            "Opening local server."
        };

        public StartInit()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            new Thread(InitThreadMethod)
            {
                IsBackground = true
            }
            .Start();
        }

        private void InitThreadMethod()
        {
            handler = new InitFormEvent();

            //读取设定
            RefreshText();
            AppData.Load();

            //初始化数据库
            handler.InitSQL();

            //绑定服务器事件
            RefreshText();
            LocalServer.UserProfileDataReceived += LocalServer_UserProfileDataReceived;

            LocalServer.UnknownReceived += LocalServer_UnknownReceived;
            LocalServer.LoginReceived += LocalServer_LoginReceived;
            LocalServer.LogoutReceived += LocalServer_LogoutReceived;
            LocalServer.StillOnlineReceived += LocalServer_StillOnlineReceived;

            //启动本地服务器
            RefreshText();
            LocalServer.OpenLocalServer();

            Invoke(() =>
            {
                Hide();
                mainWindow = new MainWindow();
                mainWindow.Show();
            });
        }

        private void Invoke(Action action)
        {
            Dispatcher.Invoke(action);
        }

        private void RefreshText()
        {
            Invoke(() =>
            {
                LoadingText.Content = $@"( {loadingPart + 1} / {loadingText.Count} ) {loadingText[loadingPart++]}";
            });
        }

        private void LocalServer_UserProfileDataReceived(Data_User data, IPAddress ip)
        {
            Invoke(() =>
            {
                var userData = ModelHelper.DataUserToViewUser(data);
                userData.LastResponsedTime = DateTime.Now;
                AppData.AddUser(userData);
                mainWindow.RefreshMenu();
            });
        }

        private void LocalServer_UnknownReceived(IPAddress ip)
        {

        }

        private void LocalServer_LoginReceived(IPAddress ip)
        {
            TcpMessage.SendUserProfile(ip);
        }

        private void LocalServer_LogoutReceived(IPAddress ip)
        {

        }

        private void LocalServer_StillOnlineReceived(IPAddress ip)
        {

        }
    }
}
