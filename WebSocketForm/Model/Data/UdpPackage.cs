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
    public class UdpPackage
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        /// <remarks> Guid 类型 </remarks>
        public Guid ID { get; set; }
        /// <summary>
        /// 发送者IP
        /// </summary>
        /// <remarks> IPAddress 类型 </remarks>
        public IPAddress SenderIP { get; set; }
        /// <summary>
        /// 包ID
        /// </summary>
        public int PackageID { get; set; }
        /// <summary>
        /// 包数据
        /// </summary>
        public byte[] PackageData { get; set; }
        /// <summary>
        /// 数据大小
        /// </summary>
        public long DataSize { get; set; }
    }
}
