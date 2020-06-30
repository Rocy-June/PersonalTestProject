using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.Enum
{
    public enum TcpMessageType
    {
        /// <summary>
        /// 文字消息
        /// </summary>
        TextMessage = 0,

        /// <summary>
        /// 文件消息
        /// </summary>
        File = 1,
    }
}
