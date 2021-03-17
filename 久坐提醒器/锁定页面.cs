using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace 防猝死提醒器
{
    public partial class 锁定页面 : Form
    {

        public 锁定页面()
        {
            InitializeComponent();
        }
        System.Timers.Timer timer = new System.Timers.Timer();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        /// <summary> 
        /// 得到当前活动的窗口 
        /// </summary> 
        /// <returns></returns> 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        private void 锁定页面_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);

            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Interval = 300;//执行间隔时间,单位为毫秒  
            timer.Elapsed += new ElapsedEventHandler(Timer1_Elapsed);
            timer.Start();
        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Invoke(new EventHandler(delegate
            {
                label1.Text = "剩余休息时间:"+(Info.休息时间+Info.工作时间- Info.时间变化).ToString()+"秒";

            }));
        }

        private void 锁定页面_Deactivate(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void 锁定页面_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show("摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼\r\n" +
          "鱼 腰椎间盘突出是久坐人群高发的一种疾病。人在长时间坐着的时候，\r\n" +
          "摸 大部分情况下身体处于不正确的坐姿\r\n" +
          "鱼 我们的身体前倾，这时脊柱的力量是集中在前侧的，\r\n" +
          "摸 会导致椎间盘被挤压向后，长此以往，椎间盘退变老化\r\n" +
          "鱼 就容易受外力作用向后突出压迫神经，让人产生腰腿疼的感觉。\r\n" +
          "摸 长期的久坐并且需要低头工作者还会引发颈椎的退行性变和慢性劳损，\r\n" +
          "鱼 引起头晕、颈背疼痛、上肢无力、手指发麻、下肢乏力、甚至恶心、呕吐\r\n" +
          "摸 甚至视物模糊、心动过速及吞咽困难等，这就是颈椎病了。\r\n" +
          "鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼\r\n" +
          "摸 下肢静脉曲张表现为小腿血管明显凸出，是由于长期站着，下肢静脉回流\r\n" +
          "鱼 障碍较大，此时静脉压过高，长此以往就容易导致静脉曲张。同时，由于\r\n" +
          "摸 下肢静脉回流障碍血液循环不通畅，身体的其他器官的代谢也会受到影响。\r\n" +
          "鱼 站立时全身受力最多的一个部位便是足部了。这是就很容易引发一种病症：\r\n" +
          "摸 足底肌膜炎，就是由于长期站立全身的重量持续作用于足部，引起足底的，\r\n" +
          "鱼 压力增加站立会十分疲乏疼痛。有足底筋膜炎和扁平的人感受会非常明显。\r\n" +
          "摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼 摸鱼",
                "还没到自己设置的休息时间,给劳资继续摸鱼!!!!!");
        }
    }
}
