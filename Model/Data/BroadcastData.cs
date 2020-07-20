using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model.Enum;

namespace Model.Data
{
    [Serializable]
    public class BroadcastData
    {
        /// <summary>
        /// 广播类型
        /// </summary>
        public BroadcastType Action { get; set; }
        /// <summary>
        /// 发送者IP
        /// </summary>
        public IPAddress IP { get; set; }
    }
}
