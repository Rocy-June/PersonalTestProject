using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.View;

namespace WebSocketForm.Function
{
    class ServerEvent
    {
        public static void LocalServer_LoginReceived(PostInfo data, IPAddress ip)
        {
            var userData = (Data_User)data.Data;

            AppData.AddUser(ModelHelper.DataUserToViewUser(userData));
            MainWindow.CurrentWindow.RefreshMenu();
        }

        public static void LocalServer_LogoutReceived(PostInfo data, IPAddress ip)
        {
            var userData = (Data_User)data.Data;

            AppData.AddUser(ModelHelper.DataUserToViewUser(userData));
            MainWindow.CurrentWindow.RefreshMenu();
        }
    }
}
