using System;
using System.Windows.Forms;

namespace ScreenshotGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            const string examplesPath = @"C:\Users\user\AppData\Roaming\Sublime Text 3\Packages\GitCommitSyntax\examples";
            const string outputPath = @"C:\Users\user\AppData\Roaming\Sublime Text 3\Packages\GitCommitSyntax\generated";
            const string sublimePath =  @"C:\Program Files\Sublime Text 3\sublime_text.exe";

            var form = args.Length == 0
                ? (Form) new CaptureForm(examplesPath, outputPath, sublimePath)
                :        new AutoCaptureForm(examplesPath, outputPath, sublimePath);

            Application.Run(form);
        }
    }
}
