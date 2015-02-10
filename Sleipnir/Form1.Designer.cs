namespace Sleipnir
{
    partial class Sleipnir
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
            this.buttonReceiveFile = new System.Windows.Forms.Button();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.openFDToSendVC = new System.Windows.Forms.OpenFileDialog();
            this.progressBarSleipnir = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // buttonReceiveFile
            // 
            this.buttonReceiveFile.Location = new System.Drawing.Point(47, 12);
            this.buttonReceiveFile.Name = "buttonReceiveFile";
            this.buttonReceiveFile.Size = new System.Drawing.Size(75, 23);
            this.buttonReceiveFile.TabIndex = 0;
            this.buttonReceiveFile.Text = "Receive File";
            this.buttonReceiveFile.UseVisualStyleBackColor = true;
            this.buttonReceiveFile.Click += new System.EventHandler(this.buttonReceiveFile_Click);
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.Location = new System.Drawing.Point(47, 41);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSendFile.TabIndex = 1;
            this.buttonSendFile.Text = "Send File";
            this.buttonSendFile.UseVisualStyleBackColor = true;
            this.buttonSendFile.Click += new System.EventHandler(this.buttonSendFile_Click);
            // 
            // openFDToSendVC
            // 
            this.openFDToSendVC.FileName = "openFileDialog1";
            // 
            // progressBarSleipnir
            // 
            this.progressBarSleipnir.Location = new System.Drawing.Point(12, 80);
            this.progressBarSleipnir.Name = "progressBarSleipnir";
            this.progressBarSleipnir.Size = new System.Drawing.Size(148, 23);
            this.progressBarSleipnir.TabIndex = 2;
            // 
            // Sleipnir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(172, 114);
            this.Controls.Add(this.progressBarSleipnir);
            this.Controls.Add(this.buttonSendFile);
            this.Controls.Add(this.buttonReceiveFile);
            this.Name = "Sleipnir";
            this.Text = "Sleipnir";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonReceiveFile;
        private System.Windows.Forms.Button buttonSendFile;
        private System.Windows.Forms.OpenFileDialog openFDToSendVC;
        private System.Windows.Forms.ProgressBar progressBarSleipnir;
    }
}

