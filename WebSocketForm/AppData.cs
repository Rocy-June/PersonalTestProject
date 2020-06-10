using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketForm.Function;
using WebSocketForm.Helper;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.Model.File;
using WebSocketForm.Model.View;

namespace WebSocketForm
{
    static class AppData
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

        /// <summary>
        /// 用户列表
        /// </summary>
        public static readonly List<User> UserList = new List<User>();

        /// <summary>
        /// 群列表
        /// </summary>
        public static readonly List<GroupChat> GroupList = new List<GroupChat>();


        #region 存档文件读取与保存

        /// <summary>
        /// 保存设定
        /// </summary>
        /// <returns>保存错误信息</returns>
        public static Exception Save()
        {
            try
            {
                IoHelper.PathCheckAndCreate(SETTING_DATA_URI);
                using (var fs = new FileStream(SETTING_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    var bytesData = Setting.UserConfig.ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
                }

                IoHelper.PathCheckAndCreate(SETTING_USER_DATA_URI);
                using (var fs = new FileStream(SETTING_USER_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    var bytesData = UserList.Select(e => ModelHelper.ViewUserToFileUser(e)).ToList().ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
                }

                IoHelper.PathCheckAndCreate(SETTING_GROUP_DATA_URI);
                using (var fs = new FileStream(SETTING_GROUP_DATA_URI, FileMode.Create, FileAccess.Write))
                {
                    var bytesData = GroupList.Select(e => ModelHelper.ViewGroupChatToFileGroupChat(e)).ToList().ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
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
                IoHelper.PathCheckAndCreate(SETTING_DATA_URI);
                using (var fs = new FileStream(SETTING_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var streamBytes = new byte[fs.Length];
                    fs.Read(streamBytes, 0, fs.Length.ToIntWithEx());
                    if (streamBytes.Length > 0)
                    {
                        Setting.UserConfig = streamBytes.ToObject<File_User>();
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(SETTING_DATA_URI);
                MessageBox.Show($@"用户设定存档出错并已将其重置，请重新设定
{ex.Message}
{ex.StackTrace}");
            }

            if (Setting.UserConfig != null )
            {
                if (File.Exists(HEAD_IMAGE_URI))
                {
                    using (var bm = new Bitmap(HEAD_IMAGE_URI))
                    {
                        Setting.UserConfig.HeadImage = ImageHelper.BitmapToBytes(bm);
                    }
                }

                Setting.UserConfig.IP = NetHelper.GetLocalIp()?.GetAddressBytes();
            }
            
            try
            {
                IoHelper.PathCheckAndCreate(SETTING_USER_DATA_URI);
                using (var fs = new FileStream(SETTING_USER_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var streamBytes = new byte[fs.Length];
                    fs.Read(streamBytes, 0, fs.Length.ToIntWithEx());
                    if (streamBytes.Length > 0)
                    {
                        var dataList = streamBytes.ToObject<List<File_User>>();
                        foreach (var user in dataList)
                        {
                            AddUser(ModelHelper.FileUserToViewUser(user));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(SETTING_USER_DATA_URI);
                MessageBox.Show($@"通讯用户集存档出错并已将其重置
{ex.Message}
{ex.StackTrace}");
            }

            try
            {
                IoHelper.PathCheckAndCreate(SETTING_GROUP_DATA_URI);
                using (var fs = new FileStream(SETTING_GROUP_DATA_URI, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var streamBytes = new byte[fs.Length];
                    fs.Read(streamBytes, 0, fs.Length.ToIntWithEx());
                    if (streamBytes.Length > 0)
                    {
                        var dataList = streamBytes.ToObject<List<File_GroupChat>>();
                        foreach (var group in dataList)
                        {
                            AddGroupChat(ModelHelper.FileGroupChatToViewGroupChat(group));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(SETTING_GROUP_DATA_URI);
                MessageBox.Show($@"通讯用户群集存档出错并已将其重置
{ex.Message}
{ex.StackTrace}");
            }
        }

        #endregion

        #region MenuList

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>菜单列表</returns>
        public static List<Menu> GetMenuList()
        {
            var menuList = new List<Menu>();
            menuList.AddRange(UserList);
            menuList.AddRange(GroupList);

            return menuList.OrderByDescending(e => e.LastChatTime).ToList();
        }

        #endregion

        #region User

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        public static List<User> GetUserList() => UserList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 新增一条用户设定
        /// </summary>
        /// <param name="value">用户设定</param>
        /// <returns>true: 添加成功; false: 覆盖成功</returns>
        public static bool AddUser(User value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < UserList.Count; ++i)
            {
                if (UserList[i].IP.ToString() == value.IP.ToString())
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(UserList[i], data);
                            }
                        }
                    }
                    return false;
                }
            }
            UserList.Add(value);
            return true;
        }

        #endregion

        #region GroupChat

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns>群列表</returns>
        public static List<GroupChat> GetGroupChat() => GroupList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 添加群信息
        /// </summary>
        /// <param name="value">群信息</param>
        /// <returns>true: 添加成功; false: 覆盖成功</returns>
        public static bool AddGroupChat(GroupChat value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < GroupList.Count; ++i)
            {
                if (GroupList[i].ID.Ticks == value.ID.Ticks)
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(GroupList[i], data);
                            }
                        }
                    }
                    return false;
                }
            }
            GroupList.Add(value);
            return true;
        }

        #endregion

        #region ChatList

        private static readonly List<Chat> chatList = new List<Chat>();

        /// <summary>
        /// 获取个人聊天内容
        /// </summary>
        /// <param name="id">个人IP</param>
        /// <returns></returns>
        public static List<Chat> GetChatList(IPAddress id)
        {
            return chatList.FindAll(e => e.ReceiverIP.ToString() == id.ToString());
        }

        /// <summary>
        /// 获取群聊天内容
        /// </summary>
        /// <param name="id">群创建时间</param>
        /// <returns></returns>
        public static List<Chat> GetChatList(DateTime id)
        {
            return chatList.FindAll(e => e.ToGroupChatID.Ticks == id.Ticks);
        }

        /// <summary>
        /// 获取最后一条个人消息记录
        /// </summary>
        /// <param name="id">个人IP</param>
        /// <returns></returns>
        public static Chat GetLastChat(IPAddress id)
        {
            return chatList.FindLast(e => e.ReceiverIP.ToString() == id.ToString());
        }

        /// <summary>
        /// 获取最后一条群组消息记录
        /// </summary>
        /// <param name="id">群创建时间</param>
        /// <returns></returns>
        public static Chat GetLastChat(DateTime id)
        {
            return chatList.FindLast(e => e.ToGroupChatID.Ticks == id.Ticks);
        }

        /// <summary>
        /// 修改聊天状态
        /// </summary>
        /// <param name="index">ID</param>
        /// <param name="value"></param>
        public static void EditChatStatus(int index, MessageStatus value)
        {
            chatList[index].MessageStatus = value;
        }

        public static int AddChat(Chat value)
        {
            chatList.Add(value);
            return chatList.Count - 1;
        }

        #endregion

        #region 刷新用户状态

        public static void UserStatusRefresh()
        {
            while (true)
            {
                Thread.Sleep(60000);
                var now = DateTime.Now;
                var timeOut = new TimeSpan(90000);
                for (int i = 0; i < UserList.Count; ++i)
                {
                    if (now - UserList[i].LastResponsedTime > timeOut)
                    {
                        UserList[i].OnlineStatus = OnlineStatus.Offline;
                    }
                }
            }
        }

        #endregion
    }
}
