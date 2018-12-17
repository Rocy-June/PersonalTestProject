using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;

namespace WebSocketForm.Model
{
    public class MessageListModel
    {
        public MessageListModel()
        {
            LastTime = new DateTime();
            Status = new List<IconFont>();

            IP = null;
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 返回IsTop的可见属性字符串
        /// </summary>
        public string IsTopVisibilityStr { get { return IsTop ? "Visible" : "Hidden"; } }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                var title = "";
                if (IP != null)
                {
                    var settedNickName = Setting.GetNickNames().Find(e => e.IP == IP);
                    if (settedNickName != null)
                    {
                        title = settedNickName.Name;
                    }
                    else
                    {
                        title = IP.ToString();
                    }
                }
                else
                {
                    title = _Title;
                }
                return title;
            }

            set => _Title = value;
        }
        private string _Title;
        /// <summary>
        /// 最后一条消息的时间
        /// </summary>
        public DateTime LastTime { get; set; }
        /// <summary>
        /// 最后一条消息的时间字符串(HH:mm)
        /// </summary>
        public string LastTimeStr { get { return LastTime.ToString("HH:mm"); } }
        /// <summary>
        /// 最后一条消息
        /// </summary>
        public string LastSay { get; set; }
        /// <summary>
        /// 状态图标列表
        /// </summary>
        public List<IconFont> Status { get; set; }
        /// <summary>
        /// 状态图标列表输出的文字
        /// </summary>
        public string StatusStr { get { return string.Join("", Status.Select(e => IconFontChar.Get(e))); } }

        /// <summary>
        /// 如果是用户, 则为用户的IP; 如果不是用户, 则为null
        /// </summary>
        public IPAddress IP { get; set; }
    }
}
