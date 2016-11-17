using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenshotGenerator
{
    public partial class AutoCaptureForm : Form
    {
        private readonly string _examplesDir;
        private readonly string _outputDir;

        public AutoCaptureForm(string examplesDir, string outputDir, string sublimePath)
        {
            _examplesDir = examplesDir;
            _outputDir = outputDir;
            _sublimePath = sublimePath;

            InitializeComponent();

            WindowState = FormWindowState.Minimized;
        }

        private const string TestFileName = "COMMIT_EDITMSG";
        private readonly string _sublimePath;

        private void AutoCaptureForm_Load(object sender, EventArgs e)
        {
            // "-n" forces a new window no matter what files are already open
            // We may end up with two windows if the last session is enabled
            var sublime = Process.Start(_sublimePath, "-n " + TestFileName);
            sublime.WaitForInputIdle();

            var maxTries = 5;
            var attempt = 0;

            // wait until sublime has a main window we can hijack
            // this normally happens really quickly, so don't wait too long.
            // if there was already a sublime open, the process will already be finished,
            // and the new window will be added to the old process.
            while (attempt++ < maxTries && !sublime.HasExited
                   && sublime.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(100);
                sublime.Refresh();
            }

            // if sublime was already open, then the new window ends up as a child of the old process
            // we need to find the instance that has our new window
            if (sublime.HasExited || sublime.MainWindowHandle == IntPtr.Zero)
            {
                var sublimeContainingWindow =
                    Process.GetProcessesByName("sublime_text").
                    FirstOrDefault(proc => proc.MainWindowTitle.Contains(TestFileName));

                if (sublimeContainingWindow != null)
                {
                    sublime = sublimeContainingWindow;
                }
                else
                {
                    Close();
                    return;
                }
            }

            Generator.CaptureScreensUsingInstance(sublime.MainWindowHandle, _examplesDir, _outputDir);
            sublime.CloseMainWindow();

            // Create the gif in a background thread, we don't need the UI thread anymore.
            var t = Task.Factory.StartNew(() => Generator.CreateGif(_outputDir));

            t.ContinueWith((fail) =>
            {
                Trace.WriteLine("Error: " + fail.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);

            // Has to run on the UI Thread
            t.ContinueWith((success) =>
            {
                Close();
            }, new CancellationToken(), TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
