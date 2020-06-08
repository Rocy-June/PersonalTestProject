using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WebSocketForm.Enum;

namespace WebSocketForm.Model.View
{
    [Serializable]
    public abstract class Menu
    {
        /// <summary>
        /// 头像
        /// </summary>
        public byte[] HeadImage { get; set; }
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
        public abstract string LastSay { get; }
        /// <summary>
        /// 最后一条消息的时间
        /// </summary>
        public abstract DateTime LastChatTime { get; }
        /// <summary>
        /// 最后一条消息的时间字符串(HH:mm)
        /// </summary>
        public abstract string LastTimeStr { get; }
        /// <summary>
        /// 状态图标列表
        /// </summary>
        private List<IconFont> Status { get; }
        /// <summary>
        /// 添加一个图标状态
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>如果当前状态不存在并添加成功则返回True; 否则返回False</returns>
        public bool AddStatusIcon(IconFont value)
        {
            for (var i = 0; i < Status.Count; ++i)
            {
                if (Status[i] == value)
                {
                    return false;
                }
            }
            Status.Add(value);
            return true;
        }
        /// <summary>
        /// 移除一个图标状态
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>如果包含当前状态并删除成功则返回True; 否则返回False</returns>
        public bool RemoveStatusIcon(IconFont value)
        {
            return Status.Remove(value);
        }
        /// <summary>
        /// 状态图标列表输出的文字
        /// </summary>
        public string StatusStr
        {
            get
            {
                var str = "";
                foreach (var _if in Status)
                {
                    str += IconFontChar.Get(_if);
                }
                return str;
            }
        }
    }
}
