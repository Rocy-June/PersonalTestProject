using PortPinger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortPinger
{
    public partial class MainForm : Form
    {
        /*
         * todo list:
         * 
         *      清理离线用户
         *      重连提醒
         *  √   名称ini配置项
         *      固定显示
         *  √   置顶选项
         *  √   把IP记录个人信息改为IP获取MAC记录个人信息
         *      
         *      集体状态缩略图
         *      
         *      强制全部刷新
         *  
         * * * * * * * * * */

        #region 用户界面设定

        /// <summary>
        /// 延时颜色块起始位置
        /// </summary>
        const int COLOR_LOCATION_X = 0;
        /// <summary>
        /// 颜色块纵向大小
        /// </summary>
        const int COLOR_BLOCK_SIZE = 12;
        /// <summary>
        /// 颜色块横向半块大小
        /// </summary>
        const int COLOR_BLOCK_SIZE_HALF = 6;
        /// <summary>
        /// IP标签起始位置
        /// </summary>
        const int IP_LABEL_LOCATION_X = 20;
        /// <summary>
        /// IP标签宽度
        /// </summary>
        const int IP_LABEL_WIDTH = 126;
        /// <summary>
        /// 当前延时标签起始位置
        /// </summary>
        const int NOW_DELAY_LABEL_LOCATION_X = 160;
        /// <summary>
        /// 当前标签宽度
        /// </summary>
        const int NOW_DELAY_LABEL_WIDTH = 66;
        /// <summary>
        /// 平均延时标签起始位置
        /// </summary>
        const int AVG_DELAY_LABEL_LOCATION_X = 240;
        /// <summary>
        /// 平均延时标签宽度
        /// </summary>
        const int AVG_DELAY_LABEL_WIDTH = 66;

        #endregion


        /// <summary>
        /// 正在检测的离线子网段
        /// </summary>
        private int knockDoorPart = 0;

        /// <summary>
        /// 全网段初始化线程
        /// </summary>
        private Thread initThread;
        /// <summary>
        /// 用户界面控制线程
        /// </summary>
        Thread controlThread;
        /// <summary>
        /// 离线敲门检测线程
        /// </summary>
        Thread offlineDetectThread;

        /// <summary>
        /// 在线数量
        /// </summary>
        int onlineCount = 0;

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑重构前  ||  重构后↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        /// <summary>
        /// 信息控件组
        /// </summary>
        private Control.ControlCollection panelControls;

        bool controlsInited = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Setting.Load();
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            panelControls = panel_info.Controls;
            new Thread(() =>
            {
                for (int i = 3; i >= 0; --i)
                {
                    Thread.Sleep(1000);
                    if (!button_Start.Enabled)
                    {
                        Thread.CurrentThread.Abort();
                    }
                    Invoke(new Action(() =>
                    {
                        button_Start.Text = $"开始更新({i})";
                    }));
                }
                button_Start_Click(button_Start, null);
            })
            {
                IsBackground = true
            }
            .Start();
        }

        /// <summary>
        /// 全网段初始化
        /// </summary>
        private void MainThreadInit()
        {
            Invoke(new Action(() =>
            {
                panelControls.Remove(button_Start);
                label_PlzWait.Visible = true;
            }));

            var knockDoorThreads = new List<Thread>();
            for (int i = 0; i < Setting.PART_COUNT; ++i)
            {
                var t = new Thread((arg1) =>
                 {
                     var part = arg1.ToInt();
                     var delay = KnockDoor(part);

                     var name = Setting.NetworkInfo[part].GetDeviceName();

                     if (delay == 0)
                     {
                         Setting.THIS_COMPUTER_IP = Setting.NetworkSegment + part;

                         new Thread(() =>
                         {
                             if (Setting.GetDeviceNickName(name) == null)
                             {
                                 Setting.SetDeviceNickName(name, "本机");
                             }
                         })
                         {
                             IsBackground = true
                         }
                         .Start();
                     }
                 })
                {
                    IsBackground = true
                };
                knockDoorThreads.Add(t);
                t.Start(i);
            }

            initThread = new Thread(() =>
            {
                while (knockDoorThreads.Find(e => e.IsAlive) != null)
                {
                    Thread.Sleep(100);
                }

                controlThread = new Thread(() =>
                {
                    while (true)
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateForm();
                        }));

                        Thread.Sleep(1000);
                    }
                })
                {
                    IsBackground = true
                };
                controlThread.Start();

                offlineDetectThread = new Thread(() =>
                {
                    while (true)
                    {
                        for (int i = 0; i < Setting.PART_COUNT; ++i)
                        {
                            knockDoorPart = i;
                            Invoke(new Action(() =>
                            {
                                try
                                {
                                    ((ProgressBar)panelControls.Find("ProgressBar_KnockDoor", false)[0]).Value = knockDoorPart;
                                }
                                catch { }
                            }));

                            if (!Setting.NetworkInfo[i].IsDisconnect)
                            {
                                continue;
                            }
                            new Thread((arg1) =>
                            {
                                var tmp = arg1.ToInt();
                                KnockDoor(tmp, 3000);
                            })
                            {
                                IsBackground = true
                            }
                            .Start(i);
                            Thread.Sleep(200);
                        }
                    }
                })
                {
                    IsBackground = true
                };
                offlineDetectThread.Start();
            })
            {
                IsBackground = true
            };
            initThread.Start();
        }

        /// <summary>
        /// 离线敲门检测
        /// </summary>
        /// <param name="part">子网段号</param>
        /// <param name="timeout">超时时长</param>
        /// <returns></returns>
        private long KnockDoor(int part, int timeout = Ping_Handler.TIMEOUT)
        {
            //敲门
            var result = Ping_Handler.PingIP(Setting.NetworkSegment + part, timeout);
            //在线
            if (result >= 0 && result <= Ping_Handler.TIMEOUT)
            {
                //初始化网段状态
                Setting.NetworkInfo[part].Delay = result;
                Setting.NetworkInfo[part].Visible = true;

                //启用归属当前网段的实时检测线程
                Setting.NetworkInfo[part].DetectingThread = new Thread(() =>
                {
                    Detecting(part);
                })
                {
                    IsBackground = true
                };
                Setting.NetworkInfo[part].DetectingThread.Start();
            }
            return result;
        }

        /// <summary>
        /// 网段检测 (线程内)
        /// </summary>
        /// <param name="part">子网段号</param>
        private void Detecting(int part)
        {
            while (true)
            {
                //检测
                var delay = Ping_Handler.PingIP(Setting.NetworkSegment + part);

                //超出记录总量删除多余部分并记录
                Setting.NetworkInfo[part].Delay = delay;

                //计算下一次间隔延时
                var sleepTime = Setting.DetectDelay - delay.ToInt();

                if (sleepTime >= 0)
                {
                    //线程等待间隔
                    Thread.Sleep(sleepTime);
                }
            }
        }

        /// <summary>
        /// 更新用户界面
        /// </summary>
        private void UpdateForm()
        {
            //统计曾经总在线人数
            var nowOnlineCount = Setting.NetworkInfo.Count(e => e.Visible);
            //上次人数
            var lastCount = onlineCount;
            //记录历史在线人数
            onlineCount = nowOnlineCount;

            //在线人数发生变化则重置显示界面并更新
            if (nowOnlineCount != lastCount || !controlsInited)
            {
                ResetInformation();

                controlsInited = true;
            }
            if (nowOnlineCount != 0)
            {
                UpdateInformation();
            }
        }

        /// <summary>
        /// 重置用户界面
        /// </summary>
        private void ResetInformation()
        {
            //清空界面控件
            panelControls.Clear();

            //新建标题控件
            panelControls.Add(new Label()
            {
                Location = GetPoint(IP_LABEL_LOCATION_X, 0),
                Text = "IP",
                Size = new Size(18, 12)
            });
            panelControls.Add(new Label()
            {
                Location = GetPoint(NOW_DELAY_LABEL_LOCATION_X, 0),
                Text = "Now",
                Size = new Size(30, 12)
            });
            panelControls.Add(new Label()
            {
                Location = GetPoint(AVG_DELAY_LABEL_LOCATION_X, 0),
                Text = "Avg",
                Size = new Size(30, 12)
            });

            //铺入行数 ~ 标题+1
            var line = 1;

            for (int i = 0; i < Setting.PART_COUNT; ++i)
            {
                var info = Setting.NetworkInfo[i];
                //用户曾在线则创建网段信息控件
                if (info.Visible && !info.UserHidden)
                {
                    //新建网段信息控件
                    var p_now = new Panel()
                    {
                        Location = GetPoint(COLOR_LOCATION_X, line),
                        Size = new Size(COLOR_BLOCK_SIZE_HALF, COLOR_BLOCK_SIZE),
                        Name = "pn_" + i
                    };
                    var p_avg = new Panel()
                    {
                        Location = GetPoint(COLOR_LOCATION_X + COLOR_BLOCK_SIZE_HALF, line),
                        Size = new Size(COLOR_BLOCK_SIZE_HALF, COLOR_BLOCK_SIZE),
                        Name = "pa_" + i
                    };
                    var l_ip = new Label()
                    {
                        Location = GetPoint(IP_LABEL_LOCATION_X, line),
                        Size = new Size(IP_LABEL_WIDTH, 12),
                        Name = "lip_" + i
                    };
                    var l_nt = new Label()
                    {
                        Location = GetPoint(NOW_DELAY_LABEL_LOCATION_X, line),
                        Size = new Size(NOW_DELAY_LABEL_WIDTH, 12),
                        Name = "ln_" + i
                    };
                    var l_at = new Label()
                    {
                        Location = GetPoint(AVG_DELAY_LABEL_LOCATION_X, line),
                        Size = new Size(AVG_DELAY_LABEL_WIDTH, 12),
                        Name = "la_" + i
                    };

                    l_ip.MouseDoubleClick += L_ip_MouseDoubleClick;

                    panelControls.Add(p_now);
                    panelControls.Add(p_avg);
                    panelControls.Add(l_ip);
                    panelControls.Add(l_nt);
                    panelControls.Add(l_at);

                    ++line;
                }

            }

            //新建消息展示标签
            panelControls.Add(new Label()
            {
                Location = GetPoint(IP_LABEL_LOCATION_X, line++),
                Size = new Size(310, 12),
                Name = "label_Log"
            });

            //新建离线敲门进度条
            panelControls.Add(new ProgressBar()
            {
                Location = GetPoint(IP_LABEL_LOCATION_X, line++),
                Size = new Size(240, 4),
                Name = "ProgressBar_KnockDoor",
                Minimum = 0,
                Maximum = 255
            });

            //重置窗体大小
            ReSetFormSize(line);

        }

        Form l_ip_form = null;
        Thread control_log_thread = null;
        private void L_ip_MouseDoubleClick(object sender, EventArgs e)
        {
            var obj = (Label)sender;
            var ip = obj.Text;
            var index = obj.Name.Replace("lip_", "").ToInt();

            if (l_ip_form == null)
            {
                l_ip_form = new Form()
                {
                    Size = new Size(300, 360)
                };

                l_ip_form.Controls.Add(new Label()
                {
                    Location = new Point(10, 10),
                    Size = new Size(280, 340),
                    Name = "log",
                    Text = Setting.NetworkInfo[index].DelayLogString
                });
            }

            if (control_log_thread != null && control_log_thread.IsAlive)
            {
                control_log_thread.Abort();
            }

            control_log_thread = new Thread(() =>
            {
                while (true)
                {
                    var l_log = (Label)l_ip_form.Controls.Find("log", false)[0];

                    Invoke(new Action(() =>
                    {
                        l_log.Text = ip + ":\r\n" + Setting.NetworkInfo[index].DelayLogString;
                    }));

                    Thread.Sleep(20);
                }
            })
            {
                IsBackground = true
            };
            control_log_thread.Start();

            l_ip_form.FormClosing += L_ip_form_FormClosing;

            l_ip_form.Show();
        }

        private void L_ip_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            control_log_thread.Abort();
        }

        /// <summary>
        /// 更新用户界面信息
        /// </summary>
        private void UpdateInformation()
        {
            for (int i = 0; i < Setting.PART_COUNT; ++i)
            {
                var info = Setting.NetworkInfo[i];
                //用户曾在线则更新用户信息
                if (info.Visible && !info.UserHidden)
                {
                    //控件获取
                    var panel_Now = (Panel)panelControls.Find("pn_" + i, false)[0];
                    var panel_Avg = (Panel)panelControls.Find("pa_" + i, false)[0];
                    var label_IP = (Label)panelControls.Find("lip_" + i, false)[0];
                    var label_Now = (Label)panelControls.Find("ln_" + i, false)[0];
                    var label_Avg = (Label)panelControls.Find("la_" + i, false)[0];

                    //信息更新
                    panel_Now.BackColor = info.DelayColor;
                    panel_Avg.BackColor = info.DelayColor_Avg;
                    label_IP.Text = Setting.NetworkInfo[i].HostNickName;
                    label_Now.Text = info.DelayText;
                    label_Now.ForeColor = info.DelayTextColor;
                    label_Avg.Text = info.DelayText_Avg;
                    label_Avg.ForeColor = info.DelayTextColor_Avg;
                }
            }

            //消息控件获取
            var label_Log = (Label)panelControls.Find("label_Log", false)[0];

            //消息控件消息更新
            label_Log.Text =
                "共" + onlineCount +
                "人(在线: " + Setting.NetworkInfo.Count(e => !e.IsDisconnect) +
                "人) " +
                "离线敲门: " + knockDoorPart;

        }

        /// <summary>
        /// 将横向位置与根据行数计算的纵向位置生成为点信息
        /// </summary>
        /// <param name="x">横向位置</param>
        /// <param name="count">行数</param>
        /// <returns>点信息</returns>
        private Point GetPoint(int x, int count)
        {
            return new Point(x, (count + 1) * 10 + count * 9);
        }

        /// <summary>
        /// 根据用户界面控件铺入行数确定窗体大小
        /// </summary>
        /// <param name="count">行数</param>
        private void ReSetFormSize(int count)
        {
            var panelHeight = (count + 1) * 10 + count * 9;
            panel_info.Size = new Size(280, panelHeight);
            Size = new Size(320, panelHeight + 64);
        }

        /// <summary>
        /// 开始初始化按钮点击事件
        /// </summary>
        /// <param name="sender">事件触发控件</param>
        /// <param name="e">事件消息</param>
        private void button_Start_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                button_Start.Enabled = false;
            }));
            MainThreadInit();
        }

        #region 菜单功能项

        private void MenuItem_ShowTop_Click(object sender, EventArgs e)
        {
            MenuItem_ShowTop.Checked = TopMost = !MenuItem_ShowTop.Checked;
            MenuItem_ShowTop.Text = "置顶显示 " + (MenuItem_ShowTop.Checked ? "√" : "×");
        }

        #endregion

        /// <summary>
        /// 窗体关闭前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (initThread.IsAlive)
            {
                initThread.Abort();
            }
            if (controlThread.IsAlive)
            {
                controlThread.Abort();
            }
            if (offlineDetectThread.IsAlive)
            {
                offlineDetectThread.Abort();
            }
        }
    }

}
