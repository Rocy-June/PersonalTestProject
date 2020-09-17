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
    public class BoardcastPackage
    {
        /// <summary>
        /// 广播类型
        /// </summary>
        public BoardcastMessageType Action { get; set; }
        /// <summary>
        /// 发送者IP
        /// </summary>
        public IPAddress IP { get; set; }
    }
}
