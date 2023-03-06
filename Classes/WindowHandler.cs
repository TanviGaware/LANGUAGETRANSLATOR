using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;



namespace Automation.common
{
    /// <summary>
    /// This class is used to handle Windows of external 
    /// applications.
    /// </summary>
    ///
    public class WindowHandler
    {
        private static WindowHandler windowHandler;

        #region Windows API Constants

        private const int WM_CLOSE = 0x0010;

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MAXIMIZE = 0xF030;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MOUSEMOVE = 0x0200;

        public const int WM_NULL = 0x0;
        
        // http://www.pinvoke.net/default.aspx/user32/SendMessageTimeout.html
        // http://www.tek-tips.com/viewthread.cfm?qid=1493515&page=1
        enum SendMessageTimeoutFlags : uint{
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8
        }

        #endregion

        #region Windows API Methods
        /// <summary>
        /// Send a Windows Message
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd,
            int Msg,
            int wParam,
            long lParam);

        /// <summary>
        /// Get the foreground window
        /// </summary>
        /// <returns>windowhandle</returns>
        [DllImport("user32.dll")]
        private static extern int GetForegroundWindow();

        /// <summary>
        /// Show a Window
        /// </summary>
        /// <param name="hwnd">windowhandle</param>
        /// <param name="nCmdShow">parameter to set a special state</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        /// <summary>
        /// Gets teh handle of window
        /// </summary>
        /// <param name="className">Class Name of window</param>
        /// <param name="windowCaption">Title of Widnow</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int FindWindow(string className, string windowCaption);

