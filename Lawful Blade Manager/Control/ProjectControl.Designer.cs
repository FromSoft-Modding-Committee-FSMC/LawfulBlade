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
            tsFuncPackages = new ToolStripButton();
            tsFuncExport = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsFuncOpen = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            tsFunctions.SuspendLayout();
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
            lbDescription.Location = new Point(126, 53);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(386, 40);
            lbDescription.TabIndex = 2;
            lbDescription.Text = "This sample project is for people who really like samples.\r\nTest";
            // 
            // flTagList
            // 
            flTagList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flTagList.Location = new Point(126, 100);
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
            tsFunctions.Items.AddRange(new ToolStripItem[] { tsFuncDelete, tsFuncPackages, tsFuncExport, toolStripSeparator1, tsFuncOpen });
            tsFunctions.Location = new Point(855, 89);
            tsFunctions.Name = "tsFunctions";
            tsFunctions.Size = new Size(161, 31);
            tsFunctions.TabIndex = 9;
            // 
            // tsFuncDelete
            // 
            tsFuncDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncDelete.Image = Properties.Resources._64x_delete;
            tsFuncDelete.ImageTransparentColor = Color.Magenta;
            tsFuncDelete.Name = "tsFuncDelete";
            tsFuncDelete.Size = new Size(28, 28);
            tsFuncDelete.Text = "Delete Project...";
            tsFuncDelete.Click += tsFuncDelete_Click;
            // 
            // tsFuncPackages
            // 
            tsFuncPackages.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncPackages.Image = Properties.Resources._64x_packages;
            tsFuncPackages.ImageTransparentColor = Color.Magenta;
            tsFuncPackages.Name = "tsFuncPackages";
            tsFuncPackages.Size = new Size(28, 28);
            tsFuncPackages.Text = "Manage Packages...";
            tsFuncPackages.Click += tsFuncPackages_Click;
            // 
            // tsFuncExport
            // 
            tsFuncExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsFuncExport.Image = Properties.Resources._64x_export;
            tsFuncExport.ImageTransparentColor = Color.Magenta;
            tsFuncExport.Name = "tsFuncExport";
            tsFuncExport.Size = new Size(28, 28);
            tsFuncExport.Text = "Export Runtime...";
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
            tsFuncOpen.Text = "Open Project...";
            // 
            // ProjectControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(tsFunctions);
            Controls.Add(flTagList);
            Controls.Add(lbDescription);
            Controls.Add(pbIcon);
            Controls.Add(lbTitle);
            Name = "ProjectControl";
            Size = new Size(1024, 128);
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            tsFunctions.ResumeLayout(false);
            tsFunctions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
    }
}
