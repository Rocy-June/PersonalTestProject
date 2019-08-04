using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model
{
    public class Chat
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 发送者IP
        /// </summary>
        public IPAddress SenderIP { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public ChatType ChatType { get; set; }

        /// <summary>
        /// 接收者IP
        /// </summary>
        public IPAddress ReceiverIP { get; set; }

        /// <summary>
        /// 发送到的群组ID
        /// </summary>
        public DateTime ToGroupChatID { get; set; }

        /// <summary>
        /// 消息信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MessageStatus MessageStatus { get; set; }
    }
}
