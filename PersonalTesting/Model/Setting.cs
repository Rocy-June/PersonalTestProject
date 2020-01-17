using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PortPinger.Model
{
    class Setting
    {
        /// <summary>
        /// 用户数据
        /// </summary>
        public static NetworkStatus[] NetworkInfo = new NetworkStatus[PART_COUNT];

        /// <summary>
        /// 机器对应昵称数据
        /// </summary>
        private static List<DeviceNickName> DeviceNamesInfo = new List<DeviceNickName>();

        public static string THIS_COMPUTER_IP;

        #region 默认值

        private static readonly string NetworkSegment_Default = "192.168.1.";

        private static readonly int AvgTimeLength_Default = 5000;

        private static readonly int DetectDelay_Min_Default = 150;

        private static readonly int SlowDownMaxPersonCount_Default = 15;

        private static readonly int SlowDownDelayPerPerson_Default = 10;

        #endregion

        #region 常量

        /// <summary>
        /// 总子网段数量
        /// </summary>
        public const int PART_COUNT = 256;

        /// <summary>
        /// 程序启动目录
        /// </summary>
        public static readonly string APP_START_PATH = Application.StartupPath;

        #endregion

        #region 存档变量

        /// <summary>
        /// 前置网段
        /// </summary>
        public static string NetworkSegment { get; private set; }

        /// <summary>
        /// 测量平均延时总时长
        /// </summary>
        public static int AvgTimeLength { get; set; }

        /// <summary>
        /// 最低两次检测间隔时长
        /// </summary>
        private static int DetectDelay_Min { get; set; }

        /// <summary>
        /// 开始降低检测间隔的最大人数
        /// </summary>
        private static int SlowDownMaxPersonCount { get; set; }

        /// <summary>
        /// 超员后每人增加延时
        /// </summary>
        private static int SlowDownDelayPerPerson { get; set; }

        #endregion

        #region 实时计算变量

        /// <summary>
        /// 两次检测间隔时长 (只读)
        /// </summary>
        /// <remarks>
        /// 为保证同时在线人数过多而网络端口请求过多而加重延迟的情况采用了人数越多延迟越久的方式
        /// </remarks>
        public static int DetectDelay
        {
            get
            {
                var onlineCount = NetworkInfo.Count(e => e.Visible && !e.UserHidden);
                return DetectDelay_Min + (onlineCount > SlowDownMaxPersonCount ? onlineCount : 0) * SlowDownDelayPerPerson;
            }
        }

        /// <summary>
        /// 平均延时测量时长总记录数量
        /// </summary>
        public static int MaxLogCount
        {
            get
            {
                return AvgTimeLength / DetectDelay;
            }
        }

        #endregion






        /// <summary>
        /// 读取设定
        /// </summary>
        public static void Load(bool resetAlert = true)
        {
            var reset2Default = !INI_Handler.CheckFiles();

            #region Network-Segment

            var networkSegment = INI_Handler.Read("Setting", "Network-Segment", INI_File.SaveFile);
            if (!Regex.IsMatch(networkSegment, "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.$"))
            {
                NetworkSegment = NetworkSegment_Default;
                INI_Handler.Write("Setting", "Network-Segment", NetworkSegment, INI_File.SaveFile);
                reset2Default = true;
            }
            else
            {
                NetworkSegment = networkSegment;
            }

            #endregion

            #region Avg-Time-Length

            var avgTimeLength = INI_Handler.Read("Setting", "Avg-Time-Length", INI_File.SaveFile).ToInt();
            if (avgTimeLength < 3000)
            {
                AvgTimeLength = AvgTimeLength_Default;
                INI_Handler.Write("Setting", "Avg-Time-Length", AvgTimeLength.ToString(), INI_File.SaveFile);
            }
            else
            {
                AvgTimeLength = avgTimeLength;
            }

            #endregion

            #region Detect-Delay-Min

            var detectDelay_Min = INI_Handler.Read("Setting", "Detect-Delay-Min", INI_File.SaveFile).ToInt();
            if (detectDelay_Min < 10 || detectDelay_Min > 1000)
            {
                DetectDelay_Min = DetectDelay_Min_Default;
                INI_Handler.Write("Setting", "Detect-Delay-Min", DetectDelay_Min.ToString(), INI_File.SaveFile);
            }
            else
            {
                DetectDelay_Min = detectDelay_Min;
            }

            #endregion

            #region Slow-Down-Max-Person-Count

            var slowDownMaxPersonCount = INI_Handler.Read("Setting", "Slow-Down-Max-Person-Count", INI_File.SaveFile).ToInt();
            if (slowDownMaxPersonCount < 0 || slowDownMaxPersonCount > PART_COUNT)
            {
                SlowDownMaxPersonCount = SlowDownMaxPersonCount_Default;
                INI_Handler.Write("Setting", "Slow-Down-Max-Person-Count", SlowDownMaxPersonCount.ToString(), INI_File.SaveFile);
            }
            else
            {
                SlowDownMaxPersonCount = slowDownMaxPersonCount;
            }

            #endregion

            #region Slow-Down-Delay-Per-Person

            var slowDownDelayPerPerson = INI_Handler.Read("Setting", "Slow-Down-Delay-Per-Person", INI_File.SaveFile).ToInt();
            if (slowDownDelayPerPerson < 0 || slowDownDelayPerPerson > PART_COUNT)
            {
                SlowDownDelayPerPerson = SlowDownDelayPerPerson_Default;
                INI_Handler.Write("Setting", "Slow-Down-Delay-Per-Person", SlowDownDelayPerPerson.ToString(), INI_File.SaveFile);
            }
            else
            {
                SlowDownDelayPerPerson = slowDownDelayPerPerson;
            }

            #endregion



            #region NetworkStatus

            for (int i = 0; i < NetworkInfo.Length; ++i)
            {
                NetworkInfo[i] = new NetworkStatus(i);
                NetworkInfo[i].LoadFromSaveFile();
            }

            #endregion

            #region DeviceNickName

            var lines = 0;
            while (true)
            {
                var deviceName = INI_Handler.Read("DeviceName", "m_" + lines, INI_File.SaveFile);
                var name = INI_Handler.Read("DeviceName", "n_" + lines, INI_File.SaveFile);
                if (string.IsNullOrWhiteSpace(deviceName) && string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                if (Regex.IsMatch(deviceName, "^([0-9A-F]{2}-){5}[0-9A-F]{2}$"))
                {
                    DeviceNamesInfo.Add(new DeviceNickName(deviceName, name));
                }

                ++lines;
            }

            INI_Handler.DeleteSection("DeviceName", INI_File.SaveFile);

            #endregion


            if (resetAlert && reset2Default)
            {
                MessageBox.Show("配置文件出错, 部分设定已重置");
            }
        }

        /// <summary>
        /// 根据设备名称获取对应昵称
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <returns>昵称</returns>
        public static DeviceNickName GetDeviceNickName(string deviceName)
        {
            return DeviceNamesInfo.Find(e => e.DeviceName == deviceName);
        }

        public static void SetDeviceNickName(string deviceName, string nickName)
        {
            var index = DeviceNamesInfo.FindIndex(e => e.DeviceName == deviceName);
            if (index >= 0)
            {
                DeviceNamesInfo[index].NickName = nickName;
                INI_Handler.Write("DeviceName", "n_" + index, nickName, INI_File.SaveFile);
            }
            else
            {
                DeviceNamesInfo.Add(new DeviceNickName(deviceName, nickName));
                var newIndex = DeviceNamesInfo.Count - 1;
                INI_Handler.Write("DeviceName", "m_" + newIndex, deviceName, INI_File.SaveFile);
                INI_Handler.Write("DeviceName", "n_" + newIndex, nickName, INI_File.SaveFile);
            }
        }

    }
}
