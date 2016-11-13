using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScreenshotGenerator
{
    public static class BitmapExtensions
    {
        public static Bitmap ReplaceColor(this Bitmap _image, Color target, Color replacement)
        {
            var b = new Bitmap(_image);

            var bData = b.LockBits(new Rectangle(0, 0, _image.Width, _image.Height), ImageLockMode.ReadWrite, b.PixelFormat);

            var bitsPerPixel = Image.GetPixelFormatSize(bData.PixelFormat);
            if (bitsPerPixel < 24)
                throw new InvalidOperationException("Can't handle your small amount of bits per pixel");

            /*the size of the image in bytes */
            var size = bData.Stride * bData.Height;

            /*Allocate buffer for image*/
            var data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);


            for (var i = 0; i < size; i += bitsPerPixel / 8)
            {
                var blue = data[i];
                var green = data[i + 1];
                var red = data[i + 2];

                if (red == target.R && green == target.G && blue == target.B)
                {
                    data[i] = replacement.B;
                    data[i + 1] = replacement.G;
                    data[i + 2] = replacement.R;
                }
            }

            /* This override copies the data back into the location specified */
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);

            b.UnlockBits(bData);

            return b;
        }
    }
}
