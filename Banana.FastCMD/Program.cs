using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Banana.FastCMD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //设置启动位置
                SetConsolePosition(Position.LeftTop);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Title = "Banana.FastCMD by zhangw version 1.0";

                //设置窗口大小
                Console.WindowWidth = 50;
                Console.WindowHeight = 30;
                Console.BufferWidth = 50;
                Console.BufferHeight = 30;

                Console.CursorVisible = false;
                Console.WriteLine("请稍后......");

                RegistryKey rootkey = Registry.LocalMachine;
                //rootkey.DeleteSubKeyTree(@"SOFTWARE\Classes\Folder\shell\cmd");
                RegistryKey cmdkey = rootkey.CreateSubKey(@"SOFTWARE\Classes\Folder\shell\cmd");
                cmdkey.SetValue("", "在 CMD 中打开");
                RegistryKey commandkey = rootkey.CreateSubKey(@"SOFTWARE\Classes\Folder\shell\cmd\command");
                commandkey.SetValue("", "cmd.exe /k cd %1", RegistryValueKind.String);
                rootkey.Close();
                cmdkey.Close();
                Console.Clear();
                Console.WriteLine("设置成功，按任意键退出程序");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("异常：" + ex.Message);
            }
            Console.ReadKey();
        }

        #region code
        
        private struct RECT { public int left, top, right, bottom; }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);

        static void SetConsolePosition(Position pos)
        {
            IntPtr hWin = GetConsoleWindow();
            RECT rc;
            GetWindowRect(hWin, out rc);
            Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));

            int x = 0;
            int y = 0;
            switch (pos)
            {
                case Position.LeftTop:
                    x = x - 7;
                    break;
                case Position.Center:
                    x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
                    y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
                    break;
                default:
                    break;
            }

            MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, true);
        }
        
        #endregion

    }

    /// <summary>
    /// 启动位置
    /// </summary>
    enum Position
    {
        LeftTop = 1,
        Center = 2
    }
}