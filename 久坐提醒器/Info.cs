using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 防猝死提醒器
{
    static class Info
    {
        static public double 时间变化 { get; set; }
        static public double 工作时间 { get; set; }
        static public double 休息时间 { get; set; }
        static public double 提醒间隔 { get; set; }
        static public double 上午上班时间 { get; set; }
        static public double 上午下班时间 { get; set; }
        static public double 下午上班时间 { get; set; }
        static public double 下午下班时间 { get; set; }
        static public int 透明度 { get; set; }
        static public bool 热键是否按下 { get; set; }
    }
}
