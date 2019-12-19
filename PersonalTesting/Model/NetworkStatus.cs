using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortPinger.Model
{
    class NetworkStatus
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip_Part">子网段号</param>
        public NetworkStatus(int ip_Part)
        {
            Visible = false;
            UserHidden = false;
            IP_Part = ip_Part;

            DelayLog = new List<long>();
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 是否用户隐藏
        /// </summary>
        public bool UserHidden { get; set; }

        /// <summary>
        /// 子网段号
        /// </summary>
        public int IP_Part { get; private set; }

        /// <summary>
        /// 完整IP地址
        /// </summary>
        public string IP
        {
            get
            {
                return Setting.NetworkSegment + IP_Part;
            }
        }

        /// <summary>
        /// 计算机名称
        /// </summary>
        public string PC_Name { get; private set; }

        /// <summary>
        /// 主机昵称
        /// </summary>
        public string HostNickName
        {
            get
            {
                var name = Setting.GetDeviceNickName(PC_Name)?.NickName ?? string.Empty;
                string result;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    result = name + "(" + IP_Part + ")";
                }
                else if (!string.IsNullOrWhiteSpace(PC_Name))
                {
                    result = PC_Name + "(" + IP_Part + ")";
                }
                else
                {
                    result = IP;
                }
                return result;
            }
        }

        /// <summary>
        /// 延时时长记录
        /// </summary>
        private List<long> DelayLog { get; set; }

        /// <summary>
        /// 是否掉线或不在线
        /// </summary>
        public bool IsDisconnect
        {
            get
            {
                if (DelayLog.Count > 5)
                {
                    for (int i = 1; i <= 5; ++i)
                    {
                        var thisDelay = DelayLog[DelayLog.Count - i];
                        if (thisDelay < Ping_Handler.TIMEOUT && thisDelay >= 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 最后延时
        /// </summary>
        public long Delay
        {
            get
            {
                return DelayLog.Last();
            }
            set
            {
                if (DelayLog.Count > Setting.MaxLogCount)
                {
                    DelayLog.RemoveRange(0, DelayLog.Count - Setting.MaxLogCount + 1);
                }
                DelayLog.Add(value);
            }
        }

        /// <summary>
        /// 最后延时的用户显示文字
        /// </summary>
        public string DelayText
        {
            get
            {
                var nowDelay = Delay;

                if (nowDelay < 0)
                {
                    return "Error";
                }
                else if (IsDisconnect)
                {
                    return "Disconnect";
                }
                else if (nowDelay > Ping_Handler.TIMEOUT)
                {
                    return "Timeout";
                }
                else
                {
                    return nowDelay + "ms";
                }
            }
        }

        /// <summary>
        /// 最后延时文字的显示颜色
        /// </summary>
        public Color DelayTextColor
        {
            get
            {
                var nowDelay = Delay;

                if (nowDelay < 0 || nowDelay > Ping_Handler.TIMEOUT || IsDisconnect)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.Black;
                }
            }
        }

        /// <summary>
        /// 最后延时对应的块颜色
        /// </summary>
        public Color DelayColor
        {
            get
            {
                return GetColor(Delay);
            }
        }

        /// <summary>
        /// 时间段内的平均延时
        /// </summary>
        public long Delay_Avg
        {
            get
            {
                return DelayLog.Average().ToInt();
            }
        }

        /// <summary>
        /// 平均延时的用户显示文字
        /// </summary>
        public string DelayText_Avg
        {
            get
            {
                if (IsDisconnect)
                {
                    return "--";
                }
                else
                {
                    return Delay_Avg + "ms";
                }
            }
        }

        /// <summary>
        /// 平均延时文字的显示颜色
        /// </summary>
        public Color DelayTextColor_Avg
        {
            get
            {
                var nowDelay_Avg = Delay_Avg;

                if (nowDelay_Avg < 0 || nowDelay_Avg > Ping_Handler.TIMEOUT || IsDisconnect)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.Black;
                }
            }
        }

        /// <summary>
        /// 平均延时的对应的块颜色
        /// </summary>
        public Color DelayColor_Avg
        {
            get
            {
                return GetColor(Delay_Avg);
            }
        }

        /// <summary>
        /// 检测线程
        /// </summary>
        public Thread DetectingThread { get; set; }




        /// <summary>
        /// 获取延时对应颜色
        /// </summary>
        /// <param name="delay">延时</param>
        /// <returns>颜色</returns>
        private Color GetColor(long delay)
        {
            if (delay < 50)
            {
                return Color.Cyan;
            }
            else if (delay < 100)
            {
                return Color.Green;
            }
            else if (delay < 200)
            {
                return Color.Orange;
            }
            else if (delay < 500)
            {
                return Color.Brown;
            }
            else if (delay < 0 || delay > Ping_Handler.TIMEOUT)
            {
                return Color.Black;
            }
            else
            {
                return Color.Red;
            }
        }

        /// <summary>
        /// 从存档文件中读取记录
        /// </summary>
        /// <returns></returns>
        public bool LoadFromSaveFile()
        {
            try
            {
                var section = Setting.NetworkSegment + IP_Part;
                PC_Name = INI_Handler.Read(section, "PC_Name", INI_File.SaveFile);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取设备名称对应的昵称
        /// </summary>
        public string GetDeviceName()
        {
            PC_Name = IP_Handler.GetDeviceName(IP);
            return PC_Name;
        }

    }
}
