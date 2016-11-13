using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;

namespace ScreenshotGenerator
{
    public partial class CaptureForm : Form
    {
        private readonly string _examplesDirectory;
        private readonly string _generatedDirectory;
        private readonly string _targetExePath = @"C:\Program Files\Sublime Text 3\sublime_text.exe";

        public CaptureForm()
        {
            InitializeComponent();

            var inputDirName = "examples";
            var root = FindContainingDirectory(inputDirName);
            _examplesDirectory = Path.Combine(root, inputDirName);
            _generatedDirectory = Path.Combine(root, "generated");

            GlobalMouseHook.Start();
        }

        public string FindContainingDirectory(string targetChild)
        {
            var startPath = Application.StartupPath;
            var dirInfo = new DirectoryInfo(startPath);
            while (dirInfo != null)
            {
                if (dirInfo.EnumerateDirectories().Any(d => d.Name == targetChild))
                    return dirInfo.FullName;

                dirInfo = Directory.GetParent(dirInfo.FullName);
            }

            throw new DirectoryNotFoundException("Can't find root git path");
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            GlobalMouseHook.MouseAction += Event;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                GlobalMouseHook.MouseAction -= Event;
                Trace.WriteLine("Cancel");
            }
        }

        private void CreateGif()
        {
            using (var collection = new MagickImageCollection())
            {
                var info = new DirectoryInfo(_generatedDirectory);
                var delay = 250; // 2.5 seconds
                foreach (var file in info.GetFiles().OrderBy(p => p.CreationTime))
                {
                    var item = new MagickImage(file.FullName);
                    item.AnimationDelay = delay;
                    collection.Add(item);
                }

                collection.Write(Path.Combine(_generatedDirectory, "examples.gif"));
            }
        }

        private void Event(object sender, GlobalMouseHook.LowLevelMouseEventArgs e)
        {
            GlobalMouseHook.MouseAction -= Event;
            Trace.WriteLine("Clicked: " + e.LlMouse.pt.x + ", " + e.LlMouse.pt.y);

            var handle = WindowHelper.GetWindowForPoint(e.LlMouse.pt);
            var targetPath = WindowHelper.GetFilePath(handle);
            Trace.WriteLine(targetPath);

            if (targetPath != _targetExePath)
                return;

            // +14 and +7 seem to make the end result actually equal the size we want??
            var targetWidth = 768 + 14;
            var targetHeight = 539 + 7;
            WindowHelper.Resize(handle, targetWidth, targetHeight);

            var samples = Directory.EnumerateFiles(_examplesDirectory);
            Directory.CreateDirectory(_generatedDirectory);

            // Block the UI thread on purpose so that we can't
            // click away while it is taking screenshots
            foreach (var sample in samples)
            {
                Clipboard.SetDataObject(File.ReadAllText(sample), true);

                var split = Path.GetFileNameWithoutExtension(sample).Split(',');
                var cursorLine = int.Parse(split[1]);

                WindowHelper.SetCurrentWindow(handle);

                // Clear text
                SendKeys.Send("^{a}");
                SendKeys.Send("{DEL}");

                // Paste text from clipboard
                SendKeys.Send("^{v}");

                // Go to the line
                SendKeys.Send("^{HOME}");
                for (var i = 0; i < cursorLine - 1; i++)
                    SendKeys.Send("{DOWN}");

                // Put cursor at the end of the line
                SendKeys.Send("{END}");

                var outPath = Path.Combine(_generatedDirectory, split[0]);
                outPath = Path.ChangeExtension(outPath, "png");

                var img = WindowHelper.CaptureWindow(handle);

                // 1 pixel border on every side
                // 50 pixel title bar / menu
                var titleBarHeight = 50;
                var bufferRect = new Rectangle(1, titleBarHeight + 1, img.Width - 2, img.Height - titleBarHeight - 2);
                var framelessImg = img.Clone(bufferRect, img.PixelFormat);

                // Hide the cursor
                framelessImg = framelessImg.ReplaceColor(Color.FromArgb(255, 255, 204, 0), Color.FromArgb(255, 38, 50, 56));

                framelessImg.Save(outPath, ImageFormat.Png);
            }

            // Create the gif in a background thread, we don't need the UI thread anymore.
            var t = Task.Factory.StartNew(CreateGif);
            t.ContinueWith((fail) =>
            {
                Trace.WriteLine("Error: " + fail.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
