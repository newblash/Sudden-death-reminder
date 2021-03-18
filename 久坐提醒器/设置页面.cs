using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

namespace 防猝死提醒器
{
    public partial class 设置页面 : Form
    {
        public 设置页面()
        {
            InitializeComponent();
        }

        System.Timers.Timer timer = new System.Timers.Timer();
        锁定页面 SD = new 锁定页面();
        private const int nHotKeyID = 0xabcd;           //热键标识
        private const int nHotKeyID1 = 0xabce;           //热键标识
        private void Form1_Load(object sender, EventArgs e)
        {

            Info.时间变化 = 0;
            HotKey.RegHotKey(this.Handle, nHotKeyID, (int)HotKey.MOD.MOD_ALT, 81);
            HotKey.RegHotKey(this.Handle, nHotKeyID1, (int)HotKey.MOD.MOD_ALT, 87);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            textBox1.Text = config.AppSettings.Settings["工作时间"].Value;
            textBox2.Text = config.AppSettings.Settings["休息时间"].Value;
            textBox3.Text = config.AppSettings.Settings["上午上班时间"].Value;
            textBox4.Text = config.AppSettings.Settings["上午下班时间"].Value;
            textBox5.Text = config.AppSettings.Settings["下午上班时间"].Value;
            textBox6.Text = config.AppSettings.Settings["下午下班时间"].Value;
            textBox7.Text = config.AppSettings.Settings["间隔提醒时间"].Value;
            if (config.AppSettings.Settings["开机启动"].Value == "1")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            Info.工作时间 = double.Parse(config.AppSettings.Settings["工作时间"].Value) * 60;
            Info.休息时间 = double.Parse(config.AppSettings.Settings["休息时间"].Value) * 60;
            Info.提醒间隔 = double.Parse(config.AppSettings.Settings["间隔提醒时间"].Value);
            numericUpDown1.Value = int.Parse(config.AppSettings.Settings["透明度"].Value);
            Info.上午上班时间 = double.Parse(config.AppSettings.Settings["上午上班时间"].Value.Replace(":", ""));
            Info.上午下班时间 = double.Parse(config.AppSettings.Settings["上午下班时间"].Value.Replace(":", ""));
            Info.下午上班时间 = double.Parse(config.AppSettings.Settings["下午上班时间"].Value.Replace(":", ""));
            Info.下午下班时间 = double.Parse(config.AppSettings.Settings["下午下班时间"].Value.Replace(":", ""));
            Thread objThread1 = new Thread(new ThreadStart(delegate//异步执行
            {
                if (Info.时间变化 == 0)
                {
                    timer.Enabled = true;
                    timer.AutoReset = true;
                    timer.Interval = 1000;//执行间隔时间,单位为毫秒  
                    timer.Elapsed += new ElapsedEventHandler(Timer1_Elapsed);
                    timer.Start();
                }
            }));
            objThread1.IsBackground = true;
            objThread1.Priority = ThreadPriority.Normal;
            objThread1.Start();

        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {

            GC.Collect();
            GC.WaitForPendingFinalizers();
            switch (DateTime.Now.ToString("HH:mm:ss"))
            {
                case "19:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("加班呢?,偶尔加班还行吧", "警告", 30000);
                    }));
                    break;
                case "20:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("加班呢?,脑子进水了啊?????? 是老婆抱着不舒服还是游戏不好玩?", "警告", 30000);
                    }));
                    break;
                case "21:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("9点了还加班呢?,脑子进水了啊?????? 是老婆抱着不舒服还是游戏不好玩?", "警告", 30000);
                    }));
                    break;
                case "22:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("10点还加班呢?,没救了吗? 嫌自己死的不够快啊!!!!随你了", "警告", 30000);
                    }));
                    break;
                case "23:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("11点了,11点了,11点了", "猝死警告", 30000);
                    }));
                    break;
                case "24:00:00":
                    Invoke(new EventHandler(delegate
                    {
                        Show("12点了,12点了,12点了,等死吧!!!!", "猝死警告", 30000);
                    }));
                    break;
            }
            Info.时间变化++;
            //测试
            //Console.WriteLine("测试还剩:" + Math.Truncate((Info.工作时间 - Info.时间变化) / 60).ToString() + "分" + (Info.工作时间 - Info.时间变化) % 60 + "秒");

            if (Math.Truncate((Info.工作时间 - Info.时间变化) / 60) > 0)
            {
                notifyIcon1.Text = "休息提醒\r\n距离暴毙还剩:" + Math.Truncate((Info.工作时间 - Info.时间变化) / 60).ToString() + "分" + (Info.工作时间 - Info.时间变化) % 60 + "秒";
                Console.WriteLine("还剩:" + Math.Truncate((Info.工作时间 - Info.时间变化) / 60).ToString() + "分" + (Info.工作时间 - Info.时间变化) % 60 + "秒");
            }
            else
            {
                notifyIcon1.Text = "休息提醒\r\n距离暴毙还剩:"  + (Info.工作时间 - Info.时间变化) % 60 + "秒";
                Console.WriteLine("还剩:"  + (Info.工作时间 - Info.时间变化) % 60 + "秒");
            }


            if ((double.Parse(DateTime.Now.ToString("HHmmss")) > Info.上午上班时间 && double.Parse(DateTime.Now.ToString("HHmmss")) < Info.上午下班时间) ||
            (double.Parse(DateTime.Now.ToString("HHmmss")) > Info.下午上班时间 && double.Parse(DateTime.Now.ToString("HHmmss")) < Info.下午下班时间))
            {
                //工作时间内才执行气泡弹窗,其他时间统一锁死操作界面
                if (Info.时间变化 < Info.工作时间 && Info.时间变化 % Info.提醒间隔 == 0)
                {//上班间隔分钟提醒
                    Invoke(new EventHandler(delegate
                    {
                        if (Info.热键是否按下)
                        {
                            notifyIcon1.ShowBalloonTip(1, "猝死提醒", "已打开强制关闭锁屏,解锁请按Alt+W", ToolTipIcon.Error);
                        }
                        else
                        {
                            if (Math.Truncate((Info.工作时间 - Info.时间变化) / 60) > 0)
                            {
                                notifyIcon1.ShowBalloonTip(1, "休息提醒", "距离暴毙还剩:" + Math.Truncate((Info.工作时间 - Info.时间变化) / 60).ToString() + "分" + (Info.工作时间 - Info.时间变化) % 60 + "秒", ToolTipIcon.Error);
                            }
                            else
                            {
                                notifyIcon1.ShowBalloonTip(1, "休息提醒", "距离暴毙还剩:" + (Info.工作时间 - Info.时间变化) % 60 + "秒", ToolTipIcon.Error);
                            }
                        }
                    }));
                }
                if (Info.时间变化 >= Info.工作时间 && Info.时间变化 <= (Info.工作时间 + Info.休息时间))
                {
                    Console.WriteLine("锁定屏幕时的读秒:" + Info.时间变化);
                    //工作时间到了,休息时间不够,锁死屏幕
                    Invoke(new EventHandler(delegate
                    {
                        改变锁定页面状态(Info.热键是否按下, false);
                    }));
                }
                else if (Info.时间变化 > (Info.工作时间 + Info.休息时间))
                {
                    //休息完成
                    Invoke(new EventHandler(delegate
                    {
                        Console.WriteLine("解锁被触发");
                        改变锁定页面状态(Info.热键是否按下, true);

                    }));
                    Info.时间变化 = 0;
                }
            }
            else
            {
                Info.时间变化 = 0;
                Invoke(new EventHandler(delegate
                {
                    改变锁定页面状态(Info.热键是否按下, false);
                }));


            }
            //Console.WriteLine(Info.时间变化);

        }
        /// <summary>
        /// 加入开机启动
        /// </summary>
        public void StartRun(bool 开机启动)
        {
            string strName = Application.ExecutablePath;
            string strnewName = strName.Substring(strName.LastIndexOf("\\") + 1);

            if (!File.Exists(strName))//指定文件是否存在
                return;
            try
            {
                Microsoft.Win32.RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (开机启动)
                {
                    if (Rkey == null)
                    {
                        Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                        Rkey.SetValue(strnewName, strName);//修改注册表，使程序开机时自动执行。  
                        Console.WriteLine("开机启动开启");
                    }
                    else
                    {
                        Rkey.SetValue(strnewName, strName);//修改注册表，使程序开机时自动执行。  
                        Console.WriteLine("开机启动开启");
                    }
                }
                else
                {
                    Console.WriteLine("开机启动关闭");
                    Rkey.DeleteValue(strnewName);//删除注册表。  
                }

            }
            catch (Exception ex)
            {
                new AggregateException("StartRun", ex); //MessageBox.Show(ex.Message);
            }
        }


        #region//定时关闭弹出窗口
        private string _caption;

        public void Show(string text, string caption, int timeout)
        {
            this._caption = caption;
            StartTimer(timeout);
            MessageBox.Show(text, caption);
        }

        private void StartTimer(int interval)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick1);
            timer.Enabled = true;
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止计时器
            ((System.Timers.Timer)sender).Enabled = false;
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        private void KillMessageBox()
        {
            //查找MessageBox的弹出窗口,注意对应标题
            IntPtr ptr = FindWindow(null, this._caption);
            if (ptr != IntPtr.Zero)
            {
                //查找到窗口则关闭
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            //确定按钮
            this.Hide();
            Info.工作时间 = double.Parse(textBox1.Text) * 60;
            Info.休息时间 = double.Parse(textBox2.Text) * 60;
            Info.提醒间隔 = double.Parse(textBox7.Text);
            SD.Opacity = (double)numericUpDown1.Value / 10;
            Info.上午上班时间 = double.Parse(textBox3.Text.Replace(":", ""));
            Info.上午下班时间 = double.Parse(textBox4.Text.Replace(":", ""));
            Info.下午上班时间 = double.Parse(textBox5.Text.Replace(":", ""));
            Info.下午下班时间 = double.Parse(textBox6.Text.Replace(":", ""));
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        XmlDocument doc = new XmlDocument();
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            //写入配置文件方法
            //获得配置文件的全路径  
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            // DirectoryInfo dir = Directory.GetParent(path);
            string strFileName = Application.ExecutablePath + ".config";//dir.Parent.Parent.FullName
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            TextBox tb = (TextBox)sender;
            //get tb.ID or tb.Name
            switch (tb.Name)
            {
                case "textBox1":
                    //获得将当前元素的key属性  
                    XmlAttribute att = nodes[0].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att.Value != textBox1.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att = nodes[0].Attributes["value"];
                        att.Value = textBox1.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox2":
                    //获得将当前元素的key属性  
                    XmlAttribute att1 = nodes[1].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att1.Value != textBox2.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att1 = nodes[1].Attributes["value"];
                        att1.Value = textBox2.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox3":
                    //获得将当前元素的key属性  
                    XmlAttribute att2 = nodes[2].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att2.Value != textBox3.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att2 = nodes[2].Attributes["value"];
                        att2.Value = textBox3.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox4":
                    //获得将当前元素的key属性  
                    XmlAttribute att3 = nodes[3].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att3.Value != textBox4.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att3 = nodes[3].Attributes["value"];
                        att3.Value = textBox4.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox5":
                    //获得将当前元素的key属性  
                    XmlAttribute att4 = nodes[4].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att4.Value != textBox5.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att4 = nodes[4].Attributes["value"];
                        att4.Value = textBox5.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox6":
                    //获得将当前元素的key属性  
                    XmlAttribute att5 = nodes[5].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att5.Value != textBox6.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att5 = nodes[5].Attributes["value"];
                        att5.Value = textBox6.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
                case "textBox7":
                    //获得将当前元素的key属性  
                    XmlAttribute att6 = nodes[6].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att6.Value != textBox7.Text)
                    {
                        //对目标元素中的第1个属性赋值  
                        att6 = nodes[6].Attributes["value"];
                        att6.Value = textBox7.Text;
                        doc.Save(strFileName);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    break;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string strFileName = Application.ExecutablePath + ".config";//dir.Parent.Parent.FullName
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            XmlAttribute att7 = nodes[7].Attributes["key"];
            if (numericUpDown1.Value <= 10)
            {
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att7.Value != numericUpDown1.Value.ToString())
                {
                    //对目标元素中的第1个属性赋值  
                    att7 = nodes[7].Attributes["value"];
                    att7.Value = numericUpDown1.Value.ToString();
                    doc.Save(strFileName);
                    ConfigurationManager.RefreshSection("appSettings");
                    SD.Opacity = (double)numericUpDown1.Value / 10;
                }
            }
            else
            {
                this.numericUpDown1.Value = 10;
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att7.Value != "10")
                {
                    //对目标元素中的第1个属性赋值  
                    att7 = nodes[7].Attributes["value"];
                    att7.Value = numericUpDown1.Value.ToString();
                    doc.Save(strFileName);
                    ConfigurationManager.RefreshSection("appSettings");
                    SD.Opacity = (double)numericUpDown1.Value / 10;
                }
            }

        }

        /// <summary>
        /// 重写WndProc响应热键方法
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.WParam.ToInt32())
            {
                case nHotKeyID:
                    Info.热键是否按下 = true;
                    Console.WriteLine("ALT+Q");
                    Invoke(new EventHandler(delegate
                    {
                        notifyIcon1.ShowBalloonTip(1, "提醒", "已打开强制关闭锁屏,解锁请按Alt+W", ToolTipIcon.Error);
                    }));
                    break;
                case nHotKeyID1:
                    Info.热键是否按下 = false;
                    Console.WriteLine("ALT+W");
                    Invoke(new EventHandler(delegate
                    {
                        notifyIcon1.ShowBalloonTip(1, "提醒", "强制关闭锁屏已关闭,重新锁定请按Alt+Q", ToolTipIcon.Error);
                    }));
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 改变锁定页面状态
        /// </summary>
        /// <param name="热键"></param>
        /// <param name="锁定时间结束"></param>

        public void 改变锁定页面状态(bool 热键, bool 锁定结束)
        {
            if (!热键 && !锁定结束)
            {
                Invoke(new EventHandler(delegate
                {
                    SD.Show();
                    if (!热键 && !锁定结束)
                    {
                        SD.Activate();
                    }
                }));
            }
            else
            {
                Invoke(new EventHandler(delegate
                {
                    SD.Hide();
                }));
            }
        }

        //开机启动开关
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string strFileName = Application.ExecutablePath + ".config";//dir.Parent.Parent.FullName
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            XmlAttribute att8 = nodes[8].Attributes["key"];
            //根据元素的第一个属性来判断当前的元素是不是目标元素

            if (checkBox1.Checked == true)
            {
                StartRun(true);
                if (att8.Value != "1")
                {
                    //对目标元素中的第1个属性赋值  
                    att8 = nodes[8].Attributes["value"];
                    att8.Value = "1";
                    doc.Save(strFileName);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            else
            {
                StartRun(false);
                if (att8.Value != "0")
                {
                    //对目标元素中的第1个属性赋值  
                    att8 = nodes[8].Attributes["value"];
                    att8.Value = "0";
                    doc.Save(strFileName);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
        }
    }
}

