using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //模拟 MessageBox.Show() 的写法
            var a = Form2.Message();

            //测试是否为异步
            MessageBox.Show(a.ToString());
        }
        [DllImport("user32.dll")]
        static extern void mouse_event(int flags, int dX, int dY, int buttons, int extraInfo);

        const int MOUSEEVENTF_MOVE = 0x1;           //0000 0000 0000 0001
        const int MOUSEEVENTF_LEFTDOWN = 0x2;       //0000 0000 0000 0010
        const int MOUSEEVENTF_LEFTUP = 0x4;         //0000 0000 0000 0100
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;      //0000 0000 0000 1000
        const int MOUSEEVENTF_RIGHTUP = 0x10;       //0000 0000 0001 0000
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;    //0000 0000 0010 0000
        const int MOUSEEVENTF_MIDDLEUP = 0x40;      //0000 0000 0100 0000
        const int MOUSEEVENTF_WHEEL = 0x800;        //0000 1000 0000 0000
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;    //1000 0000 0000 0000

        Size sR = Screen.PrimaryScreen.Bounds.Size;

        private void button2_Click(object sender, EventArgs e)
        {
            //lastPosition
            var lP = MousePosition;
            //screenResolution
            var x = textBox1.Text.Int();
            var y = textBox2.Text.Int();
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, GetIntPixel(x, true), GetIntPixel(y, false), 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, GetIntPixel(lP.X + 1, true), GetIntPixel(lP.Y + 1, false), 0, 0);
        }

        private int GetIntPixel(int val, bool isWidth)
        {
            return val * 0x10000 / (isWidth ? sR.Width : sR.Height);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textChange(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textChange(sender, e);
        }

        private void textChange(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                tb.Text.Int();
            }
            catch (Exception)
            {
                tb.Text = "0";
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var position = MousePosition;
            label1.Text = position.X + " x " + position.Y;
        }
    }

    public static class extendFunctions
    {
        public static int Int(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }
    }
}
