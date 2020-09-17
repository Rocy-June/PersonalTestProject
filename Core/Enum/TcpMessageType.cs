using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public enum TcpMessageType
    {
        /// <summary>
        /// 收到用户个人档案事件
        /// </summary>
        UserProfileDataReceived = 1,

        /// <summary>
        /// 文件消息
        /// </summary>
        SendFile = 0x40000000 | 1,
    }
}
