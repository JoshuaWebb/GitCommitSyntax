using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenshotGenerator
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        // ReSharper restore MemberCanBePrivate.Local

        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }
    }
}