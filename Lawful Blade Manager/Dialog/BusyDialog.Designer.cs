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
            SuspendLayout();
            // 
            // pbTheOnlyOne
            // 
            pbTheOnlyOne.Location = new Point(12, 46);
            pbTheOnlyOne.MarqueeAnimationSpeed = 1;
            pbTheOnlyOne.Name = "pbTheOnlyOne";
            pbTheOnlyOne.Size = new Size(280, 23);
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
            // BusyDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(304, 81);
            ControlBox = false;
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
            Text = "Busy...";
            TopMost = true;
            UseWaitCursor = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar pbTheOnlyOne;
        private TextBox tbMessage;
    }
}