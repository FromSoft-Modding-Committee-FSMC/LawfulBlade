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
            textBox1 = new TextBox();
            lbVersion = new Label();
            lbTags = new Label();
            lbSource = new Label();
            btSourceSelect = new Button();
            lbDescription = new Label();
            btCancel = new Button();
            btCreate = new Button();
            lbIcon = new Label();
            pcSlidingView = new Panel();
            lbCreatePackage2 = new Label();
            lbDependencies = new Label();
            btNext = new Button();
            btBack = new Button();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            pcSlidingView.SuspendLayout();
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
            tbName.BackColor = Color.FromArgb(24, 24, 24);
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
            lbProjectName.Location = new Point(10, 57);
            lbProjectName.Name = "lbProjectName";
            lbProjectName.Size = new Size(39, 15);
            lbProjectName.TabIndex = 3;
            lbProjectName.Text = "Name";
            // 
            // tbAuthors
            // 
            tbAuthors.BackColor = Color.FromArgb(24, 24, 24);
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
            lbAuthor.Location = new Point(10, 105);
            lbAuthor.Name = "lbAuthor";
            lbAuthor.Size = new Size(57, 15);
            lbAuthor.TabIndex = 5;
            lbAuthor.Text = "Author(s)";
            // 
            // tbVersion
            // 
            tbVersion.BackColor = Color.FromArgb(24, 24, 24);
            tbVersion.BorderStyle = BorderStyle.FixedSingle;
            tbVersion.ForeColor = SystemColors.ButtonFace;
            tbVersion.Location = new Point(12, 267);
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
            tbTags.BackColor = Color.FromArgb(24, 24, 24);
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
            tbSource.BackColor = Color.FromArgb(24, 24, 24);
            tbSource.BorderStyle = BorderStyle.FixedSingle;
            tbSource.ForeColor = SystemColors.ButtonFace;
            tbSource.Location = new Point(12, 315);
            tbSource.Name = "tbSource";
            tbSource.PlaceholderText = "C:\\...";
            tbSource.Size = new Size(519, 23);
            tbSource.TabIndex = 11;
            ttMain.SetToolTip(tbSource, "The directory containing the package files");
            tbSource.WordWrap = false;
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(24, 24, 24);
            tbDescription.BorderStyle = BorderStyle.FixedSingle;
            tbDescription.ForeColor = SystemColors.ButtonFace;
            tbDescription.Location = new Point(652, 75);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.PlaceholderText = "Write something in me";
            tbDescription.ScrollBars = ScrollBars.Vertical;
            tbDescription.Size = new Size(600, 325);
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
            xbExpectOverwrites.Location = new Point(12, 409);
            xbExpectOverwrites.Name = "xbExpectOverwrites";
            xbExpectOverwrites.Size = new Size(125, 19);
            xbExpectOverwrites.TabIndex = 20;
            xbExpectOverwrites.Text = "Expect Overwrites?";
            ttMain.SetToolTip(xbExpectOverwrites, "Check this if you expect the package to overwrite files, such as a translation patch etc");
            xbExpectOverwrites.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(24, 24, 24);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.ForeColor = SystemColors.ButtonFace;
            textBox1.Location = new Point(12, 219);
            textBox1.MaxLength = 64;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Editor; Sample; Runtime";
            textBox1.Size = new Size(519, 23);
            textBox1.TabIndex = 25;
            ttMain.SetToolTip(textBox1, "Tags the package conforms to (seperate with semi-colon)");
            textBox1.WordWrap = false;
            // 
            // lbVersion
            // 
            lbVersion.AutoSize = true;
            lbVersion.ForeColor = SystemColors.ButtonFace;
            lbVersion.Location = new Point(10, 249);
            lbVersion.Name = "lbVersion";
            lbVersion.Size = new Size(45, 15);
            lbVersion.TabIndex = 7;
            lbVersion.Text = "Version";
            // 
            // lbTags
            // 
            lbTags.AutoSize = true;
            lbTags.ForeColor = SystemColors.ButtonFace;
            lbTags.Location = new Point(10, 153);
            lbTags.Name = "lbTags";
            lbTags.Size = new Size(38, 15);
            lbTags.TabIndex = 10;
            lbTags.Text = "Tag(s)";
            // 
            // lbSource
            // 
            lbSource.AutoSize = true;
            lbSource.ForeColor = SystemColors.ButtonFace;
            lbSource.Location = new Point(10, 297);
            lbSource.Name = "lbSource";
            lbSource.Size = new Size(43, 15);
            lbSource.TabIndex = 12;
            lbSource.Text = "Source";
            // 
            // btSourceSelect
            // 
            btSourceSelect.FlatStyle = FlatStyle.Popup;
            btSourceSelect.ForeColor = SystemColors.ButtonFace;
            btSourceSelect.Location = new Point(537, 315);
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
            lbDescription.Location = new Point(650, 57);
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
            btCreate.Location = new Point(1177, 406);
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
            lbIcon.Location = new Point(535, 57);
            lbIcon.Name = "lbIcon";
            lbIcon.Size = new Size(30, 15);
            lbIcon.TabIndex = 19;
            lbIcon.Text = "Icon";
            // 
            // pcSlidingView
            // 
            pcSlidingView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pcSlidingView.BackColor = Color.FromArgb(32, 32, 32);
            pcSlidingView.Controls.Add(lbCreatePackage2);
            pcSlidingView.Controls.Add(lbCreatePackage);
            pcSlidingView.Controls.Add(textBox1);
            pcSlidingView.Controls.Add(lbDependencies);
            pcSlidingView.Controls.Add(btNext);
            pcSlidingView.Controls.Add(xbExpectOverwrites);
            pcSlidingView.Controls.Add(btBack);
            pcSlidingView.Controls.Add(tbDescription);
            pcSlidingView.Controls.Add(lbIcon);
            pcSlidingView.Controls.Add(tbSource);
            pcSlidingView.Controls.Add(lbDescription);
            pcSlidingView.Controls.Add(tbVersion);
            pcSlidingView.Controls.Add(tbTags);
            pcSlidingView.Controls.Add(pbIcon);
            pcSlidingView.Controls.Add(lbSource);
            pcSlidingView.Controls.Add(tbAuthors);
            pcSlidingView.Controls.Add(btCancel);
            pcSlidingView.Controls.Add(tbName);
            pcSlidingView.Controls.Add(btCreate);
            pcSlidingView.Controls.Add(lbTags);
            pcSlidingView.Controls.Add(btSourceSelect);
            pcSlidingView.Controls.Add(lbVersion);
            pcSlidingView.Controls.Add(lbProjectName);
            pcSlidingView.Controls.Add(lbAuthor);
            pcSlidingView.Location = new Point(0, 0);
            pcSlidingView.Margin = new Padding(0);
            pcSlidingView.MaximumSize = new Size(1264, 441);
            pcSlidingView.MinimumSize = new Size(1264, 441);
            pcSlidingView.Name = "pcSlidingView";
            pcSlidingView.Size = new Size(1264, 441);
            pcSlidingView.TabIndex = 21;
            // 
            // lbCreatePackage2
            // 
            lbCreatePackage2.AutoSize = true;
            lbCreatePackage2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreatePackage2.ForeColor = SystemColors.ButtonFace;
            lbCreatePackage2.Location = new Point(652, 9);
            lbCreatePackage2.Name = "lbCreatePackage2";
            lbCreatePackage2.Size = new Size(218, 30);
            lbCreatePackage2.TabIndex = 26;
            lbCreatePackage2.Text = "Create New Package...";
            // 
            // lbDependencies
            // 
            lbDependencies.AutoSize = true;
            lbDependencies.ForeColor = SystemColors.ButtonFace;
            lbDependencies.Location = new Point(10, 201);
            lbDependencies.Name = "lbDependencies";
            lbDependencies.Size = new Size(94, 15);
            lbDependencies.TabIndex = 24;
            lbDependencies.Text = "Dependencies(s)";
            // 
            // btNext
            // 
            btNext.FlatStyle = FlatStyle.Popup;
            btNext.ForeColor = SystemColors.ButtonFace;
            btNext.Location = new Point(537, 406);
            btNext.Name = "btNext";
            btNext.Size = new Size(75, 23);
            btNext.TabIndex = 23;
            btNext.Text = "&Next";
            btNext.UseVisualStyleBackColor = true;
            btNext.Click += OnNextPage;
            // 
            // btBack
            // 
            btBack.FlatStyle = FlatStyle.Popup;
            btBack.ForeColor = SystemColors.ButtonFace;
            btBack.Location = new Point(1096, 406);
            btBack.Name = "btBack";
            btBack.Size = new Size(75, 23);
            btBack.TabIndex = 22;
            btBack.Text = "&Back";
            btBack.UseVisualStyleBackColor = true;
            btBack.Click += OnPrevPage;
            // 
            // PackageCreateDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(pcSlidingView);
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
            pcSlidingView.ResumeLayout(false);
            pcSlidingView.PerformLayout();
            ResumeLayout(false);
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
        private Panel pcSlidingView;
        private Button btBack;
        private Button btNext;
        private TextBox textBox1;
        private Label lbDependencies;
        private Label lbCreatePackage2;
    }
}