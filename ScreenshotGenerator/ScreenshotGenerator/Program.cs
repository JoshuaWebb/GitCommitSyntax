using System;
using System.IO;
using System.Linq;
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
            const string sublimePath = @"C:\Program Files\Sublime Text 3\sublime_text.exe";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var baseDir = FindSolutionDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var examplesPath = Path.Combine(baseDir, "examples");
            var outputPath = Path.Combine(baseDir, "screenshots");

            Form form;
            if (args.Length > 0)
            {
                var fileType = args[0];
                examplesPath = Path.Combine(examplesPath, fileType);
                outputPath = Path.Combine(outputPath, fileType);

                form = new AutoCaptureForm(examplesPath, fileType, outputPath, sublimePath);
            }
            else
            {
                form = new CaptureForm(examplesPath, outputPath, sublimePath);
            }

            Application.Run(form);
        }

        private static string FindSolutionDirectory(string path)
        {
            var di = new DirectoryInfo(path);
            while (di != null && di.EnumerateDirectories().All(d => d.Name != ".git"))
                di = di.Parent;

            return di?.FullName;
        }
    }
}
