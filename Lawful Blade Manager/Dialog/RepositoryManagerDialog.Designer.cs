namespace LawfulBladeManager.Dialog
{
    partial class RepositoryManagerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryManagerDialog));
            lbTitle = new Label();
            ttPrimary = new ToolTip(components);
            lvSourceList = new ListView();
            clName = new ColumnHeader();
            clDescription = new ColumnHeader();
            clCreationDate = new ColumnHeader();
            clURI = new ColumnHeader();
            clPackageCount = new ColumnHeader();
            btSourceAdd = new Button();
            btSourceRemove = new Button();
            tbSourceField = new TextBox();
            btDone = new Button();
            SuspendLayout();
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbTitle.ForeColor = SystemColors.ButtonFace;
            lbTitle.Location = new Point(12, 9);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(222, 30);
            lbTitle.TabIndex = 0;
            lbTitle.Text = "Manage Repositories...";
            // 
            // lvSourceList
            // 
            lvSourceList.BackColor = Color.FromArgb(32, 32, 32);
            lvSourceList.Columns.AddRange(new ColumnHeader[] { clName, clDescription, clCreationDate, clURI, clPackageCount });
            lvSourceList.ForeColor = SystemColors.ButtonFace;
            lvSourceList.FullRowSelect = true;
            lvSourceList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvSourceList.Location = new Point(12, 42);
            lvSourceList.MultiSelect = false;
            lvSourceList.Name = "lvSourceList";
            lvSourceList.Size = new Size(600, 329);
            lvSourceList.TabIndex = 2;
            ttPrimary.SetToolTip(lvSourceList, "All current sources registered with Lawful Blade.");
            lvSourceList.UseCompatibleStateImageBehavior = false;
            lvSourceList.View = View.Details;
            // 
            // clName
            // 
            clName.Text = "Name";
            clName.Width = 96;
            // 
            // clDescription
            // 
            clDescription.Text = "Description";
            clDescription.Width = 256;
            // 
            // clCreationDate
            // 
            clCreationDate.Text = "Creation Date";
            clCreationDate.Width = 128;
            // 
            // clURI
            // 
            clURI.Text = "URI";
            clURI.Width = 256;
            // 
            // clPackageCount
            // 
            clPackageCount.Text = "Package Count";
            clPackageCount.TextAlign = HorizontalAlignment.Center;
            clPackageCount.Width = 96;
            // 
            // btSourceAdd
            // 
            btSourceAdd.BackColor = Color.FromArgb(32, 192, 32);
            btSourceAdd.FlatStyle = FlatStyle.Popup;
            btSourceAdd.ForeColor = SystemColors.ButtonFace;
            btSourceAdd.Location = new Point(456, 377);
            btSourceAdd.Name = "btSourceAdd";
            btSourceAdd.Size = new Size(75, 23);
            btSourceAdd.TabIndex = 12;
            btSourceAdd.Text = "Add";
            ttPrimary.SetToolTip(btSourceAdd, "Adds the source from the text box to the left.");
            btSourceAdd.UseVisualStyleBackColor = false;
            btSourceAdd.Click += OnClickSourceAdd;
            // 
            // btSourceRemove
            // 
            btSourceRemove.BackColor = Color.FromArgb(192, 32, 32);
            btSourceRemove.FlatStyle = FlatStyle.Popup;
            btSourceRemove.ForeColor = SystemColors.ButtonFace;
            btSourceRemove.Location = new Point(537, 377);
            btSourceRemove.Name = "btSourceRemove";
            btSourceRemove.Size = new Size(75, 23);
            btSourceRemove.TabIndex = 13;
            btSourceRemove.Text = "Remove";
            ttPrimary.SetToolTip(btSourceRemove, "Removes the selected source.");
            btSourceRemove.UseVisualStyleBackColor = false;
            // 
            // tbSourceField
            // 
            tbSourceField.BackColor = Color.FromArgb(32, 32, 32);
            tbSourceField.BorderStyle = BorderStyle.FixedSingle;
            tbSourceField.ForeColor = SystemColors.ButtonFace;
            tbSourceField.Location = new Point(12, 377);
            tbSourceField.Name = "tbSourceField";
            tbSourceField.Size = new Size(438, 23);
            tbSourceField.TabIndex = 3;
            // 
            // btDone
            // 
            btDone.BackColor = Color.FromArgb(120, 66, 135);
            btDone.FlatStyle = FlatStyle.Popup;
            btDone.ForeColor = SystemColors.ButtonFace;
            btDone.Location = new Point(537, 406);
            btDone.Name = "btDone";
            btDone.Size = new Size(75, 23);
            btDone.TabIndex = 14;
            btDone.Text = "Done";
            btDone.UseVisualStyleBackColor = false;
            btDone.Click += OnClickDone;
            // 
            // RepositoryManagerDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
            Controls.Add(btDone);
            Controls.Add(btSourceRemove);
            Controls.Add(btSourceAdd);
            Controls.Add(tbSourceField);
            Controls.Add(lvSourceList);
            Controls.Add(lbTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(640, 480);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(640, 480);
            Name = "RepositoryManagerDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Manage Repositories...";
            TopMost = true;
            Load += OnDialogLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbTitle;
        private ToolTip ttPrimary;
        private ListView lvSourceList;
        private TextBox tbSourceField;
        private Button btSourceAdd;
        private Button btSourceRemove;
        private Button btDone;
        private ColumnHeader clName;
        private ColumnHeader clDescription;
        private ColumnHeader clCreationDate;
        private ColumnHeader clURI;
        private ColumnHeader clPackageCount;
    }
}