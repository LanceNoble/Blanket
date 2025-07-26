using HtmlAgilityPack;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
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

        public Form1()
        {
            // TODO: Only send progman message if workerw window can't be found
            IntPtr progman = FindWindow("Progman", null);
            SendMessageTimeout(
                FindWindow("Progman", null),
                0x052C,
                IntPtr.Zero,
                IntPtr.Zero,
                SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out _);
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

            SetParent(this.Handle, workerw);
            InitializeComponent();
            this.Location = new Point(0, 0);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.DOMContentLoaded += async (foo, args) =>
            {
                String path = File.ReadAllText(".\\web\\index.js");
                await webView.CoreWebView2.ExecuteScriptAsync(path);
            };
            webView.CoreWebView2.Navigate("https://www.shadertoy.com/view/3csSWB");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                "C:\\Users\\Lance\\Desktop\\2560x1440-48404.png",
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
