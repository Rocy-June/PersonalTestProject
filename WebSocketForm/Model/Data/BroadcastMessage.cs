using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;

namespace WebSocketForm.Model.Data
{
    [Serializable]
    public class BroadcastMessage
    {
        /// <summary>
        /// 广播类型
        /// </summary>
        public BroadcastActionType Action { get; set; }
        /// <summary>
        /// 发送者IP
        /// </summary>
        public IPAddress IP { get; set; }
    }
}
