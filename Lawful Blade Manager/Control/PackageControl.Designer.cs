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
            pcMain.Margin = new Padding(4, 5, 4, 5);
            pcMain.Name = "pcMain";
            pcMain.Size = new Size(1463, 53);
            pcMain.TabIndex = 0;
            // 
            // pbIcon
            // 
            pbIcon.BorderStyle = BorderStyle.FixedSingle;
            pbIcon.Dock = DockStyle.Left;
            pbIcon.Location = new Point(0, 0);
            pbIcon.Margin = new Padding(4, 5, 4, 5);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(45, 51);
            pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbIcon.TabIndex = 3;
            pbIcon.TabStop = false;
            // 
            // pcStatus
            // 
            pcStatus.Controls.Add(lbStatus);
            pcStatus.Dock = DockStyle.Right;
            pcStatus.Location = new Point(1175, 0);
            pcStatus.Margin = new Padding(4, 5, 4, 5);
            pcStatus.Name = "pcStatus";
            pcStatus.Padding = new Padding(0, 8, 3, 3);
            pcStatus.Size = new Size(286, 51);
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
            lbStatus.Location = new Point(165, 8);
            lbStatus.Margin = new Padding(4, 7, 4, 0);
            lbStatus.Name = "lbStatus";
            lbStatus.RightToLeft = RightToLeft.No;
            lbStatus.Size = new Size(118, 30);
            lbStatus.TabIndex = 1;
            lbStatus.Text = "Conflict(s)!";
            lbStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lbName.ForeColor = SystemColors.ButtonFace;
            lbName.Location = new Point(51, 8);
            lbName.Margin = new Padding(4, 0, 4, 0);
            lbName.Name = "lbName";
            lbName.Size = new Size(88, 32);
            lbName.TabIndex = 0;
            lbName.Text = "{name}";
            // 
            // PackageControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            Controls.Add(pcMain);
            Margin = new Padding(0);
            Name = "PackageControl";
            Padding = new Padding(0, 0, 0, 7);
            Size = new Size(1463, 60);
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
