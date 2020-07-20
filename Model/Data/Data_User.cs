using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model.Data
{
    [Serializable]
    public class Data_User
    {
        /// <summary>
        /// 用户IP
        /// </summary>
        public byte[] IP { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public byte[] HeadImage { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 在线状态
        /// </summary>
        public int OnlineStatus { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public long RequestTime { get; set; }
    }
}
