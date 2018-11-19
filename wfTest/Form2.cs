using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfTest
{
    public partial class Form2 : Form
    {
        //用户控制状态
        int result = -1;

        //模拟 .Show() 方法
        public static int Message()
        {
            //新建一个弹出窗口并显示
            Form2 f2 = new Form2();
            f2.Show();

            //实时占用检测用户输入
            while (true)
            {
                //当用户输入时 关闭窗口并返回
                if (f2.result != -1)
                {
                    f2.Close();
                    return f2.result;
                }

                //没操作时交出控制权并闲置线程1ms
                Application.DoEvents();
                Thread.Sleep(1);
            }
        }

        private Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //设定用户操作
            result = 1;
        }
    }
}
