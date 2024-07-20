namespace LawfulBladeManager.Dialog
{
    partial class InstanceCreateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstanceCreateDialog));
            lbCreateProject = new Label();
            lbName = new Label();
            tbName = new TextBox();
            lbTargetPath = new Label();
            tbTargetPath = new TextBox();
            btTargetSelect = new Button();
            lbDescription = new Label();
            tbDescription = new TextBox();
            btCreate = new Button();
            btCancel = new Button();
            ttPrimary = new ToolTip(components);
            pbIcon = new PictureBox();
            btIconRight = new Button();
            btIconLeft = new Button();
            lbIcon = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            SuspendLayout();
            // 
            // lbCreateProject
            // 
            lbCreateProject.AutoSize = true;
            lbCreateProject.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreateProject.ForeColor = SystemColors.ButtonFace;
            lbCreateProject.Location = new Point(12, 9);
            lbCreateProject.Name = "lbCreateProject";
            lbCreateProject.Size = new Size(215, 30);
            lbCreateProject.TabIndex = 0;
            lbCreateProject.Text = "Create New Instance..";
            // 
            // lbName
            // 
            lbName.AutoSize = true;
            lbName.ForeColor = SystemColors.ButtonFace;
            lbName.Location = new Point(9, 53);
            lbName.Name = "lbName";
            lbName.Size = new Size(39, 15);
            lbName.TabIndex = 1;
            lbName.Text = "Name";
            // 
            // tbName
            // 
            tbName.BackColor = Color.FromArgb(32, 32, 32);
            tbName.BorderStyle = BorderStyle.FixedSingle;
            tbName.ForeColor = SystemColors.ButtonFace;
            tbName.Location = new Point(12, 74);
            tbName.MaxLength = 64;
            tbName.Name = "tbName";
            tbName.PlaceholderText = "Wonderful name here...";
            tbName.Size = new Size(519, 23);
            tbName.TabIndex = 2;
            ttPrimary.SetToolTip(tbName, "The name of the instance");
            tbName.WordWrap = false;
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
            tbTargetPath.PlaceholderText = "Where'd you want it?..";
            tbTargetPath.Size = new Size(519, 23);
            tbTargetPath.TabIndex = 4;
            ttPrimary.SetToolTip(tbTargetPath, "The target path of the instance. An additional sub directory will be created in the selected path to store the instance");
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
            ttPrimary.SetToolTip(btTargetSelect, "Select the project path");
            btTargetSelect.UseVisualStyleBackColor = true;
            btTargetSelect.Click += OnClickSelect;
            // 
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.ForeColor = SystemColors.ButtonFace;
            lbDescription.Location = new Point(9, 162);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(67, 15);
            lbDescription.TabIndex = 7;
            lbDescription.Text = "Description";
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(24, 24, 24);
            tbDescription.BorderStyle = BorderStyle.FixedSingle;
            tbDescription.ForeColor = SystemColors.ButtonFace;
            tbDescription.Location = new Point(12, 182);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.PlaceholderText = "Insert description here...";
            tbDescription.Size = new Size(438, 96);
            tbDescription.TabIndex = 9;
            ttPrimary.SetToolTip(tbDescription, "A description of the project.");
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
            btCreate.Click += OnClickCreate;
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
            btCancel.Click += OnClickCancel;
            // 
            // pbIcon
            // 
            pbIcon.BackColor = Color.FromArgb(16, 16, 16);
            pbIcon.Image = Properties.Resources._256xInstMoonlightSword;
            pbIcon.Location = new Point(486, 182);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(96, 96);
            pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbIcon.TabIndex = 12;
            pbIcon.TabStop = false;
            // 
            // btIconRight
            // 
            btIconRight.FlatStyle = FlatStyle.Popup;
            btIconRight.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btIconRight.ForeColor = SystemColors.ButtonFace;
            btIconRight.Location = new Point(588, 182);
            btIconRight.Name = "btIconRight";
            btIconRight.Size = new Size(24, 96);
            btIconRight.TabIndex = 13;
            btIconRight.Text = "▶";
            btIconRight.UseVisualStyleBackColor = true;
            btIconRight.Click += OnChangeIcon;
            // 
            // btIconLeft
            // 
            btIconLeft.FlatStyle = FlatStyle.Popup;
            btIconLeft.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btIconLeft.ForeColor = SystemColors.ButtonFace;
            btIconLeft.Location = new Point(456, 182);
            btIconLeft.Name = "btIconLeft";
            btIconLeft.Size = new Size(24, 96);
            btIconLeft.TabIndex = 14;
            btIconLeft.Text = "◀";
            btIconLeft.UseVisualStyleBackColor = true;
            btIconLeft.Click += OnChangeIcon;
            // 
            // lbIcon
            // 
            lbIcon.AutoSize = true;
            lbIcon.ForeColor = SystemColors.ButtonFace;
            lbIcon.Location = new Point(456, 162);
            lbIcon.Name = "lbIcon";
            lbIcon.Size = new Size(30, 15);
            lbIcon.TabIndex = 15;
            lbIcon.Text = "Icon";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(16, 16, 16);
            panel1.Location = new Point(456, 182);
            panel1.Name = "panel1";
            panel1.Size = new Size(156, 96);
            panel1.TabIndex = 16;
            // 
            // InstanceCreateDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(lbIcon);
            Controls.Add(btIconLeft);
            Controls.Add(btIconRight);
            Controls.Add(pbIcon);
            Controls.Add(btCancel);
            Controls.Add(btCreate);
            Controls.Add(tbDescription);
            Controls.Add(lbDescription);
            Controls.Add(btTargetSelect);
            Controls.Add(tbTargetPath);
            Controls.Add(lbTargetPath);
            Controls.Add(tbName);
            Controls.Add(lbName);
            Controls.Add(lbCreateProject);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "InstanceCreateDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Instance Creator";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbCreateProject;
        private Label lbName;
        private TextBox tbName;
        private Label lbTargetPath;
        private TextBox tbTargetPath;
        private Button btTargetSelect;
        private Label lbDescription;
        private TextBox tbDescription;
        private Button btCreate;
        private Button btCancel;
        private ToolTip ttPrimary;
        private PictureBox pbIcon;
        private Button btIconRight;
        private Button btIconLeft;
        private Label lbIcon;
        private Panel panel1;
    }
}