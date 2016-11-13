using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenshotGenerator
{
    public static class WindowHelper
    {
        public static bool Resize(IntPtr handle, int newWidth, int newHeight)
        {
            // SWP_NOMOVE (ignore x and y)
            uint flags = 0x0002;
 
            return SetWindowPos(handle, IntPtr.Zero, 0, 0, newWidth, newHeight, flags);
        }

        public static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = GetWindowRectangle(handle);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
            }

            return result;
        }

        // https://code.google.com/p/zscreen/source/browse/trunk/ZScreenLib/Global/GraphicsCore.cs?r=1349

        /// <summary>
        /// Get real window size, no matter whether Win XP, Win Vista, 7 or 8.
        /// </summary>
        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return GetWindowRect(handle);
            }
            else
            {
                Rectangle rectangle;
                return DWMWA_EXTENDED_FRAME_BOUNDS(handle, out rectangle) ? rectangle : GetWindowRect(handle);
            }
        }

        [DllImport(@"dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);

        private enum Dwmwindowattribute
        {
            DwmwaExtendedFrameBounds = 9
        }

        private static bool DWMWA_EXTENDED_FRAME_BOUNDS(IntPtr handle, out Rectangle rectangle)
        {
            Rect rect;
            var result = DwmGetWindowAttribute(handle, (int) Dwmwindowattribute.DwmwaExtendedFrameBounds,
                out rect, Marshal.SizeOf(typeof(Rect)));
            rectangle = rect.ToRectangle();
            return result >= 0;
        }

        private static Rectangle GetWindowRect(IntPtr handle)
        {
            Rect rect;
            GetWindowRect(handle, out rect);
            return rect.ToRectangle();
        }

        public static IntPtr GetWindowForPoint(Point p)
        {
            return WindowFromPoint(p);
        }

        public static bool SetCurrentWindow(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
            hWnd = SetActiveWindow(hWnd);
            return hWnd != IntPtr.Zero;
        }

        public static string GetFilePath(IntPtr hwnd)
        {
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            var proc = Process.GetProcessById((int)pid); //Gets the process by ID.
            return proc.MainModule.FileName;   //Returns the path.
        }

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point p);

        [DllImport("user32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.DLL")]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport(@"user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr handle, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);
    }
}