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
        #region Setting

        public static User Config = new User();

        #endregion

        #region InfoSave

        public static readonly string path = AppDomain.CurrentDomain.BaseDirectory;

        public static Exception SettingSave()
        {
            try
            {
                using (var fs = new FileStream(path + "setting\\setting.dat", FileMode.Create, FileAccess.Write))
                {
                    var bytesData = Config.ToBytes();
                    fs.Write(bytesData, 0, bytesData.Length);
                }

                using (var fs = new FileStream(path + "data\\user.dat", FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < userList.Count; i++)
                    {
                        var bytesData = userList[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);
                    }
                }

                using (var fs = new FileStream(path + "data\\group.dat", FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < groupList.Count; i++)
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

        public static Exception SettingLoad()
        {
            try
            {
                byte[] configBytesData = null;
                using (var fs = new FileStream(path + "setting\\setting.dat", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    configBytesData = new byte[fs.Length];
                    fs.Read(configBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (configBytesData.Length > 0)
                {
                    Config = configBytesData.ToObject<User>();
                }
                else
                {

                }

                byte[] userBytesData = null;
                using (var fs = new FileStream(path + "data\\user.dat", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    userBytesData = new byte[fs.Length];
                    fs.Read(userBytesData, 0, fs.Length.ToIntWidthEx());
                }
                if (userBytesData.Length > 0)
                {
                    var entitySize = new User().ToBytes().Length;
                    if (userBytesData.Length % entitySize != 0)
                    {
                        return new FileFormatException("版本更新致保存文件格式不正确。");
                    }
                    var entityCount = userBytesData.Length / entitySize;
                    for (var i = 0; i < entityCount; i++)
                    {
                        var entityBytes = new byte[entitySize];
                        Array.Copy(userBytesData, 0, entityBytes, i * entitySize, entitySize);
                        AddUser(entityBytes.ToObject<User>());
                    }
                }

                byte[] groupBytesData = null;
                using (var fs = new FileStream(path + "data\\group.dat", FileMode.OpenOrCreate, FileAccess.Read))
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
                    for (var i = 0; i < entityCount; i++)
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

        public static List<Menu> GetMenuList()
        {
            var menuList = new List<Menu>();
            menuList.AddRange(userList);
            menuList.AddRange(groupList);

            return menuList.OrderByDescending(e => e.LastChatTime).ToList();
        }
        #endregion

        #region User
        public static List<User> GetUserList() => userList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 新增一条用户设定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddUser(User value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < userList.Count; i++)
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
                    return true;
                }
            }
            userList.Add(value);
            return false;
        }

        private static readonly List<User> userList = new List<User>();
        #endregion

        #region GroupChat
        public static List<GroupChat> GetGroupChat() => groupList.OrderByDescending(e => e.LastChatTime).ToList();

        public static bool AddGroupChat(GroupChat value)
        {
            if (value == null) throw new ArgumentNullException();
            for (var i = 0; i < groupList.Count; i++)
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
                    return true;
                }
            }
            groupList.Add(value);
            return false;
        }

        private static readonly List<GroupChat> groupList = new List<GroupChat>();
        #endregion

        #region ChatList
        public static List<Chat> GetChatList(IPAddress id)
        {
            return chatList.FindAll(e => e.ToUserChatID.ToString() == id.ToString());
        }
        public static List<Chat> GetChatList(DateTime id)
        {
            return chatList.FindAll(e => e.ToGroupChatID.Ticks == id.Ticks);
        }

        public static Chat GetLastChat(IPAddress id)
        {
            return chatList.FindLast(e => e.ToUserChatID.ToString() == id.ToString());
        }

        public static Chat GetLastChat(DateTime id)
        {
            return chatList.FindLast(e => e.ToGroupChatID.Ticks == id.Ticks);
        }

        public static void EditChatStatus(int index, ChatStatus value)
        {
            chatList[index].ChatStatus = value;
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
