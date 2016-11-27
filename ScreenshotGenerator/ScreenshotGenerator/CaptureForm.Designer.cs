namespace ScreenshotGenerator
{
    partial class CaptureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GoButton = new System.Windows.Forms.Button();
            this.ExamplesFolder = new System.Windows.Forms.TextBox();
            this.SelectExampleDirButton = new System.Windows.Forms.Button();
            this.ExamplesFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.OutputDirButton = new System.Windows.Forms.Button();
            this.OutputFolder = new System.Windows.Forms.TextBox();
            this.OutputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // GoButton
            // 
            this.GoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GoButton.Location = new System.Drawing.Point(13, 68);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(259, 181);
            this.GoButton.TabIndex = 0;
            this.GoButton.Text = "Capture";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.GoButton_Click);
            // 
            // ExamplesFolder
            // 
            this.ExamplesFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExamplesFolder.Location = new System.Drawing.Point(12, 16);
            this.ExamplesFolder.Name = "ExamplesFolder";
            this.ExamplesFolder.Size = new System.Drawing.Size(179, 20);
            this.ExamplesFolder.TabIndex = 1;
            // 
            // SelectExampleDirButton
            // 
            this.SelectExampleDirButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectExampleDirButton.Location = new System.Drawing.Point(197, 14);
            this.SelectExampleDirButton.Name = "SelectExampleDirButton";
            this.SelectExampleDirButton.Size = new System.Drawing.Size(75, 23);
            this.SelectExampleDirButton.TabIndex = 2;
            this.SelectExampleDirButton.Text = "Example Dir";
            this.SelectExampleDirButton.UseVisualStyleBackColor = true;
            this.SelectExampleDirButton.Click += new System.EventHandler(this.SelectExampleDirButton_Click);
            // 
            // OutputDirButton
            // 
            this.OutputDirButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputDirButton.Location = new System.Drawing.Point(197, 40);
            this.OutputDirButton.Name = "OutputDirButton";
            this.OutputDirButton.Size = new System.Drawing.Size(75, 23);
            this.OutputDirButton.TabIndex = 4;
            this.OutputDirButton.Text = "Output Dir";
            this.OutputDirButton.UseVisualStyleBackColor = true;
            this.OutputDirButton.Click += new System.EventHandler(this.OutputDirButton_Click);
            // 
            // OutputFolder
            // 
            this.OutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFolder.Location = new System.Drawing.Point(12, 42);
            this.OutputFolder.Name = "OutputFolder";
            this.OutputFolder.Size = new System.Drawing.Size(179, 20);
            this.OutputFolder.TabIndex = 3;
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.OutputDirButton);
            this.Controls.Add(this.OutputFolder);
            this.Controls.Add(this.SelectExampleDirButton);
            this.Controls.Add(this.ExamplesFolder);
            this.Controls.Add(this.GoButton);
            this.KeyPreview = true;
            this.Name = "CaptureForm";
            this.Text = "CaptureForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CaptureForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.TextBox ExamplesFolder;
        private System.Windows.Forms.Button SelectExampleDirButton;
        private System.Windows.Forms.FolderBrowserDialog ExamplesFolderBrowserDialog;
        private System.Windows.Forms.Button OutputDirButton;
        private System.Windows.Forms.TextBox OutputFolder;
        private System.Windows.Forms.FolderBrowserDialog OutputFolderBrowserDialog;
    }
}

