using System;
using System.Collections.Generic;
using System.IO;
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
        #region InfoSave

        private static readonly byte[] spliter = new byte[] { 0x00, 0x33, 0x00 };

        public static Exception SettingSave()
        {
            try
            {
                using (var fs = new FileStream("\\MessageList.dat", FileMode.Truncate, FileAccess.Write))
                {
                    var messageList_data = GetMessageList();

                    for (var i = 0; i < messageList_data.Count; i++)
                    {
                        var bytesData = messageList_data[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);

                        if (i != messageList_data.Count - 1)
                        {
                            fs.Write(spliter, 0, spliter.Length);
                        }
                    }

                    fs.Flush();
                }

                using (var fs = new FileStream("\\NickNameList.dat", FileMode.Truncate, FileAccess.Write))
                {
                    var nickname_data = GetNickNames();

                    for (var i = 0; i < nickname_data.Count; i++)
                    {
                        var bytesData = nickname_data[i].ToBytes();
                        fs.Write(bytesData, 0, bytesData.Length);

                        if (i != nickname_data.Count - 1)
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
                var messageListData = new List<MessageListModel>();
                byte[] messageListBytesData = null;
                using (var fs = new FileStream("\\MessageList.dat", FileMode.Open, FileAccess.Read))
                {
                    messageListBytesData = new byte[fs.Length];
                    fs.Read(messageListBytesData, 0, fs.Length.ToIntWidthEx());
                }

                var thisDataStartIndex = 0;
                for (int i = 0; i < messageListBytesData.Length - 2; i++)
                {
                    if (messageListBytesData[i] == 0x00 && messageListBytesData[i] == 0x33 && messageListBytesData[i] == 0x00)
                    {
                        var entityBytes = new byte[i - thisDataStartIndex];
                        Array.Copy(messageListBytesData, thisDataStartIndex, entityBytes, 0, entityBytes.Length);
                        var savedEntity = entityBytes.ToObject<MessageListModel>();
                        messageListData.Add(savedEntity);
                    }
                }
                //var entityBytes = new byte[messageListBytesData.Length - thisDataStartIndex];
                //Array.Copy(messageListBytesData, thisDataStartIndex, entityBytes, 0, entityBytes.Length);
                //var savedEntity = entityBytes.ToObject<MessageListModel>();
                //messageListData.Add(savedEntity);



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
        public static List<MessageListModel> GetMessageList() => messageList.OrderByDescending(e => e.IsTop).ThenByDescending(e => e.LastTime).ToList();

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

        private static readonly List<MessageListModel> messageList = new List<MessageListModel>();
        #endregion

        #region NickName
        /// <summary>
        /// 获取昵称设定列表
        /// </summary>
        /// <returns></returns>
        public static List<NickName> GetNickNames() => nickNames;

        /// <summary>
        /// 新增一条昵称设定
        /// </summary>
        /// <param name="value">昵称实体</param>
        /// <returns>该IP是否曾设定过昵称</returns>
        public static bool AddNickName(NickName value)
        {
            for (var i = 0; i < nickNames.Count; i++)
            {
                if (nickNames[i].IP.Equals(value.IP))
                {
                    foreach (var prop in value.GetType().GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var data = prop.GetValue(value);
                            if (data != null)
                            {
                                prop.SetValue(nickNames[i], data);
                            }
                        }
                    }
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
