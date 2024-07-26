namespace LawfulBladeManager.Dialog
{
    partial class PackageDependentsWarning
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageDependentsWarning));
            lbWarningTitle = new Label();
            btConfirm = new Button();
            ttPrimary = new ToolTip(components);
            btCancel = new Button();
            lbWarningBrief = new Label();
            lbDependents = new ListBox();
            lbWarningDescription = new Label();
            pictureBox1 = new PictureBox();
            tmContinueEnable = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lbWarningTitle
            // 
            lbWarningTitle.AutoSize = true;
            lbWarningTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbWarningTitle.ForeColor = SystemColors.ButtonFace;
            lbWarningTitle.Location = new Point(68, 9);
            lbWarningTitle.Name = "lbWarningTitle";
            lbWarningTitle.Size = new Size(144, 30);
            lbWarningTitle.TabIndex = 0;
            lbWarningTitle.Text = "Fair Warning...";
            // 
            // btConfirm
            // 
            btConfirm.BackColor = Color.FromArgb(120, 66, 135);
            btConfirm.Enabled = false;
            btConfirm.FlatStyle = FlatStyle.Popup;
            btConfirm.ForeColor = SystemColors.ButtonFace;
            btConfirm.Location = new Point(537, 246);
            btConfirm.Name = "btConfirm";
            btConfirm.Size = new Size(75, 23);
            btConfirm.TabIndex = 10;
            btConfirm.Text = "&Continue...";
            btConfirm.UseVisualStyleBackColor = false;
            btConfirm.Click += OnDialogContinue;
            // 
            // btCancel
            // 
            btCancel.FlatStyle = FlatStyle.Popup;
            btCancel.ForeColor = SystemColors.ButtonFace;
            btCancel.Location = new Point(456, 246);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 12;
            btCancel.Text = "Ca&ncel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += OnDialogCancel;
            // 
            // lbWarningBrief
            // 
            lbWarningBrief.AutoSize = true;
            lbWarningBrief.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbWarningBrief.ForeColor = SystemColors.ButtonFace;
            lbWarningBrief.Location = new Point(71, 38);
            lbWarningBrief.Name = "lbWarningBrief";
            lbWarningBrief.Size = new Size(373, 15);
            lbWarningBrief.TabIndex = 13;
            lbWarningBrief.Text = "The package you are trying to uninstall has the following dependents:";
            // 
            // lbDependents
            // 
            lbDependents.BackColor = Color.FromArgb(24, 24, 24);
            lbDependents.BorderStyle = BorderStyle.None;
            lbDependents.ForeColor = Color.IndianRed;
            lbDependents.FormattingEnabled = true;
            lbDependents.ItemHeight = 15;
            lbDependents.Items.AddRange(new object[] { "None", "None2" });
            lbDependents.Location = new Point(12, 57);
            lbDependents.Name = "lbDependents";
            lbDependents.Size = new Size(600, 150);
            lbDependents.TabIndex = 14;
            // 
            // lbWarningDescription
            // 
            lbWarningDescription.AutoSize = true;
            lbWarningDescription.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbWarningDescription.ForeColor = Color.Orange;
            lbWarningDescription.Location = new Point(12, 213);
            lbWarningDescription.Name = "lbWarningDescription";
            lbWarningDescription.Size = new Size(470, 30);
            lbWarningDescription.TabIndex = 15;
            lbWarningDescription.Text = "If you continue with the uninstallation you may completely brick your instance/project! \r\nPlease carefully consider before continuing.";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources._64x_caution;
            pictureBox1.InitialImage = Properties.Resources._64x_caution;
            pictureBox1.Location = new Point(12, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 16;
            pictureBox1.TabStop = false;
            // 
            // tmContinueEnable
            // 
            tmContinueEnable.Interval = 5000;
            // 
            // PackageDependentsWarning
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 281);
            Controls.Add(pictureBox1);
            Controls.Add(lbWarningDescription);
            Controls.Add(lbDependents);
            Controls.Add(lbWarningBrief);
            Controls.Add(btCancel);
            Controls.Add(btConfirm);
            Controls.Add(lbWarningTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 320);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 320);
            Name = "PackageDependentsWarning";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Dependent Packages!";
            TopMost = true;
            Shown += OnDialogShown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbWarningTitle;
        private Button btConfirm;
        private ToolTip ttPrimary;
        private Button btCancel;
        private Label lbWarningBrief;
        private ListBox lbDependents;
        private Label lbWarningDescription;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer tmContinueEnable;
    }
}