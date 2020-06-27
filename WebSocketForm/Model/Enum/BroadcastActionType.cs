using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Enum
{
    public enum BroadcastActionType
    {
        /// <summary>
        /// 未知传送信息
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 首次登陆消息
        /// </summary>
        Login = 1,
        /// <summary>
        /// 下线消息
        /// </summary>
        Logout = 2,
        /// <summary>
        /// 持续登陆状态
        /// </summary>
        StillOnline = 3,
    }
}
