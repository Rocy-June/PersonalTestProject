using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketForm.Function;

namespace WebSocketForm.Model
{
    public class Setting
    {

        #region MessageList
        /// <summary>
        /// 获取菜单设定列表
        /// </summary>
        /// <returns></returns>
        public static List<MessageListModel> GetMessageList()
        {
            return messageList;
        }

        /// <summary>
        /// 新增一条菜单设定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddMessageMenu(MessageListModel value)
        {
            for (var i = 0; i < messageList.Count; i++)
            {
                if (messageList[i].IP.Equals(value.IP))
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        var type = prop.PropertyType.Name;
                        var data = prop.GetValue(value);
                        if (type == "string")
                        {
                            if (!string.IsNullOrWhiteSpace((string)data))
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

        private static readonly List<MessageListModel> messageList = new List<MessageListModel>();
        #endregion

        #region NickName
        /// <summary>
        /// 获取昵称设定列表
        /// </summary>
        /// <returns></returns>
        public static List<NickName> GetNickNames()
        {
            return nickNames;
        }

        /// <summary>
        /// 新增一条昵称设定
        /// </summary>
        /// <param name="value">昵称实体</param>
        /// <returns>该IP是否曾设定过昵称</returns>
        public static bool AddNickName(NickName value)
        {
            for (var i = 0; i < nickNames.Count; i++)
            {
                if (nickNames[i].IP == value.IP)
                {
                    nickNames[i].Name = value.Name;
                    return true;
                }
            }
            nickNames.Add(value);
            return false;
        }

        /// <summary>
        /// 删除对应IP的昵称
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>是否找到并删除成功</returns>
        public static bool RemoveNickName(IPAddress ip)
        {
            for (var i = 0; i < nickNames.Count; i++)
            {
                if (nickNames[i].IP == ip)
                {
                    nickNames.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        private static readonly List<NickName> nickNames = new List<NickName>();
        #endregion

    }
}
