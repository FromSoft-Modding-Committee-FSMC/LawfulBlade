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
            lvPackageFilter = new CheckedListBox();
            scPackageView = new SplitContainer();
            packageInfoControl1 = new Control.PackageInfoControl();
            ((System.ComponentModel.ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scPackageView).BeginInit();
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
            scMain.Panel1.Controls.Add(lvPackageFilter);
            scMain.Panel1MinSize = 192;
            // 
            // scMain.Panel2
            // 
            scMain.Panel2.Controls.Add(scPackageView);
            scMain.Size = new Size(800, 450);
            scMain.SplitterDistance = 192;
            scMain.TabIndex = 0;
            // 
            // lvPackageFilter
            // 
            lvPackageFilter.BackColor = Color.FromArgb(32, 32, 32);
            lvPackageFilter.BorderStyle = BorderStyle.FixedSingle;
            lvPackageFilter.CheckOnClick = true;
            lvPackageFilter.ColumnWidth = 128;
            lvPackageFilter.Dock = DockStyle.Fill;
            lvPackageFilter.ForeColor = SystemColors.ButtonFace;
            lvPackageFilter.FormattingEnabled = true;
            lvPackageFilter.ImeMode = ImeMode.On;
            lvPackageFilter.Items.AddRange(new object[] { "Samples; 1", "Primary, 4" });
            lvPackageFilter.Location = new Point(0, 0);
            lvPackageFilter.MultiColumn = true;
            lvPackageFilter.Name = "lvPackageFilter";
            lvPackageFilter.Size = new Size(192, 450);
            lvPackageFilter.TabIndex = 0;
            lvPackageFilter.ThreeDCheckBoxes = true;
            // 
            // scPackageView
            // 
            scPackageView.Dock = DockStyle.Fill;
            scPackageView.Location = new Point(0, 0);
            scPackageView.Name = "scPackageView";
            // 
            // scPackageView.Panel2
            // 
            scPackageView.Panel2.Controls.Add(packageInfoControl1);
            scPackageView.Size = new Size(604, 450);
            scPackageView.SplitterDistance = 344;
            scPackageView.TabIndex = 0;
            // 
            // packageInfoControl1
            // 
            packageInfoControl1.BackColor = Color.FromArgb(32, 32, 32);
            packageInfoControl1.Dock = DockStyle.Fill;
            packageInfoControl1.Location = new Point(0, 0);
            packageInfoControl1.Name = "packageInfoControl1";
            packageInfoControl1.Size = new Size(256, 450);
            packageInfoControl1.TabIndex = 0;
            // 
            // PackageManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(800, 450);
            Controls.Add(scMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PackageManagerForm";
            Text = "Lawful Blade - Package Manager";
            Load += PackageManagerForm_Load;
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            scPackageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scPackageView).EndInit();
            scPackageView.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer scMain;
        private CheckedListBox lvPackageFilter;
        private SplitContainer scPackageView;
        private Control.PackageInfoControl packageInfoControl1;
    }
}