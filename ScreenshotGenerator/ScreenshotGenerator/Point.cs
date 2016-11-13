using System.Runtime.InteropServices;

namespace ScreenshotGenerator
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }
}