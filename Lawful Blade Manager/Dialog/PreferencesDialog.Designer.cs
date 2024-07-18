namespace LawfulBladeManager.Dialog
{
    partial class PreferencesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesDialog));
            lbCreateProject = new Label();
            btConfirm = new Button();
            ttPrimary = new ToolTip(components);
            tbPackageCacheLocation = new TextBox();
            btPackageCacheSelect = new Button();
            btCancel = new Button();
            lbPackageCacheLocation = new Label();
            SuspendLayout();
            // 
            // lbCreateProject
            // 
            lbCreateProject.AutoSize = true;
            lbCreateProject.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreateProject.ForeColor = SystemColors.ButtonFace;
            lbCreateProject.Location = new Point(12, 9);
            lbCreateProject.Name = "lbCreateProject";
            lbCreateProject.Size = new Size(136, 30);
            lbCreateProject.TabIndex = 0;
            lbCreateProject.Text = "Preferences...";
            // 
            // btConfirm
            // 
            btConfirm.BackColor = Color.FromArgb(120, 66, 135);
            btConfirm.FlatStyle = FlatStyle.Popup;
            btConfirm.ForeColor = SystemColors.ButtonFace;
            btConfirm.Location = new Point(537, 406);
            btConfirm.Name = "btConfirm";
            btConfirm.Size = new Size(75, 23);
            btConfirm.TabIndex = 10;
            btConfirm.Text = "&Confirm";
            btConfirm.UseVisualStyleBackColor = false;
            btConfirm.Click += OnDialogConfirm;
            // 
            // tbPackageCacheLocation
            // 
            tbPackageCacheLocation.BackColor = Color.FromArgb(32, 32, 32);
            tbPackageCacheLocation.BorderStyle = BorderStyle.FixedSingle;
            tbPackageCacheLocation.ForeColor = SystemColors.AppWorkspace;
            tbPackageCacheLocation.Location = new Point(12, 74);
            tbPackageCacheLocation.MaxLength = 64;
            tbPackageCacheLocation.Name = "tbPackageCacheLocation";
            tbPackageCacheLocation.ReadOnly = true;
            tbPackageCacheLocation.Size = new Size(519, 23);
            tbPackageCacheLocation.TabIndex = 14;
            ttPrimary.SetToolTip(tbPackageCacheLocation, "The directory where package *.PAZ files are locally cached.");
            tbPackageCacheLocation.WordWrap = false;
            // 
            // btPackageCacheSelect
            // 
            btPackageCacheSelect.FlatStyle = FlatStyle.Popup;
            btPackageCacheSelect.ForeColor = SystemColors.ButtonFace;
            btPackageCacheSelect.Location = new Point(537, 74);
            btPackageCacheSelect.Name = "btPackageCacheSelect";
            btPackageCacheSelect.Size = new Size(75, 23);
            btPackageCacheSelect.TabIndex = 15;
            btPackageCacheSelect.Text = "Select...";
            ttPrimary.SetToolTip(btPackageCacheSelect, "Select where the Package Cache is located");
            btPackageCacheSelect.UseVisualStyleBackColor = true;
            btPackageCacheSelect.Click += OnPathSelect;
            // 
            // btCancel
            // 
            btCancel.FlatStyle = FlatStyle.Popup;
            btCancel.ForeColor = SystemColors.ButtonFace;
            btCancel.Location = new Point(456, 406);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 12;
            btCancel.Text = "Ca&ncel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += OnDialogCancel;
            // 
            // lbPackageCacheLocation
            // 
            lbPackageCacheLocation.AutoSize = true;
            lbPackageCacheLocation.ForeColor = SystemColors.ButtonFace;
            lbPackageCacheLocation.Location = new Point(9, 53);
            lbPackageCacheLocation.Name = "lbPackageCacheLocation";
            lbPackageCacheLocation.Size = new Size(136, 15);
            lbPackageCacheLocation.TabIndex = 13;
            lbPackageCacheLocation.Text = "Package Cache Location";
            // 
            // PreferencesDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(btPackageCacheSelect);
            Controls.Add(tbPackageCacheLocation);
            Controls.Add(lbPackageCacheLocation);
            Controls.Add(btCancel);
            Controls.Add(btConfirm);
            Controls.Add(lbCreateProject);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "PreferencesDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Project Creator";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbCreateProject;
        private Button btConfirm;
        private ToolTip ttPrimary;
        private Button btCancel;
        private Label lbPackageCacheLocation;
        private TextBox tbPackageCacheLocation;
        private Button btPackageCacheSelect;
    }
}