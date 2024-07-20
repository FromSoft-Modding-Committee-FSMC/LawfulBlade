namespace LawfulBladeManager.Dialog
{
    partial class RepositoryCreateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryCreateDialog));
            lbCreatePackage = new Label();
            tbName = new TextBox();
            lbProjectName = new Label();
            ttMain = new ToolTip(components);
            tbDescription = new TextBox();
            btBundleAdd = new Button();
            btBundleRemove = new Button();
            lvBundles = new ListView();
            clBundleFile = new ColumnHeader();
            clBundleSizeMB = new ColumnHeader();
            btCancel = new Button();
            btCreate = new Button();
            lbDescription = new Label();
            gbBundles = new GroupBox();
            gbBundles.SuspendLayout();
            SuspendLayout();
            // 
            // lbCreatePackage
            // 
            lbCreatePackage.AutoSize = true;
            lbCreatePackage.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbCreatePackage.ForeColor = SystemColors.ButtonFace;
            lbCreatePackage.Location = new Point(12, 9);
            lbCreatePackage.Name = "lbCreatePackage";
            lbCreatePackage.Size = new Size(239, 30);
            lbCreatePackage.TabIndex = 1;
            lbCreatePackage.Text = "Create New Repository...";
            // 
            // tbName
            // 
            tbName.BackColor = Color.FromArgb(32, 32, 32);
            tbName.BorderStyle = BorderStyle.FixedSingle;
            tbName.ForeColor = SystemColors.ButtonFace;
            tbName.Location = new Point(12, 74);
            tbName.MaxLength = 32;
            tbName.Name = "tbName";
            tbName.PlaceholderText = "My sick repository";
            tbName.Size = new Size(600, 23);
            tbName.TabIndex = 4;
            ttMain.SetToolTip(tbName, "The name of the repository (max 32 characters)");
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
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(32, 32, 32);
            tbDescription.ForeColor = SystemColors.ButtonFace;
            tbDescription.Location = new Point(12, 122);
            tbDescription.MaxLength = 128;
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.PlaceholderText = "Write something in me";
            tbDescription.ScrollBars = ScrollBars.Vertical;
            tbDescription.Size = new Size(600, 75);
            tbDescription.TabIndex = 15;
            ttMain.SetToolTip(tbDescription, "A description of the repository (max of 128 characters)");
            // 
            // btBundleAdd
            // 
            btBundleAdd.BackColor = Color.FromArgb(32, 192, 32);
            btBundleAdd.FlatStyle = FlatStyle.Popup;
            btBundleAdd.ForeColor = SystemColors.ButtonFace;
            btBundleAdd.Location = new Point(519, 127);
            btBundleAdd.Name = "btBundleAdd";
            btBundleAdd.Size = new Size(75, 23);
            btBundleAdd.TabIndex = 13;
            btBundleAdd.Text = "Add...";
            ttMain.SetToolTip(btBundleAdd, "Add a bundle to the repository");
            btBundleAdd.UseVisualStyleBackColor = false;
            btBundleAdd.Click += OnAddRemoveBundle;
            // 
            // btBundleRemove
            // 
            btBundleRemove.BackColor = Color.FromArgb(192, 32, 32);
            btBundleRemove.FlatStyle = FlatStyle.Popup;
            btBundleRemove.ForeColor = SystemColors.ButtonFace;
            btBundleRemove.Location = new Point(519, 156);
            btBundleRemove.Name = "btBundleRemove";
            btBundleRemove.Size = new Size(75, 23);
            btBundleRemove.TabIndex = 19;
            btBundleRemove.Text = "Remove...";
            ttMain.SetToolTip(btBundleRemove, "Remove the selected item from the repository");
            btBundleRemove.UseVisualStyleBackColor = false;
            btBundleRemove.Click += OnAddRemoveBundle;
            // 
            // lvBundles
            // 
            lvBundles.Activation = ItemActivation.OneClick;
            lvBundles.BackColor = Color.FromArgb(32, 32, 32);
            lvBundles.BorderStyle = BorderStyle.FixedSingle;
            lvBundles.Columns.AddRange(new ColumnHeader[] { clBundleFile, clBundleSizeMB });
            lvBundles.ForeColor = SystemColors.ButtonFace;
            lvBundles.FullRowSelect = true;
            lvBundles.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvBundles.LabelWrap = false;
            lvBundles.Location = new Point(6, 15);
            lvBundles.Name = "lvBundles";
            lvBundles.ShowGroups = false;
            lvBundles.Size = new Size(507, 164);
            lvBundles.TabIndex = 20;
            ttMain.SetToolTip(lvBundles, "List of bundles to include in the repository");
            lvBundles.UseCompatibleStateImageBehavior = false;
            lvBundles.View = View.Details;
            // 
            // clBundleFile
            // 
            clBundleFile.Text = "File";
            clBundleFile.Width = 420;
            // 
            // clBundleSizeMB
            // 
            clBundleSizeMB.Text = "Size (MB)";
            clBundleSizeMB.TextAlign = HorizontalAlignment.Center;
            clBundleSizeMB.Width = 85;
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
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.ForeColor = SystemColors.ButtonFace;
            lbDescription.Location = new Point(9, 103);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(67, 15);
            lbDescription.TabIndex = 5;
            lbDescription.Text = "Description";
            // 
            // gbBundles
            // 
            gbBundles.BackColor = Color.FromArgb(24, 24, 24);
            gbBundles.Controls.Add(btBundleRemove);
            gbBundles.Controls.Add(btBundleAdd);
            gbBundles.Controls.Add(lvBundles);
            gbBundles.ForeColor = SystemColors.ButtonFace;
            gbBundles.Location = new Point(12, 203);
            gbBundles.Name = "gbBundles";
            gbBundles.Size = new Size(600, 185);
            gbBundles.TabIndex = 21;
            gbBundles.TabStop = false;
            gbBundles.Text = "Bundles";
            // 
            // RepositoryCreateDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(btCancel);
            Controls.Add(btCreate);
            Controls.Add(tbDescription);
            Controls.Add(lbDescription);
            Controls.Add(tbName);
            Controls.Add(lbProjectName);
            Controls.Add(lbCreatePackage);
            Controls.Add(gbBundles);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "RepositoryCreateDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Repository Creator";
            TopMost = true;
            gbBundles.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbCreatePackage;
        private TextBox tbName;
        private Label lbProjectName;
        private ToolTip ttMain;
        private Button btBundleAdd;
        private TextBox tbDescription;
        private Button btCancel;
        private Button btCreate;
        private Label lbDescription;
        private Button btBundleRemove;
        private ListView lvBundles;
        private ColumnHeader clBundleFile;
        private ColumnHeader clBundleSizeMB;
        private GroupBox gbBundles;
    }
}