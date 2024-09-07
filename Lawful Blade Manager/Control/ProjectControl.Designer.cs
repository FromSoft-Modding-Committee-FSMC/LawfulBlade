namespace LawfulBladeManager.Control
{
    partial class ProjectControl
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
            pbIcon = new PictureBox();
            lbTitle = new Label();
            lbDescription = new Label();
            flTagList = new FlowLayoutPanel();
            tsFunctions = new ToolStrip();
            tsFuncDelete = new ToolStripButton();
            tsFuncSettings = new ToolStripButton();
            tsFuncPackages = new ToolStripButton();
            tsFuncExport = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsFuncOpen = new ToolStripButton();
            pcMain = new Panel();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            tsFunctions.SuspendLayout();
            pcMain.SuspendLayout();
            SuspendLayout();
            // 
            // pbIcon
            // 
            pbIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pbIcon.BackColor = Color.FromArgb(120, 66, 135);
            pbIcon.Location = new Point(8, 8);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(112, 112);
            pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbIcon.TabIndex = 0;
            pbIcon.TabStop = false;
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.BackColor = Color.Transparent;
            lbTitle.FlatStyle = FlatStyle.Flat;
            lbTitle.Font = new Font("Segoe UI", 32.25F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            lbTitle.ForeColor = SystemColors.ButtonFace;
            lbTitle.Location = new Point(118, -4);
            lbTitle.Margin = new Padding(0);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(327, 59);
            lbTitle.TabIndex = 1;
            lbTitle.Text = "Sample Project";
            lbTitle.TextAlign = ContentAlignment.TopCenter;
            lbTitle.UseMnemonic = false;
            // 
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lbDescription.ForeColor = SystemColors.ButtonFace;
            lbDescription.Location = new Point(126, 54);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(386, 40);
            lbDescription.TabIndex = 2;
            lbDescription.Text = "This sample project is for people who really like samples.\r\nTest";
            // 
            // flTagList
            // 
            flTagList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flTagList.Location = new Point(126, 96);
            flTagList.Name = "flTagList";
            flTagList.Size = new Size(640, 24);
            flTagList.TabIndex = 7;
            // 
            // tsFunctions
            // 
            tsFunctions.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tsFunctions.BackgroundImageLayout = ImageLayout.None;
            tsFunctions.Dock = DockStyle.None;
            tsFunctions.ImageScalingSize = new Size(24, 24);
            tsFunctions.Items.AddRange(new ToolStripItem[] { tsFuncDelete, tsFuncSettings, tsFuncPackages, tsFuncExport, toolStripSeparator1, tsFuncOpen });
            tsFunctions.Location = new Point(829, 89);
            tsFunctions.Name = "tsFunctions";
            tsFunctions.RightToLeft = RightToLeft.Yes;
            tsFunctions.Size = new Size(189, 31);
            tsFunctions.TabIndex = 9;
            // 
            // tsFuncDelete
            // 
            tsFuncDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncDelete.Image = Properties.Resources._64x_delete;
            tsFuncDelete.ImageTransparentColor = Color.Magenta;
            tsFuncDelete.Name = "tsFuncDelete";
            tsFuncDelete.Size = new Size(28, 28);
            tsFuncDelete.Text = "Delete...";
            // 
            // tsFuncSettings
            // 
            tsFuncSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncSettings.Image = Properties.Resources._64x_settings;
            tsFuncSettings.ImageTransparentColor = Color.Magenta;
            tsFuncSettings.Name = "tsFuncSettings";
            tsFuncSettings.Size = new Size(28, 28);
            tsFuncSettings.Text = "Settings...";
            // 
            // tsFuncPackages
            // 
            tsFuncPackages.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncPackages.Enabled = false;
            tsFuncPackages.Image = Properties.Resources._64x_packages;
            tsFuncPackages.ImageTransparentColor = Color.Magenta;
            tsFuncPackages.Name = "tsFuncPackages";
            tsFuncPackages.Size = new Size(28, 28);
            tsFuncPackages.Text = "Packages...";
            // 
            // tsFuncExport
            // 
            tsFuncExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncExport.Enabled = false;
            tsFuncExport.Image = Properties.Resources._64x_export;
            tsFuncExport.ImageTransparentColor = Color.Magenta;
            tsFuncExport.Name = "tsFuncExport";
            tsFuncExport.Size = new Size(28, 28);
            tsFuncExport.Text = "Generate Runtime...";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 31);
            // 
            // tsFuncOpen
            // 
            tsFuncOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncOpen.Image = Properties.Resources._64x_open;
            tsFuncOpen.ImageTransparentColor = Color.Magenta;
            tsFuncOpen.Name = "tsFuncOpen";
            tsFuncOpen.Size = new Size(28, 28);
            tsFuncOpen.Text = "Open...";
            // 
            // pcMain
            // 
            pcMain.BackColor = Color.FromArgb(32, 32, 32);
            pcMain.BorderStyle = BorderStyle.FixedSingle;
            pcMain.Controls.Add(tsFunctions);
            pcMain.Controls.Add(pbIcon);
            pcMain.Controls.Add(flTagList);
            pcMain.Controls.Add(lbDescription);
            pcMain.Controls.Add(lbTitle);
            pcMain.Dock = DockStyle.Fill;
            pcMain.Location = new Point(0, 0);
            pcMain.Name = "pcMain";
            pcMain.Size = new Size(1024, 128);
            pcMain.TabIndex = 10;
            // 
            // ProjectControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(pcMain);
            MaximumSize = new Size(4096, 132);
            MinimumSize = new Size(0, 132);
            Name = "ProjectControl";
            Padding = new Padding(0, 0, 0, 4);
            Size = new Size(1024, 132);
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            tsFunctions.ResumeLayout(false);
            tsFunctions.PerformLayout();
            pcMain.ResumeLayout(false);
            pcMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbIcon;
        private Label lbTitle;
        private Label lbDescription;
        private FlowLayoutPanel flTagList;
        private ToolStrip tsFunctions;
        private ToolStripButton tsFuncDelete;
        private ToolStripButton tsFuncOpen;
        private ToolStripButton tsFuncPackages;
        private ToolStripButton tsFuncExport;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsFuncSettings;
        private Panel pcMain;
    }
}
