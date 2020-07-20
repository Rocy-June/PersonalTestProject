using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    public enum TcpMessageType
    {
        /// <summary>
        /// 事件消息
        /// </summary>
        EventMessage = 0,

        /// <summary>
        /// 文件消息
        /// </summary>
        SendFile = 0x40000000 | 1,
    }
}
