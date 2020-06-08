using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Helper;
using WebSocketForm.Model.Data;
using WebSocketForm.Model.Enum;
using WebSocketForm.Model.View;

namespace WebSocketForm
{
    static class AppData
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public static readonly List<Data_User> UserList = new List<Data_User>();

        /// <summary>
        /// 群列表
        /// </summary>
        public static readonly List<Data_GroupChat> GroupList = new List<Data_GroupChat>();



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
        public static List<Data_GroupChat> GetGroupChat() => GroupList.OrderByDescending(e => e.LastChatTime).ToList();

        /// <summary>
        /// 添加群信息
        /// </summary>
        /// <param name="value">群信息</param>
        /// <returns>true: 添加成功; false: 覆盖成功</returns>
        public static bool AddGroupChat(Data_GroupChat value)
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
    }
}
