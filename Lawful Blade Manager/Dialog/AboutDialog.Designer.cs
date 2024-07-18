namespace LawfulBladeManager.Dialog
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            btConfirm = new Button();
            pbBanner = new PictureBox();
            rtAbout = new RichTextBox();
            pnRichTextBorder = new Panel();
            ((System.ComponentModel.ISupportInitialize)pbBanner).BeginInit();
            SuspendLayout();
            // 
            // btConfirm
            // 
            btConfirm.BackColor = Color.FromArgb(120, 66, 135);
            btConfirm.FlatStyle = FlatStyle.Popup;
            btConfirm.ForeColor = SystemColors.ButtonFace;
            btConfirm.Location = new Point(280, 406);
            btConfirm.Name = "btConfirm";
            btConfirm.Size = new Size(80, 23);
            btConfirm.TabIndex = 10;
            btConfirm.Text = "&Okidoki";
            btConfirm.UseVisualStyleBackColor = false;
            btConfirm.Click += OnDialogConfirm;
            // 
            // pbBanner
            // 
            pbBanner.Image = (Image)resources.GetObject("pbBanner.Image");
            pbBanner.InitialImage = (Image)resources.GetObject("pbBanner.InitialImage");
            pbBanner.Location = new Point(24, 12);
            pbBanner.Margin = new Padding(0);
            pbBanner.Name = "pbBanner";
            pbBanner.Size = new Size(576, 128);
            pbBanner.TabIndex = 11;
            pbBanner.TabStop = false;
            // 
            // rtAbout
            // 
            rtAbout.BackColor = Color.FromArgb(32, 32, 32);
            rtAbout.BorderStyle = BorderStyle.None;
            rtAbout.ForeColor = SystemColors.ButtonFace;
            rtAbout.Location = new Point(28, 156);
            rtAbout.Name = "rtAbout";
            rtAbout.ReadOnly = true;
            rtAbout.Size = new Size(568, 240);
            rtAbout.TabIndex = 12;
            rtAbout.Text = "";
            rtAbout.LinkClicked += OnLinkClicked;
            // 
            // pnRichTextBorder
            // 
            pnRichTextBorder.BackColor = Color.Transparent;
            pnRichTextBorder.BorderStyle = BorderStyle.FixedSingle;
            pnRichTextBorder.Location = new Point(24, 152);
            pnRichTextBorder.Name = "pnRichTextBorder";
            pnRichTextBorder.Size = new Size(576, 248);
            pnRichTextBorder.TabIndex = 13;
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(pbBanner);
            Controls.Add(btConfirm);
            Controls.Add(rtAbout);
            Controls.Add(pnRichTextBorder);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "AboutDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - About";
            TopMost = true;
            Load += OnDialogLoad;
            ((System.ComponentModel.ISupportInitialize)pbBanner).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button btConfirm;
        private PictureBox pbBanner;
        private RichTextBox rtAbout;
        private Panel pnRichTextBorder;
    }
}