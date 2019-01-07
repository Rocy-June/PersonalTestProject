using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketForm.Function;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model
{
    public class Setting
    {
        #region InfoSave

        public static readonly string path = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly byte[] spliter = new byte[] { 0x00, 0x33, 0x00 };

        public static Exception SettingSave()
        {
            try
            {
                using (var fs = new FileStream(path + "data\\Menu.dat", FileMode.Create, FileAccess.Write))
                {
                    var menu_data = GetMessageList();

                    for (var i = 0; i < menu_data.Count; i++)
                    {
                        var bytesData = menu_data[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);

                        if (i != menu_data.Count - 1)
                        {
                            fs.Write(spliter, 0, spliter.Length);
                        }
                    }

                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        public static Exception SettingLoad()
        {
            try
            {
                byte[] menuBytesData = null;
                using (var fs = new FileStream(path + "data\\Menu.dat", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    menuBytesData = new byte[fs.Length];
                    fs.Read(menuBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (menuBytesData.Length > 0)
                {
                    var thisDataStartIndex = 0;
                    for (int i = 0; i < menuBytesData.Length - 2; i++)
                    {
                        if (menuBytesData[i + 1] == 0x33 && menuBytesData[i] == 0x00 && menuBytesData[i + 2] == 0x00)
                        {
                            var entityBytes = new byte[i - thisDataStartIndex];
                            Array.Copy(menuBytesData, thisDataStartIndex, entityBytes, 0, entityBytes.Length);
                            var savedEntity = entityBytes.ToObject<IMenu>();
                            messageList.Add(savedEntity);
                        }
                    }
                    var lastEntityBytes = new byte[menuBytesData.Length - thisDataStartIndex];
                    Array.Copy(menuBytesData, thisDataStartIndex, lastEntityBytes, 0, lastEntityBytes.Length);
                    var lastSavedEntity = lastEntityBytes.ToObject<IMenu>();
                    AddMessageMenu(lastSavedEntity);
                }
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        #endregion

        #region MessageList
        /// <summary>
        /// 获取菜单设定列表
        /// </summary>
        /// <returns></returns>
        public static List<IMenu> GetMessageList() => messageList.OrderByDescending(e => e.IsTop).ThenByDescending(e => e.LastTime).ToList();

        /// <summary>
        /// 新增一条菜单设定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddMessageMenu(IMenu value)
        {
            for (var i = 0; i < messageList.Count; i++)
            {
                if (messageList[i].IP.Equals(value.IP))
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(messageList[i], data);
                            }
                        }
                    }
                    return true;
                }
            }
            messageList.Add(value);
            return false;
        }

        private static readonly List<Menu> messageList = new List<Menu>();
        #endregion

        #region User
        public static List<User> GetUserList() => userList.OrderByDescending(e => e.LastRespondTime).ToList();

        /// <summary>
        /// 新增一条用户设定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddUser(User value)
        {
            for (var i = 0; i < userList.Count; i++)
            {
                if (userList[i].IP.Equals(value.IP))
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(userList[i], data);
                            }
                        }
                    }
                    return true;
                }
            }
            userList.Add(value);
            return false;
        }

        private static readonly List<User> userList = new List<User>();
        #endregion

        #region UserStatusRefresh

        public static void UserStatusRefresh()
        {
            while (true)
            {
                Thread.Sleep(60000);
                var now = DateTime.Now;
                var timeOut = new TimeSpan(90000);
                for (int i = 0; i < userList.Count; i++)
                {
                    if (now - userList[i].LastRespondTime > timeOut)
                    {
                        userList[i].OnlineStatus = OnlineStatus.Offline;
                    }
                }
            }
        }

        #endregion

    }
}
