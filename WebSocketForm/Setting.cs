using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketForm.Helper;
using WebSocketForm.Model;
using WebSocketForm.Model.Enum;
using WebSocketForm.View;
using System.Runtime.Serialization;
using System.Drawing;
using WebSocketForm.Model.File;

namespace WebSocketForm
{
    public static class Setting
    {

        #region Setting

        /// <summary>
        /// 通讯端口
        /// </summary>
        public const int PORT = 8009;

        /// <summary>
        /// 用户设定
        /// </summary>
        public static File_User UserConfig;

        #endregion

    }
}
