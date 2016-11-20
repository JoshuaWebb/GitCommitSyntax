using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ImageMagick;

namespace ScreenshotGenerator
{
    public class Generator
    {
        public static void CreateGif(string directory)
        {
            using (var collection = new MagickImageCollection())
            {
                var info = new DirectoryInfo(directory);
                var delay = 250; // 2.5 seconds
                foreach (var file in info.GetFiles("*.png").OrderBy(p => p.Name))
                {
                    var item = new MagickImage(file.FullName);
                    item.AnimationDelay = delay;
                    collection.Add(item);
                }

                collection.Write(Path.Combine(directory, "examples.gif"));
            }
        }

        public static void CaptureScreensUsingInstance(IntPtr handle, string examplesDirectory, string outputDirectory)
        {
            var targetWidth = 768;
            var targetHeight = 539;
            // +14 and +7 seem to make the end result actually equal the size we want??
            // TODO: find out where these magic numbers come from...
            WindowHelper.Resize(handle, targetWidth + 14, targetHeight + 7);

            var sampleTextFiles = Directory.EnumerateFiles(examplesDirectory);
            Directory.CreateDirectory(outputDirectory);

            var ready = false;
            var backgroundColor = Color.FromArgb(255, 38, 50, 56);
            var caretColor = Color.FromArgb(255, 255, 204, 0);

            WindowHelper.SetCurrentWindow(handle);

            // Block the UI thread on purpose so that we can't
            // click away while it is taking screenshots
            foreach (var sampleTextFile in sampleTextFiles)
            {
                var split = Path.GetFileNameWithoutExtension(sampleTextFile).Split(',');
                if (split.Length == 1)
                    continue;

                var cursorLine = int.Parse(split[1]);

                var sampleText = File.ReadAllText(sampleTextFile);
                Clipboard.SetDataObject(sampleText, true);

                // Try to ensure the window is still focused
                WindowHelper.SetCurrentWindow(handle);

                // Select all and clear text
                SendKeys.Send("^{a}");
                SendKeys.Send("{DEL}");

                // Make sure the window looks the same "two frames in a row"
                // (the window might still be fading in). Once the window
                // is ready, it should be fine for the rest of the session.
                Bitmap prevImage = null;
                while(!ready)
                {
                    var testImage = WindowHelper.CaptureWindow(handle);
                    if (!CompareMemCmp(testImage, prevImage))
                        Thread.Sleep(100);
                    else
                        ready = true;

                    prevImage = testImage;
                    Trace.WriteLine("ready: " + ready);
                }

                // Paste text from clipboard
                SendKeys.Send("^{v}");

                // Go to the line offset from the top
                SendKeys.Send("^{HOME}");
                SendKeys.Send("{DOWN " + (cursorLine - 1) + "}");

                // Put cursor at the end of the line
                SendKeys.Send("{END}");

                var outPath = Path.Combine(outputDirectory, split[0]);
                outPath = Path.ChangeExtension(outPath, "png");

                var img = WindowHelper.CaptureWindow(handle);

                // 1 pixel border on every side
                // 50 pixel title bar / menu
                var titleBarHeight = 50;
                var bufferRect = new Rectangle(1, titleBarHeight + 1, img.Width - 2, img.Height - titleBarHeight - 2);
                var framelessImg = img.Clone(bufferRect, img.PixelFormat);

                // Hide the caret
                framelessImg = framelessImg.ReplaceColor(caretColor, backgroundColor);

                framelessImg.Save(outPath, ImageFormat.Png);
            }

            // Open console (default shortcut is CTRL+`, but I've added SHIFT)
            SendKeys.Send("^+{`}");

            // Run command to set window to scratch (disables save prompt on close)
            SendKeys.Send("view.set_scratch{(}True{)}~");

            // Close console
            SendKeys.Send("{ESC}");
        }

        // Fast comparison of two images
        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        //http://stackoverflow.com/a/2038515
        public static bool CompareMemCmp(Bitmap b1, Bitmap b2)
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
