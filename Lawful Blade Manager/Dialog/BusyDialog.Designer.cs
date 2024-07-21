namespace LawfulBladeManager.Dialog
{
    partial class BusyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BusyDialog));
            pbTheOnlyOne = new ProgressBar();
            tbMessage = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pbTheOnlyOne
            // 
            pbTheOnlyOne.Location = new Point(88, 46);
            pbTheOnlyOne.MarqueeAnimationSpeed = 1;
            pbTheOnlyOne.Name = "pbTheOnlyOne";
            pbTheOnlyOne.Size = new Size(128, 23);
            pbTheOnlyOne.Style = ProgressBarStyle.Marquee;
            pbTheOnlyOne.TabIndex = 0;
            pbTheOnlyOne.UseWaitCursor = true;
            // 
            // tbMessage
            // 
            tbMessage.BackColor = Color.FromArgb(32, 32, 32);
            tbMessage.BorderStyle = BorderStyle.None;
            tbMessage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tbMessage.ForeColor = SystemColors.ButtonFace;
            tbMessage.Location = new Point(12, 8);
            tbMessage.Multiline = true;
            tbMessage.Name = "tbMessage";
            tbMessage.ReadOnly = true;
            tbMessage.ShortcutsEnabled = false;
            tbMessage.Size = new Size(280, 32);
            tbMessage.TabIndex = 1;
            tbMessage.TabStop = false;
            tbMessage.Text = "Don't fall asleep yet!\r\nghgg";
            tbMessage.TextAlign = HorizontalAlignment.Center;
            tbMessage.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.anm_busy_left;
            pictureBox1.Location = new Point(12, 35);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(70, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.anm_busy_right;
            pictureBox2.Location = new Point(222, 35);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(70, 50);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // BusyDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(304, 81);
            ControlBox = false;
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(tbMessage);
            Controls.Add(pbTheOnlyOne);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(320, 120);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(320, 120);
            Name = "BusyDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Doing a Thing...";
            TopMost = true;
            UseWaitCursor = true;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar pbTheOnlyOne;
        private TextBox tbMessage;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}