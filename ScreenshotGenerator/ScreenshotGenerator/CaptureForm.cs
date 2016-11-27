using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ScreenshotGenerator
{
    public partial class CaptureForm : Form
    {
        private readonly string _targetExePath;

        public CaptureForm(string examplesDir, string outputDir, string targetExePath)
        {
            InitializeComponent();

            ExamplesFolder.TextChanged += FolderChanged;
            OutputFolder.TextChanged += FolderChanged;

            ExamplesFolder.Text = ExamplesFolderBrowserDialog.SelectedPath = examplesDir;
            OutputFolder.Text = OutputFolderBrowserDialog.SelectedPath = outputDir;

            _targetExePath = targetExePath;

            GlobalMouseHook.Start();
        }

        private static void FolderChanged(object sender, EventArgs eventArgs)
        {
            var textBox = (TextBox) sender;
            textBox.SelectionStart = textBox.TextLength;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            GlobalMouseHook.MouseAction += Event;
        }

        private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
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

            Generator.CaptureScreensUsingInstance(handle, ExamplesFolder.Text, OutputFolder.Text);
            Generator.CreateGif(OutputFolder.Text);
        }

        private void SelectExampleDirButton_Click(object sender, EventArgs e)
        {
            ScrollToSelectedFolder();
            var result = ExamplesFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                ExamplesFolder.Text = ExamplesFolderBrowserDialog.SelectedPath;
        }

        private void OutputDirButton_Click(object sender, EventArgs e)
        {
            ScrollToSelectedFolder();
            var result = OutputFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                OutputFolder.Text = OutputFolderBrowserDialog.SelectedPath;
        }

        private void ScrollToSelectedFolder()
        {
            SendKeys.Send("{TAB}{TAB}{RIGHT}");
        }
    }
}
