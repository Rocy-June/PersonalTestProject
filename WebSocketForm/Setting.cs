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
using WebSocketForm.Model.Data;

namespace WebSocketForm
{
    public class Setting
    {

        #region Consts

        /// <summary>
        /// 运行路径
        /// </summary>
        public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string HEAD_IMAGE_URI = PATH + "setting\\HeadImage.png";
        
        public static readonly string SETTING_DATA_URI = PATH + "setting\\setting.dat";
        
        public static readonly string SETTING_USER_DATA_URI = PATH + "setting\\user.dat";
        
        public static readonly string SETTING_GROUP_DATA_URI = PATH + "setting\\group.dat";

        #endregion

        #region Setting

        /// <summary>
        /// 用户设定
        /// </summary>
        public static Data_User UserConfig = null;

        #endregion


        #region 存档文件读取与保存

        /// <summary>
        /// 保存设定
        /// </summary>
        /// <returns>保存错误信息</returns>
        public static Exception Save()
        {
            try
            {
                IoHelper.PathCheckAndCreate(Setting.SETTING_DATA_URI);
                using (var fs = new FileStream(SETTING_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    var bytesData = UserConfig.ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
                }

                IoHelper.PathCheckAndCreate(SETTING_USER_DATA_URI);
                using (var fs = new FileStream(SETTING_USER_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < UserList.Count; ++i)
                    {
                        var bytesData = UserList[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);
                    }
                }

                IoHelper.PathCheckAndCreate(SETTING_GROUP_DATA_URI);
                using (var fs = new FileStream(SETTING_GROUP_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < GroupList.Count; ++i)
                    {
                        var bytesData = GroupList[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        /// <summary>
        /// 读取设定
        /// </summary>
        /// <returns>读取错误信息</returns>
        public static void Load()
        {
            try
            {
                FileStream fs = null;
                var userEntitySize = new Data_User().ToBytes().Length;
                var groupEneitySize = new Data_GroupChat().ToBytes().Length;

                byte[] configBytesData = null;
                IoHelper.PathCheckAndCreate(SETTING_DATA_URI);
                using (fs = fs ?? new FileStream(SETTING_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    configBytesData = new byte[fs.Length];
                    fs.Read(configBytesData, 0, fs.Length.ToIntWithEx());
                }
                if (configBytesData.Length > 0)
                {
                    UserConfig = configBytesData.ToObject<Data_User>();
                }
                fs = null;

                if (File.Exists(HEAD_IMAGE_URI) && UserConfig != null)
                {
                    using (var bm = new Bitmap(HEAD_IMAGE_URI))
                    {
                        UserConfig.HeadImage = ImageHelper.BitmapToBytes(bm);
                    }
                }

                byte[] userBytesData = null;
                IoHelper.PathCheckAndCreate(SETTING_USER_DATA_URI);
                using (fs = fs ?? new FileStream(SETTING_USER_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    userBytesData = new byte[fs.Length];
                    fs.Read(userBytesData, 0, fs.Length.ToIntWithEx());
                }
                if (userBytesData.Length > 0)
                {
                    var entityCount = userBytesData.Length / userEntitySize;
                    for (var i = 0; i < entityCount; ++i)
                    {
                        var entityBytes = new byte[userEntitySize];
                        Array.Copy(userBytesData, 0, entityBytes, i * userEntitySize, userEntitySize);
                        AddUser(entityBytes.ToObject<Data_User>());
                    }
                }
                fs = null;

                byte[] groupBytesData = null;
                IoHelper.PathCheckAndCreate(SETTING_GROUP_DATA_URI);
                using (fs = fs ?? new FileStream(SETTING_GROUP_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    groupBytesData = new byte[fs.Length];
                    fs.Read(groupBytesData, 0, fs.Length.ToIntWithEx());
                }
                if (groupBytesData.Length > 0 && groupBytesData.Length % groupEneitySize == 0)
                {
                    var entityCount = groupBytesData.Length / groupEneitySize;
                    for (var i = 0; i < entityCount; ++i)
                    {
                        var entityBytes = new byte[groupEneitySize];
                        Array.Copy(groupBytesData, 0, entityBytes, i * groupEneitySize, groupEneitySize);
                        AddGroupChat(entityBytes.ToObject<Data_GroupChat>());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"{ex.Message}
{ex.StackTrace}", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #endregion

        #region UserStatusRefresh

        public static void UserStatusRefresh()
        {
            while (true)
            {
                Thread.Sleep(60000);
                var now = DateTime.Now;
                var timeOut = new TimeSpan(90000);
                for (int i = 0; i < UserList.Count; ++i)
                {
                    if (now - UserList[i].LastRespondTime > timeOut)
                    {
                        UserList[i].OnlineStatus = OnlineStatus.Offline;
                    }
                }
            }
        }

        #endregion

    }
}
