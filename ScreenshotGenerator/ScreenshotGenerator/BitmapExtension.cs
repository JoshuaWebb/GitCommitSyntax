using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ScreenshotGenerator
{
    public static class BitmapExtensions
    {
        // Fast comparison of two images
        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        //http://stackoverflow.com/a/2038515
        public static bool PixelEquals(this Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1 == null) return true;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new System.Drawing.Point(0, 0), b1.Size), ImageLockMode.ReadOnly, b1.PixelFormat);
            var bd2 = b2.LockBits(new Rectangle(new System.Drawing.Point(0, 0), b2.Size), ImageLockMode.ReadOnly, b1.PixelFormat);

            try
            {
                var bd1Scan0 = bd1.Scan0;
                var bd2Scan0 = bd2.Scan0;

                var stride = bd1.Stride;
                var len = stride * b1.Height;

                return memcmp(bd1Scan0, bd2Scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }
    }
}