        /// <summary>
        /// Set the window to foreground
        /// </summary>
        /// <param name="hwnd">handle of window</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int SetForegroundWindow(int hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessageTimeout(IntPtr windowsHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            uint uTimeout,
            out UIntPtr lpdwResult);
        

        #endregion

        /// <summary>
        /// Singleton static method.
        /// </summary>
        /// <returns></returns>
        /// 
       
        
        public static WindowHandler getInstance()
        {
            if (windowHandler == null)
            {
                windowHandler = new WindowHandler();
            }

            return windowHandler;
        }

        /// <summary>
        /// Get the Window for a special process
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <returns>Process-Handle</returns>
        public Int32 GetWnd(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new ArgumentException("Process with name " + processName +
                    " not running!");

            return processes[0].MainWindowHandle.ToInt32();
        }

        /// <summary>
        /// Gets window handle
        /// </summary>
        /// <param name="className">class name of window</param>
        /// <param name="caption">title of window</param>
        /// <returns>handle of window. 0 if no window found</returns>
        public int FindWindowHandle(string className, string caption)
        {
            int hwnd = 0;
            hwnd = FindWindow(className, caption);
            return hwnd;
        }

        /// <summary>
        /// Get handle of window
        /// </summary>
        /// <param name="processName">process name</param>
        /// <param name="captionSubstring">part of window title</param>
        /// <returns>handle of window. 0 if no window found</returns>
        public int GetWnd(string processName, string captionSubstring)
        {
            int hwnd = 0;

            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new ArgumentException("Process with name " + processName +
                    " not running!");

            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].MainWindowTitle.Contains(captionSubstring))
                {
                    hwnd = processes[i].MainWindowHandle.ToInt32();
                    break;
                }
            }

            return hwnd;
        }


        /// <summary>
        /// Get the  windowhandle of the window in the foreground
        /// </summary>
        /// <returns>windowhandle</returns>
        public Int32 GetForegroundWnd()
        {
            return GetForegroundWindow();
        }

        /// <summary>
        /// Close the MainWindow of an a process
        /// </summary>
        public void Close(Int32 handle)
        {
            SendMessage(handle, WM_CLOSE, 0, 0);
        }

        /// <summary>
        /// Minimize the Window
        /// </summary>
        /// <param name="handle">windowhandle</param>
        public void Minimize(Int32 handle)
        {
            ShowWindow(handle, SW_SHOWMINIMIZED);
        }

        /// <summary>
        /// Maximize the Window
        /// </summary>
        /// <param name="handle">windowhandle</param>
        public void Maximize(Int32 handle)
        {
            ShowWindow(handle, SW_SHOWMAXIMIZED);
        }

        /// <summary>
        /// Maximize the Window (Using send message)
        /// </summary>
        /// <param name="handle">window handle</param>
        public void Maximize1(Int32 handle)
        {
            SendMessage(handle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }

        /// <summary>
        /// "Normalize" the Window
        /// </summary>
        /// <param name="handle">windowhandle</param>
        public void Normalize(Int32 handle)
        {
            ShowWindow(handle, SW_SHOWNORMAL);
        }

        /// <summary>
        /// To set the window in foreground.
        /// </summary>
        /// <param name="handle">handle of window</param>
        public  void SetWindowInForeground(Int32 handle)
        {
            if (handle <= 0)
            {
                throw new AutomationException("Invalid Handle 0 in SetWindowInForeground");
            }
            /*
            if (getLengthOfWindowText(handle) <= 0)
            {
                throw new AutomationException("Invalid Handle as Text is blank in SetWindowInForeground");
            }
            */
            SetForegroundWindow(handle);
        }


        public string getTextForWindow(Int32 handle)
        {
            IntPtr hWnd = new IntPtr(handle);
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        //public int getLengthOfWindowText(Int32 handle)
        //{
        //    IntPtr hWnd = new IntPtr(handle);

        //    int lengthOfWindowText = 0;
        //    try
        //    {
        //        lengthOfWindowText = GetWindowTextLength(hWnd);
        //    }
        //    catch (Exception e)
        //    {

        //        LogHandler.getLogFile().WriteError(e);
        //    }
        //    return lengthOfWindowText;
        //}

        //public void waitTillWindowInForeground(int windowHandle, int delay)
        //{
        //    int foregroundWindowHandle = windowHandler.GetForegroundWnd();
        //    while (foregroundWindowHandle != windowHandle)
        //    {
        //        AbstractScript.throwExceptionIfUserInterruption();
        //        System.Threading.Thread.Sleep(delay);
        //        foregroundWindowHandle = windowHandler.GetForegroundWnd();
        //    }
        //}

        public bool isWindowResponding(int windowHandle, uint timeout)
        {
            UIntPtr lRes = UIntPtr.Zero;
            IntPtr lReturn = SendMessageTimeout(new IntPtr(windowHandle),
                WM_NULL,
                IntPtr.Zero,
                IntPtr.Zero,
                SendMessageTimeoutFlags.SMTO_ABORTIFHUNG & SendMessageTimeoutFlags.SMTO_BLOCK,
                timeout,
                out lRes);
            if (lReturn != IntPtr.Zero)
            {
                return true;
            }

            return false;
        }

        /*
        /// <summary>
        /// Maximize the Window (Using send message)
        /// </summary>
        /// <param name="handle">window handle</param>
        public void moveMouse(Int32 handle, int x, int y)
        {
            SendMessage(handle, WM_MOUSEMOVE, 0, makeDWord(x, y));
        }

        /// <summary>
        /// Maximize the Window (Using send message)
        /// </summary>
        /// <param name="handle">window handle</param>
        public void mouseClick(Int32 handle, int x, int y)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, makeDWord(x, y));
            SendMessage(handle, WM_LBUTTONUP, 0, makeDWord(x, y));
        }

        /// <summary>
        /// Maximize the Window (Using send message)
        /// </summary>
        /// <param name="handle">window handle</param>
        public void moveMouse(int x, int y)
        {
            moveMouse(GetForegroundWnd(), x, y);
        }

        /// <summary>
        /// Maximize the Window (Using send message)
        /// </summary>
        /// <param name="handle">window handle</param>
        public void mouseClick(int x, int y)
        {
            mouseClick(GetForegroundWnd(), x, y);
        }

        public long makeDWord(int LoWord, int HiWord)
        {
            return ((uint)HiWord * 0x10000) | ((uint)LoWord & 0xffffL);
        }
        */


        /// <summary>
        /// Private Constructor
        /// </summary>
        private WindowHandler()
        {
        }
    }
}