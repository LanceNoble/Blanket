using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [Flags]
        enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("User32.dll")]
        static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            UInt32 Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            UInt32 uTimeout,
            out IntPtr lpdwResult);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll")]
        static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            String lpszClass,
            IntPtr lpszWindow);

        [DllImport("User32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll")]
        static extern Boolean SystemParametersInfo(UInt32 uAction, UInt32 uParam, String lpvParam, UInt32 fuWinIni);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();
            //form.WindowState = FormWindowState.Maximized;
            //form.FormBorderStyle = FormBorderStyle.None;

            IntPtr progman = FindWindow("Progman", null);
            //IntPtr messageToProgmanResult = SendMessageTimeout(
            //    progman,
            //    0x052C,
            //    IntPtr.Zero,
            //    IntPtr.Zero,
            //    SendMessageTimeoutFlags.SMTO_NORMAL,
            //    1000,
            //    out _);
            IntPtr workerw = IntPtr.Zero;
            Boolean enumResult = EnumWindows(new EnumWindowsProc((hwnd, lParam) =>
            {
                IntPtr searchResult = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                if (searchResult != IntPtr.Zero)
                {
                    workerw = FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", IntPtr.Zero);
                }
                return true;
            }), IntPtr.Zero);

            SetParent(form.Handle, workerw);
            //form.Location = new System.Drawing.Point(100, 0);

            //const int SPI_SETDESKWALLPAPER = 20;
            //const int SPIF_UPDATEINIFILE = 0x01;
            //const int SPIF_SENDWININICHANGE = 0x02;

            //SystemParametersInfo(SPI_SETDESKWALLPAPER,
            //    0,
            //    "D:\\Users\\Lance\\Downloads\\2560x1440-48404.png",
            //    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);



            Application.Run(form);
        }
    }
}
