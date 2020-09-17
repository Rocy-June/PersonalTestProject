using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Network
{
    [Serializable]
    public class TcpPackage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public TcpMessageType ActionType { get; set; }

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
