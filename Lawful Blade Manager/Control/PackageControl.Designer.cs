namespace LawfulBladeManager.Control
{
    partial class PackageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pcMain = new Panel();
            pbIcon = new PictureBox();
            pcStatus = new Panel();
            lbStatus = new Label();
            lbName = new Label();
            pcMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            pcStatus.SuspendLayout();
            SuspendLayout();
            // 
            // pcMain
            // 
            pcMain.BorderStyle = BorderStyle.FixedSingle;
            pcMain.Controls.Add(pbIcon);
            pcMain.Controls.Add(pcStatus);
            pcMain.Controls.Add(lbName);
            pcMain.Dock = DockStyle.Fill;
            pcMain.Location = new Point(0, 0);
            pcMain.Name = "pcMain";
            pcMain.Size = new Size(1024, 32);
            pcMain.TabIndex = 0;
            // 
            // pbIcon
            // 
            pbIcon.BorderStyle = BorderStyle.FixedSingle;
            pbIcon.Dock = DockStyle.Left;
            pbIcon.Location = new Point(0, 0);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(32, 30);
            pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbIcon.TabIndex = 3;
            pbIcon.TabStop = false;
            // 
            // pcStatus
            // 
            pcStatus.Controls.Add(lbStatus);
            pcStatus.Dock = DockStyle.Right;
            pcStatus.Location = new Point(822, 0);
            pcStatus.Name = "pcStatus";
            pcStatus.Padding = new Padding(0, 5, 2, 2);
            pcStatus.Size = new Size(200, 30);
            pcStatus.TabIndex = 2;
            // 
            // lbStatus
            // 
            lbStatus.AutoSize = true;
            lbStatus.BackColor = Color.DarkOrange;
            lbStatus.BorderStyle = BorderStyle.FixedSingle;
            lbStatus.Dock = DockStyle.Right;
            lbStatus.FlatStyle = FlatStyle.Popup;
            lbStatus.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lbStatus.ForeColor = SystemColors.ButtonFace;
            lbStatus.Location = new Point(120, 5);
            lbStatus.Margin = new Padding(3, 4, 3, 0);
            lbStatus.Name = "lbStatus";
            lbStatus.RightToLeft = RightToLeft.No;
            lbStatus.Size = new Size(78, 19);
            lbStatus.TabIndex = 1;
            lbStatus.Text = "Conflict(s)!";
            lbStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lbName.ForeColor = SystemColors.ButtonFace;
            lbName.Location = new Point(36, 5);
            lbName.Name = "lbName";
            lbName.Size = new Size(59, 21);
            lbName.TabIndex = 0;
            lbName.Text = "{name}";
            // 
            // PackageControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            Controls.Add(pcMain);
            Margin = new Padding(0);
            Name = "PackageControl";
            Padding = new Padding(0, 0, 0, 4);
            Size = new Size(1024, 36);
            pcMain.ResumeLayout(false);
            pcMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            pcStatus.ResumeLayout(false);
            pcStatus.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label lbName;
        public Panel pcMain;
        private Panel pcStatus;
        private Label lbStatus;
        private PictureBox pbIcon;
    }
}
