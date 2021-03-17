using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 防猝死提醒器
{
    public class HotKey
    {
        //============= 1、声明注册热键的方法 ==================
        [DllImport("user32.dll", EntryPoint = "RegisterHotKey")]
        private static extern int RegisterHotKey(IntPtr hWnd, int nID, int nModifiers, int nVK);

        [DllImport("user32.dll", EntryPoint = "RegisterHotKey")]
        private static extern int RegisterHotKey(IntPtr hWnd, int nID, int nModifiers, Keys VK);

        [DllImport("user32.dll", EntryPoint = "UnregisterHotKey")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int nID);

        //============= 2、声明组合键常量 ========================

        public enum MOD
        {
            MOD_NONE = 0,
            MOD_ALT = 1,
            MOD_CTRL = 2,
            MOD_SHIFT = 4,
            MOD_WIN = 8
        }

        //============= 3、实现注册热键的方法 ====================

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="modKey">组合键</param>
        /// <param name="nVK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, MOD modKey, int nVK)
        {
            //========== 3.1、先释放该窗口句柄下具有相同标识的热键 =============
            UnregisterHotKey(hWnd, nID);

            //========== 3.2、注册热键 =========================================
            int nResult = RegisterHotKey(hWnd, nID, (int)modKey, nVK);

            //========== 3.3、返回注册结果 =====================================
            return nResult != 0 ? true : false;
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="modKey">组合键</param>
        /// <param name="VK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, MOD modKey, Keys VK)
        {
            return RegHotKey(hWnd, nID, modKey, (int)VK);
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="modKey">组合键</param>
        /// <param name="nVK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, int modKey, int nVK)
        {
            //========== 3.1、先释放该窗口句柄下具有相同标识的热键 =============
            UnregisterHotKey(hWnd, nID);

            //========== 3.2、注册热键 =========================================
            int nResult = RegisterHotKey(hWnd, nID, modKey, nVK);

            //========== 3.3、返回注册结果 =====================================
            return nResult != 0 ? true : false;
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="modKey">组合键</param>
        /// <param name="VK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, int modKey, Keys VK)
        {
            return RegHotKey(hWnd, nID, modKey, (int)VK);
        }


        /// <summary>
        /// 注册热键，不包含组合键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="nVK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, int nVK)
        {
            return RegHotKey(hWnd, nID, MOD.MOD_NONE, nVK);
        }

        /// <summary>
        /// 注册热键，不包含组合键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nID">热键标识</param>
        /// <param name="VK">热键</param>
        /// <returns></returns>
        public static bool RegHotKey(IntPtr hWnd, int nID, Keys VK)
        {
            return RegHotKey(hWnd, nID, (int)VK);
        }

    }
}
