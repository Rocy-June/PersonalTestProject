using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model.Data
{
    [Serializable]
    public class TcpMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageActionType ActionType { get; set; }

        /// <summary>
        /// 发送者IP
        /// </summary>
        public IPAddress SenderIP { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}
