namespace LawfulBladeManager.Forms
{
    partial class PackageDeltaDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageDeltaDialog));
            btGenerateDelta = new Button();
            btCancel = new Button();
            tbSourceA = new TextBox();
            btSelectSrcA = new Button();
            lbSourceA = new Label();
            tbSourceB = new TextBox();
            lbSourceB = new Label();
            btSelectSrcB = new Button();
            ttMainHint = new ToolTip(components);
            tbTarget = new TextBox();
            xbIncludeRemovedFiles = new CheckBox();
            btSelectTarget = new Button();
            xbGenerateLog = new CheckBox();
            lbTarget = new Label();
            SuspendLayout();
            // 
            // btGenerateDelta
            // 
            btGenerateDelta.BackColor = Color.FromArgb(120, 66, 135);
            btGenerateDelta.FlatStyle = FlatStyle.Popup;
            btGenerateDelta.ForeColor = SystemColors.ButtonFace;
            btGenerateDelta.Location = new Point(537, 186);
            btGenerateDelta.Name = "btGenerateDelta";
            btGenerateDelta.Size = new Size(75, 23);
            btGenerateDelta.TabIndex = 0;
            btGenerateDelta.Text = "&Generate";
            btGenerateDelta.UseVisualStyleBackColor = false;
            btGenerateDelta.Click += btGenerateDelta_Click;
            // 
            // btCancel
            // 
            btCancel.BackColor = Color.FromArgb(32, 32, 32);
            btCancel.FlatStyle = FlatStyle.Popup;
            btCancel.ForeColor = SystemColors.ButtonFace;
            btCancel.Location = new Point(456, 186);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 1;
            btCancel.Text = "Ca&ncel";
            btCancel.UseVisualStyleBackColor = false;
            btCancel.Click += btCancel_Click;
            // 
            // tbSourceA
            // 
            tbSourceA.BackColor = Color.FromArgb(32, 32, 32);
            tbSourceA.BorderStyle = BorderStyle.FixedSingle;
            tbSourceA.ForeColor = SystemColors.ButtonFace;
            tbSourceA.Location = new Point(12, 36);
            tbSourceA.Name = "tbSourceA";
            tbSourceA.PlaceholderText = "Enter Source A Path...";
            tbSourceA.Size = new Size(519, 23);
            tbSourceA.TabIndex = 2;
            ttMainHint.SetToolTip(tbSourceA, "Set 'Source A' to the directory containing unmodified files.");
            // 
            // btSelectSrcA
            // 
            btSelectSrcA.BackColor = Color.FromArgb(32, 32, 32);
            btSelectSrcA.FlatStyle = FlatStyle.Popup;
            btSelectSrcA.ForeColor = SystemColors.ButtonFace;
            btSelectSrcA.Location = new Point(537, 36);
            btSelectSrcA.Name = "btSelectSrcA";
            btSelectSrcA.Size = new Size(75, 23);
            btSelectSrcA.TabIndex = 3;
            btSelectSrcA.Text = "Select...";
            ttMainHint.SetToolTip(btSelectSrcA, "Select Source A.");
            btSelectSrcA.UseVisualStyleBackColor = false;
            btSelectSrcA.Click += btSelectSourceTarget_Click;
            // 
            // lbSourceA
            // 
            lbSourceA.AutoSize = true;
            lbSourceA.ForeColor = SystemColors.ButtonFace;
            lbSourceA.Location = new Point(10, 16);
            lbSourceA.Name = "lbSourceA";
            lbSourceA.Size = new Size(54, 15);
            lbSourceA.TabIndex = 4;
            lbSourceA.Text = "Source A";
            // 
            // tbSourceB
            // 
            tbSourceB.BackColor = Color.FromArgb(32, 32, 32);
            tbSourceB.BorderStyle = BorderStyle.FixedSingle;
            tbSourceB.ForeColor = SystemColors.ButtonFace;
            tbSourceB.Location = new Point(12, 94);
            tbSourceB.Name = "tbSourceB";
            tbSourceB.PlaceholderText = "Enter Source B Path...";
            tbSourceB.Size = new Size(519, 23);
            tbSourceB.TabIndex = 5;
            ttMainHint.SetToolTip(tbSourceB, "Set 'Source B' to the directory containing modified files.");
            // 
            // lbSourceB
            // 
            lbSourceB.AutoSize = true;
            lbSourceB.ForeColor = SystemColors.ButtonFace;
            lbSourceB.Location = new Point(10, 74);
            lbSourceB.Name = "lbSourceB";
            lbSourceB.Size = new Size(53, 15);
            lbSourceB.TabIndex = 6;
            lbSourceB.Text = "Source B";
            // 
            // btSelectSrcB
            // 
            btSelectSrcB.BackColor = Color.FromArgb(32, 32, 32);
            btSelectSrcB.FlatStyle = FlatStyle.Popup;
            btSelectSrcB.ForeColor = SystemColors.ButtonFace;
            btSelectSrcB.Location = new Point(537, 94);
            btSelectSrcB.Name = "btSelectSrcB";
            btSelectSrcB.Size = new Size(75, 23);
            btSelectSrcB.TabIndex = 7;
            btSelectSrcB.Text = "Select...";
            ttMainHint.SetToolTip(btSelectSrcB, "Select Source B.");
            btSelectSrcB.UseVisualStyleBackColor = false;
            btSelectSrcB.Click += btSelectSourceTarget_Click;
            // 
            // tbTarget
            // 
            tbTarget.BackColor = Color.FromArgb(32, 32, 32);
            tbTarget.BorderStyle = BorderStyle.FixedSingle;
            tbTarget.ForeColor = SystemColors.ButtonFace;
            tbTarget.Location = new Point(12, 153);
            tbTarget.Name = "tbTarget";
            tbTarget.PlaceholderText = "Enter Target Path...";
            tbTarget.Size = new Size(519, 23);
            tbTarget.TabIndex = 8;
            ttMainHint.SetToolTip(tbTarget, "Set 'Target' to the directory you want the delta to be placed in...");
            // 
            // xbIncludeRemovedFiles
            // 
            xbIncludeRemovedFiles.AutoSize = true;
            xbIncludeRemovedFiles.ForeColor = SystemColors.ButtonFace;
            xbIncludeRemovedFiles.Location = new Point(12, 190);
            xbIncludeRemovedFiles.Name = "xbIncludeRemovedFiles";
            xbIncludeRemovedFiles.Size = new Size(144, 19);
            xbIncludeRemovedFiles.TabIndex = 11;
            xbIncludeRemovedFiles.Text = "Include Removed Files";
            ttMainHint.SetToolTip(xbIncludeRemovedFiles, "Include files which have been removed from 'Source B' but exist in 'Source A'");
            xbIncludeRemovedFiles.UseVisualStyleBackColor = true;
            // 
            // btSelectTarget
            // 
            btSelectTarget.BackColor = Color.FromArgb(32, 32, 32);
            btSelectTarget.FlatStyle = FlatStyle.Popup;
            btSelectTarget.ForeColor = SystemColors.ButtonFace;
            btSelectTarget.Location = new Point(537, 153);
            btSelectTarget.Name = "btSelectTarget";
            btSelectTarget.Size = new Size(75, 23);
            btSelectTarget.TabIndex = 10;
            btSelectTarget.Text = "Select...";
            ttMainHint.SetToolTip(btSelectTarget, "Select Target (the directory the delta files are placed.)");
            btSelectTarget.UseVisualStyleBackColor = false;
            btSelectTarget.Click += btSelectSourceTarget_Click;
            // 
            // xbGenerateLog
            // 
            xbGenerateLog.AutoSize = true;
            xbGenerateLog.ForeColor = SystemColors.ButtonFace;
            xbGenerateLog.Location = new Point(162, 190);
            xbGenerateLog.Name = "xbGenerateLog";
            xbGenerateLog.Size = new Size(117, 19);
            xbGenerateLog.TabIndex = 12;
            xbGenerateLog.Text = "Generate Log File";
            ttMainHint.SetToolTip(xbGenerateLog, "Generates a log file to allow generation debugging");
            xbGenerateLog.UseVisualStyleBackColor = true;
            // 
            // lbTarget
            // 
            lbTarget.AutoSize = true;
            lbTarget.ForeColor = SystemColors.ButtonFace;
            lbTarget.Location = new Point(10, 133);
            lbTarget.Name = "lbTarget";
            lbTarget.Size = new Size(39, 15);
            lbTarget.TabIndex = 9;
            lbTarget.Text = "Target";
            // 
            // PackageDeltaDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 221);
            Controls.Add(xbGenerateLog);
            Controls.Add(xbIncludeRemovedFiles);
            Controls.Add(btSelectTarget);
            Controls.Add(lbTarget);
            Controls.Add(tbTarget);
            Controls.Add(btSelectSrcB);
            Controls.Add(lbSourceB);
            Controls.Add(tbSourceB);
            Controls.Add(lbSourceA);
            Controls.Add(btSelectSrcA);
            Controls.Add(tbSourceA);
            Controls.Add(btCancel);
            Controls.Add(btGenerateDelta);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 260);
            MinimizeBox = false;
            MinimumSize = new Size(640, 260);
            Name = "PackageDeltaDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Package Delta Generator";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btGenerateDelta;
        private Button btCancel;
        private TextBox tbSourceA;
        private Button btSelectSrcA;
        private Label lbSourceA;
        private TextBox tbSourceB;
        private Label lbSourceB;
        private Button btSelectSrcB;
        private ToolTip ttMainHint;
        private TextBox tbTarget;
        private Label lbTarget;
        private Button btSelectTarget;
        private CheckBox xbIncludeRemovedFiles;
        private CheckBox xbGenerateLog;
    }
}