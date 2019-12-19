using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketForm.Function;
using WebSocketForm.Model;
using WebSocketForm.Model.Enum;
using WebSocketForm.View;
using System.Runtime.Serialization;

namespace WebSocketForm
{
    public class Setting
    {
        #region Setting

        /// <summary>
        /// 运行路径
        /// </summary>
        public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 用户设定
        /// </summary>
        public static User Config = null;

        /// <summary>
        /// 用户列表
        /// </summary>
        private static readonly List<User> userList = new List<User>();

        /// <summary>
        /// 群列表
        /// </summary>
        private static readonly List<GroupChat> groupList = new List<GroupChat>();

        #endregion

        #region InfoSave

        /// <summary>
        /// 保存设定
        /// </summary>
        /// <returns>保存错误信息</returns>
        public static Exception SettingSave()
        {
            try
            {


                using (var fs = new FileStream(PATH + "setting\\setting.dat", FileMode.Create, FileAccess.Write))
                {
                    var bytesData = Config.ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
                }

                using (var fs = new FileStream(PATH + "data\\user.dat", FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < userList.Count; ++i)
                    {
                        var bytesData = userList[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);
                    }
                }

                using (var fs = new FileStream(PATH + "data\\group.dat", FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < groupList.Count; ++i)
                    {
                        var bytesData = groupList[i].ToBytes();
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
        public static Exception SettingLoad()
        {
            try
            {
                byte[] configBytesData = null;
                var userEntitySize = new User().ToBytes().Length;

                FileStream fs = null;
                if (!File.Exists(PATH + "setting\\setting.dat"))
                {
                    if (!Directory.Exists(PATH + "setting\\"))
                    {
                        Directory.CreateDirectory(PATH + "setting\\");
                    }
                    fs = File.Create(PATH + "setting\\setting.dat");
                }
                using (fs = fs == null ? new FileStream(PATH + "setting\\setting.dat", FileMode.OpenOrCreate, FileAccess.Read) : fs)
                {
                    configBytesData = new byte[fs.Length];
                    fs.Read(configBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (configBytesData.Length > 0)
                {
                    if (configBytesData.Length % userEntitySize != 0)
                    {
                        return new FileFormatException("版本更新致保存文件格式不正确。");
                    }
                    Config = configBytesData.ToObject<User>();
                }
                else
                {
                    Application.Current.Run();
                    Application.Current.Shutdown();
                }

                byte[] userBytesData = null;
                if (!File.Exists(PATH + "setting\\user.dat"))
                {
                    fs = File.Create(PATH + "setting\\user.dat");
                }
                using (fs = new FileStream(PATH + "data\\user.dat", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    userBytesData = new byte[fs.Length];
                    fs.Read(userBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (userBytesData.Length > 0)
                {
                    if (userBytesData.Length % userEntitySize != 0)
                    {
                        return new FileFormatException("版本更新致保存文件格式不正确。");
                    }
                    var entityCount = userBytesData.Length / userEntitySize;
                    for (var i = 0; i < entityCount; ++i)
                    {
                        var entityBytes = new byte[userEntitySize];
                        Array.Copy(userBytesData, 0, entityBytes, i * userEntitySize, userEntitySize);
                        AddUser(entityBytes.ToObject<User>());
                    }
                }

                byte[] groupBytesData = null;
                if (!File.Exists(PATH + "setting\\group.dat"))
                {
                    fs = File.Create(PATH + "setting\\group.dat");
                }
                using (fs = new FileStream(PATH + "data\\group.dat", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    groupBytesData = new byte[fs.Length];
                    fs.Read(groupBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (groupBytesData.Length > 0)
                {
                    var entitySize = new GroupChat().ToBytes().Length;
                    if (groupBytesData.Length % entitySize != 0)
                    {
                        return new FileFormatException("版本更新致保存文件格式不正确。");
                    }
                    var entityCount = groupBytesData.Length / entitySize;
                    for (var i = 0; i < entityCount; ++i)
                    {
                        var entityBytes = new byte[entitySize];
                        Array.Copy(groupBytesData, 0, entityBytes, i * entitySize, entitySize);
                        AddGroupChat(entityBytes.ToObject<GroupChat>());
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
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
            menuList.AddRange(userList);
            menuList.AddRange(groupList);

            return menuList.OrderByDescending(e => e.LastChatTime).ToList();
        }

        #endregion

        #region User

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        public static List<User> GetUserList() => userList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 新增一条用户设定
        /// </summary>
        /// <param name="value">用户设定</param>
        /// <returns>true: 添加成功; false: 覆盖成功</returns>
        public static bool AddUser(User value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < userList.Count; ++i)
            {
                if (userList[i].IP.ToString() == value.IP.ToString())
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
                    return false;
                }
            }
            userList.Add(value);
            return true;
        }

        #endregion

        #region GroupChat

        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns>群列表</returns>
        public static List<GroupChat> GetGroupChat() => groupList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 添加群信息
        /// </summary>
        /// <param name="value">群信息</param>
        /// <returns>true: 添加成功; false: 覆盖成功</returns>
        public static bool AddGroupChat(GroupChat value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < groupList.Count; ++i)
            {
                if (groupList[i].ID.Ticks == value.ID.Ticks)
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(groupList[i], data);
                            }
                        }
                    }
                    return false;
                }
            }
            groupList.Add(value);
            return true;
        }

        #endregion

        #region ChatList

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

        private static readonly List<Chat> chatList = new List<Chat>();
        #endregion

        #region UserStatusRefresh

        public static void UserStatusRefresh()
        {
            while (true)
            {
                Thread.Sleep(60000);
                var now = DateTime.Now;
                var timeOut = new TimeSpan(90000);
                for (int i = 0; i < userList.Count; ++i)
                {
                    if (now - userList[i].LastRespondTime > timeOut)
                    {
                        userList[i].OnlineStatus = OnlineStatus.Offline;
                    }
                }
            }
        }

        #endregion

        #region Model => File

        public static Exception ModelToFile()
        {



            return null;

        }

        #endregion

        #region File => Model

        #endregion

    }
}
