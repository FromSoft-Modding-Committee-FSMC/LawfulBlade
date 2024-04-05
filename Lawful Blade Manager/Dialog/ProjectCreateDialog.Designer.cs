namespace LawfulBladeManager.Dialog
{
    partial class ProjectCreateDialog
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
            lbCreateProject = new Label();
            lbProjectName = new Label();
            tbProjectName = new TextBox();
            lbTargetPath = new Label();
            tbTargetPath = new TextBox();
            btTargetSelect = new Button();
            cbTargetInstance = new ComboBox();
            lbTargetInstance = new Label();
            lbDescription = new Label();
            tbDescription = new TextBox();
            btCreate = new Button();
            btCancel = new Button();
            SuspendLayout();
            // 
            // lbCreateProject
            // 
            lbCreateProject.AutoSize = true;
            lbCreateProject.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreateProject.ForeColor = SystemColors.ButtonFace;
            lbCreateProject.Location = new Point(12, 9);
            lbCreateProject.Name = "lbCreateProject";
            lbCreateProject.Size = new Size(201, 30);
            lbCreateProject.TabIndex = 0;
            lbCreateProject.Text = "Create New Project..";
            // 
            // lbProjectName
            // 
            lbProjectName.AutoSize = true;
            lbProjectName.ForeColor = SystemColors.ButtonFace;
            lbProjectName.Location = new Point(9, 53);
            lbProjectName.Name = "lbProjectName";
            lbProjectName.Size = new Size(39, 15);
            lbProjectName.TabIndex = 1;
            lbProjectName.Text = "Name";
            // 
            // tbProjectName
            // 
            tbProjectName.BackColor = Color.FromArgb(32, 32, 32);
            tbProjectName.BorderStyle = BorderStyle.FixedSingle;
            tbProjectName.ForeColor = SystemColors.ButtonFace;
            tbProjectName.Location = new Point(12, 74);
            tbProjectName.MaxLength = 64;
            tbProjectName.Name = "tbProjectName";
            tbProjectName.Size = new Size(519, 23);
            tbProjectName.TabIndex = 2;
            tbProjectName.WordWrap = false;
            // 
            // lbTargetPath
            // 
            lbTargetPath.AutoSize = true;
            lbTargetPath.ForeColor = SystemColors.ButtonFace;
            lbTargetPath.Location = new Point(9, 107);
            lbTargetPath.Name = "lbTargetPath";
            lbTargetPath.Size = new Size(31, 15);
            lbTargetPath.TabIndex = 3;
            lbTargetPath.Text = "Path";
            // 
            // tbTargetPath
            // 
            tbTargetPath.BackColor = Color.FromArgb(32, 32, 32);
            tbTargetPath.BorderStyle = BorderStyle.FixedSingle;
            tbTargetPath.ForeColor = SystemColors.ButtonFace;
            tbTargetPath.Location = new Point(12, 128);
            tbTargetPath.Name = "tbTargetPath";
            tbTargetPath.Size = new Size(519, 23);
            tbTargetPath.TabIndex = 4;
            tbTargetPath.WordWrap = false;
            // 
            // btTargetSelect
            // 
            btTargetSelect.FlatStyle = FlatStyle.Popup;
            btTargetSelect.ForeColor = SystemColors.ButtonFace;
            btTargetSelect.Location = new Point(537, 128);
            btTargetSelect.Name = "btTargetSelect";
            btTargetSelect.Size = new Size(75, 23);
            btTargetSelect.TabIndex = 5;
            btTargetSelect.Text = "Select...";
            btTargetSelect.UseVisualStyleBackColor = true;
            btTargetSelect.Click += btTargetSelect_Click;
            // 
            // cbTargetInstance
            // 
            cbTargetInstance.BackColor = Color.FromArgb(32, 32, 32);
            cbTargetInstance.FlatStyle = FlatStyle.Flat;
            cbTargetInstance.ForeColor = SystemColors.ButtonFace;
            cbTargetInstance.FormattingEnabled = true;
            cbTargetInstance.Location = new Point(12, 183);
            cbTargetInstance.Name = "cbTargetInstance";
            cbTargetInstance.Size = new Size(519, 23);
            cbTargetInstance.TabIndex = 6;
            cbTargetInstance.Text = "Test";
            // 
            // lbTargetInstance
            // 
            lbTargetInstance.AutoSize = true;
            lbTargetInstance.ForeColor = SystemColors.ButtonFace;
            lbTargetInstance.Location = new Point(9, 162);
            lbTargetInstance.Name = "lbTargetInstance";
            lbTargetInstance.Size = new Size(51, 15);
            lbTargetInstance.TabIndex = 7;
            lbTargetInstance.Text = "Instance";
            // 
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.ForeColor = SystemColors.ButtonFace;
            lbDescription.Location = new Point(9, 218);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(67, 15);
            lbDescription.TabIndex = 8;
            lbDescription.Text = "Description";
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(32, 32, 32);
            tbDescription.BorderStyle = BorderStyle.FixedSingle;
            tbDescription.ForeColor = SystemColors.ButtonFace;
            tbDescription.Location = new Point(12, 239);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new Size(600, 161);
            tbDescription.TabIndex = 9;
            // 
            // btCreate
            // 
            btCreate.BackColor = Color.FromArgb(120, 66, 135);
            btCreate.FlatStyle = FlatStyle.Popup;
            btCreate.ForeColor = SystemColors.ButtonFace;
            btCreate.Location = new Point(537, 406);
            btCreate.Name = "btCreate";
            btCreate.Size = new Size(75, 23);
            btCreate.TabIndex = 10;
            btCreate.Text = "&Create";
            btCreate.UseVisualStyleBackColor = false;
            btCreate.Click += btCreate_Click;
            // 
            // btCancel
            // 
            btCancel.FlatStyle = FlatStyle.Popup;
            btCancel.ForeColor = SystemColors.ButtonFace;
            btCancel.Location = new Point(456, 406);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 11;
            btCancel.Text = "Ca&ncel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // ProjectCreateDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(btCancel);
            Controls.Add(btCreate);
            Controls.Add(tbDescription);
            Controls.Add(lbDescription);
            Controls.Add(lbTargetInstance);
            Controls.Add(cbTargetInstance);
            Controls.Add(btTargetSelect);
            Controls.Add(tbTargetPath);
            Controls.Add(lbTargetPath);
            Controls.Add(tbProjectName);
            Controls.Add(lbProjectName);
            Controls.Add(lbCreateProject);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "ProjectCreateDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Create Project";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbCreateProject;
        private Label lbProjectName;
        private TextBox tbProjectName;
        private Label lbTargetPath;
        private TextBox tbTargetPath;
        private Button btTargetSelect;
        private ComboBox cbTargetInstance;
        private Label lbTargetInstance;
        private Label lbDescription;
        private TextBox tbDescription;
        private Button btCreate;
        private Button btCancel;
    }
}