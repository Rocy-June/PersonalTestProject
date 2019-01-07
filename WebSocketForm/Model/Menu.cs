using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;

namespace WebSocketForm.Model
{
    public abstract class Menu
    {
        /// <summary>
        /// 头像
        /// </summary>
        public Bitmap HeadImage { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 最后一条消息
        /// </summary>
        public abstract string LastSay { get; set; }
        /// <summary>
        /// 最后一条消息的时间
        /// </summary>
        public abstract DateTime LastChatTime { get; set; }
        /// <summary>
        /// 最后一条消息的时间字符串(HH:mm)
        /// </summary>
        public abstract string LastTimeStr { get; }
        /// <summary>
        /// 状态图标列表
        /// </summary>
        public List<IconFont> Status { get; set; }
        /// <summary>
        /// 状态图标列表输出的文字
        /// </summary>
        public abstract string StatusStr { get; }
    }
}
