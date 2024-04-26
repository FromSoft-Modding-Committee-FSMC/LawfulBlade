namespace LawfulBladeManager.Forms
{
    partial class PackageManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageManagerForm));
            scMain = new SplitContainer();
            lbFilterTxt = new Label();
            lvPackageFilter = new CheckedListBox();
            scPackageView = new SplitContainer();
            pcPackageList = new Panel();
            exInfo = new Control.PackageInfoControl();
            ((System.ComponentModel.ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scPackageView).BeginInit();
            scPackageView.Panel1.SuspendLayout();
            scPackageView.Panel2.SuspendLayout();
            scPackageView.SuspendLayout();
            SuspendLayout();
            // 
            // scMain
            // 
            scMain.Dock = DockStyle.Fill;
            scMain.FixedPanel = FixedPanel.Panel1;
            scMain.IsSplitterFixed = true;
            scMain.Location = new Point(0, 0);
            scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            scMain.Panel1.Controls.Add(lbFilterTxt);
            scMain.Panel1.Controls.Add(lvPackageFilter);
            scMain.Panel1.Padding = new Padding(4);
            scMain.Panel1MinSize = 192;
            // 
            // scMain.Panel2
            // 
            scMain.Panel2.Controls.Add(scPackageView);
            scMain.Size = new Size(944, 501);
            scMain.SplitterDistance = 192;
            scMain.TabIndex = 0;
            // 
            // lbFilterTxt
            // 
            lbFilterTxt.AutoSize = true;
            lbFilterTxt.Dock = DockStyle.Top;
            lbFilterTxt.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            lbFilterTxt.ForeColor = SystemColors.ButtonFace;
            lbFilterTxt.Location = new Point(4, 4);
            lbFilterTxt.Name = "lbFilterTxt";
            lbFilterTxt.Size = new Size(113, 20);
            lbFilterTxt.TabIndex = 1;
            lbFilterTxt.Text = "Package Filters";
            // 
            // lvPackageFilter
            // 
            lvPackageFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvPackageFilter.BackColor = Color.FromArgb(32, 32, 32);
            lvPackageFilter.BorderStyle = BorderStyle.FixedSingle;
            lvPackageFilter.CheckOnClick = true;
            lvPackageFilter.ColumnWidth = 128;
            lvPackageFilter.ForeColor = SystemColors.ButtonFace;
            lvPackageFilter.FormattingEnabled = true;
            lvPackageFilter.ImeMode = ImeMode.On;
            lvPackageFilter.Items.AddRange(new object[] { "Samples; 1", "Primary, 4" });
            lvPackageFilter.Location = new Point(4, 27);
            lvPackageFilter.Name = "lvPackageFilter";
            lvPackageFilter.RightToLeft = RightToLeft.No;
            lvPackageFilter.Size = new Size(184, 452);
            lvPackageFilter.TabIndex = 0;
            lvPackageFilter.ThreeDCheckBoxes = true;
            lvPackageFilter.ItemCheck += lvPackageFilter_ItemCheck;
            // 
            // scPackageView
            // 
            scPackageView.Dock = DockStyle.Fill;
            scPackageView.Location = new Point(0, 0);
            scPackageView.Name = "scPackageView";
            // 
            // scPackageView.Panel1
            // 
            scPackageView.Panel1.Controls.Add(pcPackageList);
            scPackageView.Panel1.Padding = new Padding(0, 4, 0, 0);
            // 
            // scPackageView.Panel2
            // 
            scPackageView.Panel2.Controls.Add(exInfo);
            scPackageView.Size = new Size(748, 501);
            scPackageView.SplitterDistance = 472;
            scPackageView.TabIndex = 0;
            // 
            // pcPackageList
            // 
            pcPackageList.AutoScroll = true;
            pcPackageList.BorderStyle = BorderStyle.FixedSingle;
            pcPackageList.Dock = DockStyle.Fill;
            pcPackageList.Location = new Point(0, 4);
            pcPackageList.Name = "pcPackageList";
            pcPackageList.Padding = new Padding(4, 4, 4, 0);
            pcPackageList.Size = new Size(472, 497);
            pcPackageList.TabIndex = 0;
            // 
            // exInfo
            // 
            exInfo.BackColor = Color.FromArgb(32, 32, 32);
            exInfo.BorderStyle = BorderStyle.FixedSingle;
            exInfo.Dock = DockStyle.Fill;
            exInfo.Location = new Point(0, 0);
            exInfo.Name = "exInfo";
            exInfo.Size = new Size(272, 501);
            exInfo.TabIndex = 0;
            // 
            // PackageManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(944, 501);
            Controls.Add(scMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(960, 540);
            Name = "PackageManagerForm";
            Text = "Lawful Blade - Package Manager";
            Load += PackageManagerForm_Load;
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel1.PerformLayout();
            scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            scPackageView.Panel1.ResumeLayout(false);
            scPackageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scPackageView).EndInit();
            scPackageView.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer scMain;
        private CheckedListBox lvPackageFilter;
        private SplitContainer scPackageView;
        private Control.PackageInfoControl exInfo;
        private Label lbFilterTxt;
        private Panel pcPackageList;
    }
}