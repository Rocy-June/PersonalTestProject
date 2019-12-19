using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortPinger.Model
{
    class DeviceNickName
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="nickName">昵称</param>
        public DeviceNickName(string deviceName, string nickName)
        {
            DeviceName = deviceName;
            NickName = nickName;
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; private set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
