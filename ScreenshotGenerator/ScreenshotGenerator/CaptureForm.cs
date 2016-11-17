using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ScreenshotGenerator
{
    public partial class CaptureForm : Form
    {
        private readonly string _examplesDirectory;
        private readonly string _generatedDirectory;
        private readonly string _targetExePath;

        public CaptureForm(string examplesDir, string outputDir, string targetExePath)
        {
            InitializeComponent();

            _examplesDirectory = examplesDir;
            _generatedDirectory = outputDir;
            _targetExePath = targetExePath;

            GlobalMouseHook.Start();
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

        private void Event(object sender, GlobalMouseHook.LowLevelMouseEventArgs e)
        {
            GlobalMouseHook.MouseAction -= Event;
            Trace.WriteLine("Clicked: " + e.LlMouse.pt.x + ", " + e.LlMouse.pt.y);

            var handle = WindowHelper.GetWindowForPoint(e.LlMouse.pt);
            var targetPath = WindowHelper.GetFilePath(handle);
            Trace.WriteLine(targetPath);

            if (targetPath != _targetExePath)
                return;

            Generator.CaptureScreensUsingInstance(handle, _examplesDirectory, _generatedDirectory);
        }
    }
}
