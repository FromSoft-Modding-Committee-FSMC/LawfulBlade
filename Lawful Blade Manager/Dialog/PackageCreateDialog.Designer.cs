namespace LawfulBladeManager.Dialog
{
    partial class PackageCreateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageCreateDialog));
            lbCreatePackage = new Label();
            tbName = new TextBox();
            lbProjectName = new Label();
            tbAuthors = new TextBox();
            lbAuthor = new Label();
            ttMain = new ToolTip(components);
            tbVersion = new TextBox();
            tbTags = new TextBox();
            tbSource = new TextBox();
            tbDescription = new TextBox();
            pbIcon = new PictureBox();
            xbExpectOverwrites = new CheckBox();
            lbVersion = new Label();
            lbTags = new Label();
            lbSource = new Label();
            btSourceSelect = new Button();
            lbDescription = new Label();
            btCancel = new Button();
            btCreate = new Button();
            lbIcon = new Label();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            SuspendLayout();
            // 
            // lbCreatePackage
            // 
            lbCreatePackage.AutoSize = true;
            lbCreatePackage.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreatePackage.ForeColor = SystemColors.ButtonFace;
            lbCreatePackage.Location = new Point(12, 9);
            lbCreatePackage.Name = "lbCreatePackage";
            lbCreatePackage.Size = new Size(218, 30);
            lbCreatePackage.TabIndex = 1;
            lbCreatePackage.Text = "Create New Package...";
            // 
            // tbName
            // 
            tbName.BackColor = Color.FromArgb(32, 32, 32);
            tbName.BorderStyle = BorderStyle.FixedSingle;
            tbName.ForeColor = SystemColors.ButtonFace;
            tbName.Location = new Point(12, 75);
            tbName.MaxLength = 64;
            tbName.Name = "tbName";
            tbName.PlaceholderText = "My awesome package";
            tbName.Size = new Size(519, 23);
            tbName.TabIndex = 4;
            ttMain.SetToolTip(tbName, "The name of the package.");
            tbName.WordWrap = false;
            // 
            // lbProjectName
            // 
            lbProjectName.AutoSize = true;
            lbProjectName.ForeColor = SystemColors.ButtonFace;
            lbProjectName.Location = new Point(9, 55);
            lbProjectName.Name = "lbProjectName";
            lbProjectName.Size = new Size(39, 15);
            lbProjectName.TabIndex = 3;
            lbProjectName.Text = "Name";
            // 
            // tbAuthors
            // 
            tbAuthors.BackColor = Color.FromArgb(32, 32, 32);
            tbAuthors.BorderStyle = BorderStyle.FixedSingle;
            tbAuthors.ForeColor = SystemColors.ButtonFace;
            tbAuthors.Location = new Point(12, 123);
            tbAuthors.MaxLength = 64;
            tbAuthors.Name = "tbAuthors";
            tbAuthors.PlaceholderText = "Author 1; Author 2";
            tbAuthors.Size = new Size(519, 23);
            tbAuthors.TabIndex = 6;
            ttMain.SetToolTip(tbAuthors, "The author or authors of the package (seperate with semi-colon)");
            tbAuthors.WordWrap = false;
            // 
            // lbAuthor
            // 
            lbAuthor.AutoSize = true;
            lbAuthor.ForeColor = SystemColors.ButtonFace;
            lbAuthor.Location = new Point(9, 103);
            lbAuthor.Name = "lbAuthor";
            lbAuthor.Size = new Size(57, 15);
            lbAuthor.TabIndex = 5;
            lbAuthor.Text = "Author(s)";
            // 
            // tbVersion
            // 
            tbVersion.BackColor = Color.FromArgb(32, 32, 32);
            tbVersion.BorderStyle = BorderStyle.FixedSingle;
            tbVersion.ForeColor = SystemColors.ButtonFace;
            tbVersion.Location = new Point(12, 219);
            tbVersion.MaxLength = 64;
            tbVersion.Name = "tbVersion";
            tbVersion.PlaceholderText = "1.00R";
            tbVersion.Size = new Size(519, 23);
            tbVersion.TabIndex = 8;
            ttMain.SetToolTip(tbVersion, "The package version");
            tbVersion.WordWrap = false;
            // 
            // tbTags
            // 
            tbTags.BackColor = Color.FromArgb(32, 32, 32);
            tbTags.BorderStyle = BorderStyle.FixedSingle;
            tbTags.ForeColor = SystemColors.ButtonFace;
            tbTags.Location = new Point(12, 171);
            tbTags.MaxLength = 64;
            tbTags.Name = "tbTags";
            tbTags.PlaceholderText = "Editor; Sample; Runtime";
            tbTags.Size = new Size(519, 23);
            tbTags.TabIndex = 9;
            ttMain.SetToolTip(tbTags, "Tags the package conforms to (seperate with semi-colon)");
            tbTags.WordWrap = false;
            // 
            // tbSource
            // 
            tbSource.BackColor = Color.FromArgb(32, 32, 32);
            tbSource.BorderStyle = BorderStyle.FixedSingle;
            tbSource.ForeColor = SystemColors.ButtonFace;
            tbSource.Location = new Point(12, 267);
            tbSource.Name = "tbSource";
            tbSource.PlaceholderText = "C:\\...";
            tbSource.Size = new Size(519, 23);
            tbSource.TabIndex = 11;
            ttMain.SetToolTip(tbSource, "The directory containing the package files");
            tbSource.WordWrap = false;
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(32, 32, 32);
            tbDescription.ForeColor = SystemColors.ButtonFace;
            tbDescription.Location = new Point(12, 315);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.PlaceholderText = "Write something in me";
            tbDescription.ScrollBars = ScrollBars.Vertical;
            tbDescription.Size = new Size(600, 75);
            tbDescription.TabIndex = 15;
            ttMain.SetToolTip(tbDescription, "The description of the package");
            // 
            // pbIcon
            // 
            pbIcon.BorderStyle = BorderStyle.FixedSingle;
            pbIcon.Image = Properties.Resources._128x_package;
            pbIcon.InitialImage = Properties.Resources._64x_packages;
            pbIcon.Location = new Point(537, 75);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(75, 75);
            pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbIcon.TabIndex = 16;
            pbIcon.TabStop = false;
            ttMain.SetToolTip(pbIcon, "Icon for the package. Double click to change.");
            pbIcon.DoubleClick += OnDoubleClickIcon;
            // 
            // xbExpectOverwrites
            // 
            xbExpectOverwrites.AutoSize = true;
            xbExpectOverwrites.ForeColor = SystemColors.ButtonFace;
            xbExpectOverwrites.Location = new Point(12, 406);
            xbExpectOverwrites.Name = "xbExpectOverwrites";
            xbExpectOverwrites.Size = new Size(125, 19);
            xbExpectOverwrites.TabIndex = 20;
            xbExpectOverwrites.Text = "Expect Overwrites?";
            ttMain.SetToolTip(xbExpectOverwrites, "Check this if you expect the package to overwrite files, such as a translation patch etc");
            xbExpectOverwrites.UseVisualStyleBackColor = true;
            // 
            // lbVersion
            // 
            lbVersion.AutoSize = true;
            lbVersion.ForeColor = SystemColors.ButtonFace;
            lbVersion.Location = new Point(9, 199);
            lbVersion.Name = "lbVersion";
            lbVersion.Size = new Size(45, 15);
            lbVersion.TabIndex = 7;
            lbVersion.Text = "Version";
            // 
            // lbTags
            // 
            lbTags.AutoSize = true;
            lbTags.ForeColor = SystemColors.ButtonFace;
            lbTags.Location = new Point(9, 151);
            lbTags.Name = "lbTags";
            lbTags.Size = new Size(38, 15);
            lbTags.TabIndex = 10;
            lbTags.Text = "Tag(s)";
            // 
            // lbSource
            // 
            lbSource.AutoSize = true;
            lbSource.ForeColor = SystemColors.ButtonFace;
            lbSource.Location = new Point(9, 247);
            lbSource.Name = "lbSource";
            lbSource.Size = new Size(43, 15);
            lbSource.TabIndex = 12;
            lbSource.Text = "Source";
            // 
            // btSourceSelect
            // 
            btSourceSelect.FlatStyle = FlatStyle.Popup;
            btSourceSelect.ForeColor = SystemColors.ButtonFace;
            btSourceSelect.Location = new Point(537, 267);
            btSourceSelect.Name = "btSourceSelect";
            btSourceSelect.Size = new Size(75, 23);
            btSourceSelect.TabIndex = 13;
            btSourceSelect.Text = "Select...";
            btSourceSelect.UseVisualStyleBackColor = true;
            btSourceSelect.Click += OnClickSelect;
            // 
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.ForeColor = SystemColors.ButtonFace;
            lbDescription.Location = new Point(9, 295);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(67, 15);
            lbDescription.TabIndex = 14;
            lbDescription.Text = "Description";
            // 
            // btCancel
            // 
            btCancel.FlatStyle = FlatStyle.Popup;
            btCancel.ForeColor = SystemColors.ButtonFace;
            btCancel.Location = new Point(456, 406);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 18;
            btCancel.Text = "Ca&ncel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += OnClickCancel;
            // 
            // btCreate
            // 
            btCreate.BackColor = Color.FromArgb(120, 66, 135);
            btCreate.FlatStyle = FlatStyle.Popup;
            btCreate.ForeColor = SystemColors.ButtonFace;
            btCreate.Location = new Point(537, 406);
            btCreate.Name = "btCreate";
            btCreate.Size = new Size(75, 23);
            btCreate.TabIndex = 17;
            btCreate.Text = "&Create";
            btCreate.UseVisualStyleBackColor = false;
            btCreate.Click += OnClickCreate;
            // 
            // lbIcon
            // 
            lbIcon.AutoSize = true;
            lbIcon.ForeColor = SystemColors.ButtonFace;
            lbIcon.Location = new Point(535, 55);
            lbIcon.Name = "lbIcon";
            lbIcon.Size = new Size(30, 15);
            lbIcon.TabIndex = 19;
            lbIcon.Text = "Icon";
            // 
            // PackageCreateDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(xbExpectOverwrites);
            Controls.Add(lbIcon);
            Controls.Add(btCancel);
            Controls.Add(btCreate);
            Controls.Add(pbIcon);
            Controls.Add(tbDescription);
            Controls.Add(lbDescription);
            Controls.Add(btSourceSelect);
            Controls.Add(lbSource);
            Controls.Add(tbSource);
            Controls.Add(lbTags);
            Controls.Add(tbTags);
            Controls.Add(tbVersion);
            Controls.Add(lbVersion);
            Controls.Add(tbAuthors);
            Controls.Add(lbAuthor);
            Controls.Add(tbName);
            Controls.Add(lbProjectName);
            Controls.Add(lbCreatePackage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "PackageCreateDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Package Creator";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbCreatePackage;
        private TextBox tbName;
        private Label lbProjectName;
        private ToolTip ttMain;
        private TextBox tbAuthors;
        private Label lbAuthor;
        private Label lbVersion;
        private TextBox tbVersion;
        private TextBox tbTags;
        private Label lbTags;
        private TextBox tbSource;
        private Label lbSource;
        private Button btSourceSelect;
        private Label lbDescription;
        private TextBox tbDescription;
        private PictureBox pbIcon;
        private Button btCancel;
        private Button btCreate;
        private Label lbIcon;
        private CheckBox xbExpectOverwrites;
    }
}