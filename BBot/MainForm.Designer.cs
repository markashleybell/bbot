namespace BBot
{
    partial class MainForm
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
            this.playButton = new System.Windows.Forms.Button();
            this.debugConsole = new System.Windows.Forms.RichTextBox();
            this.preview = new System.Windows.Forms.PictureBox();
            this.captureButton = new System.Windows.Forms.Button();
            this.duration = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.duration)).BeginInit();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(12, 66);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(116, 42);
            this.playButton.TabIndex = 6;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // debugConsole
            // 
            this.debugConsole.Location = new System.Drawing.Point(141, 347);
            this.debugConsole.Name = "debugConsole";
            this.debugConsole.Size = new System.Drawing.Size(320, 347);
            this.debugConsole.TabIndex = 10;
            this.debugConsole.Text = "";
            // 
            // preview
            // 
            this.preview.Location = new System.Drawing.Point(141, 12);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(320, 320);
            this.preview.TabIndex = 14;
            this.preview.TabStop = false;
            // 
            // captureButton
            // 
            this.captureButton.Location = new System.Drawing.Point(12, 12);
            this.captureButton.Name = "captureButton";
            this.captureButton.Size = new System.Drawing.Size(116, 40);
            this.captureButton.TabIndex = 18;
            this.captureButton.Text = "Capture";
            this.captureButton.UseVisualStyleBackColor = true;
            this.captureButton.Click += new System.EventHandler(this.captureButton_Click);
            // 
            // duration
            // 
            this.duration.Location = new System.Drawing.Point(12, 312);
            this.duration.Name = "duration";
            this.duration.Size = new System.Drawing.Size(116, 20);
            this.duration.TabIndex = 19;
            this.duration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 296);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Duration";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 344);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.duration);
            this.Controls.Add(this.captureButton);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.debugConsole);
            this.Controls.Add(this.playButton);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "BBot";
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.duration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.RichTextBox debugConsole;
        private System.Windows.Forms.PictureBox preview;
        private System.Windows.Forms.Button captureButton;
        private System.Windows.Forms.NumericUpDown duration;
        private System.Windows.Forms.Label label1;
    }
}

