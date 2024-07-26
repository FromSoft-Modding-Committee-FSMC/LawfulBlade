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
            tbSandboxLocation = new TextBox();
            btSandboxSelect = new Button();
            btCancel = new Button();
            lbPackageCacheLocation = new Label();
            xbCheckUpdates = new CheckBox();
            lbSandboxing = new Label();
            xbEnableSandboxing = new CheckBox();
            label1 = new Label();
            lbSandboxDesc = new Label();
            tcPreferences = new TabControl();
            tpSandbox = new TabPage();
            tpPackages = new TabPage();
            xbShowConsole = new CheckBox();
            tcPreferences.SuspendLayout();
            tpSandbox.SuspendLayout();
            tpPackages.SuspendLayout();
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
            tbPackageCacheLocation.Location = new Point(6, 30);
            tbPackageCacheLocation.MaxLength = 64;
            tbPackageCacheLocation.Name = "tbPackageCacheLocation";
            tbPackageCacheLocation.ReadOnly = true;
            tbPackageCacheLocation.Size = new Size(502, 23);
            tbPackageCacheLocation.TabIndex = 14;
            ttPrimary.SetToolTip(tbPackageCacheLocation, "The directory where package *.PAZ files are locally cached.");
            tbPackageCacheLocation.WordWrap = false;
            // 
            // btPackageCacheSelect
            // 
            btPackageCacheSelect.FlatStyle = FlatStyle.Popup;
            btPackageCacheSelect.ForeColor = SystemColors.ButtonFace;
            btPackageCacheSelect.Location = new Point(514, 30);
            btPackageCacheSelect.Name = "btPackageCacheSelect";
            btPackageCacheSelect.Size = new Size(75, 23);
            btPackageCacheSelect.TabIndex = 15;
            btPackageCacheSelect.Text = "Select...";
            ttPrimary.SetToolTip(btPackageCacheSelect, "Select where the Package Cache is located");
            btPackageCacheSelect.UseVisualStyleBackColor = true;
            btPackageCacheSelect.Click += OnPathSelect;
            // 
            // tbSandboxLocation
            // 
            tbSandboxLocation.BackColor = Color.FromArgb(32, 32, 32);
            tbSandboxLocation.BorderStyle = BorderStyle.FixedSingle;
            tbSandboxLocation.ForeColor = SystemColors.AppWorkspace;
            tbSandboxLocation.Location = new Point(6, 30);
            tbSandboxLocation.MaxLength = 64;
            tbSandboxLocation.Name = "tbSandboxLocation";
            tbSandboxLocation.ReadOnly = true;
            tbSandboxLocation.Size = new Size(502, 23);
            tbSandboxLocation.TabIndex = 18;
            ttPrimary.SetToolTip(tbSandboxLocation, "The directory where package *.PAZ files are locally cached.");
            tbSandboxLocation.WordWrap = false;
            // 
            // btSandboxSelect
            // 
            btSandboxSelect.FlatStyle = FlatStyle.Popup;
            btSandboxSelect.ForeColor = SystemColors.ButtonFace;
            btSandboxSelect.Location = new Point(514, 30);
            btSandboxSelect.Name = "btSandboxSelect";
            btSandboxSelect.Size = new Size(75, 23);
            btSandboxSelect.TabIndex = 19;
            btSandboxSelect.Text = "Select...";
            ttPrimary.SetToolTip(btSandboxSelect, "Select where the Package Cache is located");
            btSandboxSelect.UseVisualStyleBackColor = true;
            btSandboxSelect.Click += OnPathSelect;
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
            lbPackageCacheLocation.Location = new Point(4, 12);
            lbPackageCacheLocation.Name = "lbPackageCacheLocation";
            lbPackageCacheLocation.Size = new Size(136, 15);
            lbPackageCacheLocation.TabIndex = 13;
            lbPackageCacheLocation.Text = "Package Cache Location";
            // 
            // xbCheckUpdates
            // 
            xbCheckUpdates.AutoSize = true;
            xbCheckUpdates.ForeColor = SystemColors.ButtonFace;
            xbCheckUpdates.Location = new Point(9, 410);
            xbCheckUpdates.Name = "xbCheckUpdates";
            xbCheckUpdates.Size = new Size(187, 19);
            xbCheckUpdates.TabIndex = 16;
            xbCheckUpdates.Text = "Check for updates on start up?";
            xbCheckUpdates.UseVisualStyleBackColor = true;
            // 
            // lbSandboxing
            // 
            lbSandboxing.AutoSize = true;
            lbSandboxing.ForeColor = SystemColors.ButtonFace;
            lbSandboxing.Location = new Point(4, 12);
            lbSandboxing.Name = "lbSandboxing";
            lbSandboxing.Size = new Size(102, 15);
            lbSandboxing.TabIndex = 17;
            lbSandboxing.Text = "Sandbox Location";
            // 
            // xbEnableSandboxing
            // 
            xbEnableSandboxing.AutoSize = true;
            xbEnableSandboxing.ForeColor = SystemColors.ButtonFace;
            xbEnableSandboxing.Location = new Point(6, 59);
            xbEnableSandboxing.Name = "xbEnableSandboxing";
            xbEnableSandboxing.Size = new Size(132, 19);
            xbEnableSandboxing.TabIndex = 20;
            xbEnableSandboxing.Text = "Enforce Sandboxing";
            xbEnableSandboxing.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.OrangeRed;
            label1.Location = new Point(6, 81);
            label1.Name = "label1";
            label1.Size = new Size(575, 60);
            label1.TabIndex = 1;
            label1.Text = "OI:\r\nIF YOU DISABLE THE SANDBOX, YOU DO SO AT YOU'RE OWN RISK!\r\nLAWFUL BLADE DEVELOPERS WILL NOT BE HELD ACCOUNTABLE FOR ANY ACCIDENTAL DESTRUCTION\r\nCAUSED BY POSSIBILY STUPID ACTIONS...";
            // 
            // lbSandboxDesc
            // 
            lbSandboxDesc.AutoSize = true;
            lbSandboxDesc.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbSandboxDesc.ForeColor = Color.Gold;
            lbSandboxDesc.Location = new Point(6, 150);
            lbSandboxDesc.Name = "lbSandboxDesc";
            lbSandboxDesc.Size = new Size(582, 60);
            lbSandboxDesc.TabIndex = 0;
            lbSandboxDesc.Text = resources.GetString("lbSandboxDesc.Text");
            // 
            // tcPreferences
            // 
            tcPreferences.Controls.Add(tpSandbox);
            tcPreferences.Controls.Add(tpPackages);
            tcPreferences.Location = new Point(9, 54);
            tcPreferences.Name = "tcPreferences";
            tcPreferences.SelectedIndex = 0;
            tcPreferences.Size = new Size(603, 346);
            tcPreferences.TabIndex = 22;
            // 
            // tpSandbox
            // 
            tpSandbox.BackColor = Color.FromArgb(32, 32, 32);
            tpSandbox.Controls.Add(label1);
            tpSandbox.Controls.Add(xbEnableSandboxing);
            tpSandbox.Controls.Add(lbSandboxDesc);
            tpSandbox.Controls.Add(btSandboxSelect);
            tpSandbox.Controls.Add(tbSandboxLocation);
            tpSandbox.Controls.Add(lbSandboxing);
            tpSandbox.ForeColor = SystemColors.ButtonFace;
            tpSandbox.Location = new Point(4, 24);
            tpSandbox.Name = "tpSandbox";
            tpSandbox.Padding = new Padding(3);
            tpSandbox.Size = new Size(595, 318);
            tpSandbox.TabIndex = 0;
            tpSandbox.Text = "Sandbox";
            // 
            // tpPackages
            // 
            tpPackages.BackColor = Color.FromArgb(32, 32, 32);
            tpPackages.Controls.Add(lbPackageCacheLocation);
            tpPackages.Controls.Add(tbPackageCacheLocation);
            tpPackages.Controls.Add(btPackageCacheSelect);
            tpPackages.ForeColor = SystemColors.ButtonFace;
            tpPackages.Location = new Point(4, 24);
            tpPackages.Name = "tpPackages";
            tpPackages.Padding = new Padding(3);
            tpPackages.Size = new Size(595, 318);
            tpPackages.TabIndex = 1;
            tpPackages.Text = "Package Management";
            // 
            // xbShowConsole
            // 
            xbShowConsole.AutoSize = true;
            xbShowConsole.ForeColor = SystemColors.ButtonFace;
            xbShowConsole.Location = new Point(202, 410);
            xbShowConsole.Name = "xbShowConsole";
            xbShowConsole.Size = new Size(161, 19);
            xbShowConsole.TabIndex = 23;
            xbShowConsole.Text = "Show console on startup?";
            xbShowConsole.UseVisualStyleBackColor = true;
            // 
            // PreferencesDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(xbShowConsole);
            Controls.Add(tcPreferences);
            Controls.Add(xbCheckUpdates);
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
            Text = "Lawful Blade - Preferences";
            TopMost = true;
            tcPreferences.ResumeLayout(false);
            tpSandbox.ResumeLayout(false);
            tpSandbox.PerformLayout();
            tpPackages.ResumeLayout(false);
            tpPackages.PerformLayout();
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
        private CheckBox xbCheckUpdates;
        private Label lbSandboxing;
        private TextBox tbSandboxLocation;
        private Button btSandboxSelect;
        private CheckBox xbEnableSandboxing;
        private Label label1;
        private Label lbSandboxDesc;
        private TabControl tcPreferences;
        private TabPage tpSandbox;
        private TabPage tpPackages;
        private CheckBox xbShowConsole;
    }
}