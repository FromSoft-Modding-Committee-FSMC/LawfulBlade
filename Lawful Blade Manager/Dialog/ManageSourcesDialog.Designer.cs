namespace LawfulBladeManager.Dialog
{
    partial class ManageSourcesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageSourcesDialog));
            lbTitle = new Label();
            ttPrimary = new ToolTip(components);
            lvSourceList = new ListView();
            clSourceURI = new ColumnHeader();
            clSourceDate = new ColumnHeader();
            clSourcePackageCount = new ColumnHeader();
            btSourceAdd = new Button();
            btSourceRemove = new Button();
            tbSourceField = new TextBox();
            SuspendLayout();
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbTitle.ForeColor = SystemColors.ButtonFace;
            lbTitle.Location = new Point(12, 9);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(182, 30);
            lbTitle.TabIndex = 0;
            lbTitle.Text = "Manage Sources...";
            // 
            // lvSourceList
            // 
            lvSourceList.BackColor = Color.FromArgb(32, 32, 32);
            lvSourceList.Columns.AddRange(new ColumnHeader[] { clSourceURI, clSourceDate, clSourcePackageCount });
            lvSourceList.ForeColor = SystemColors.ButtonFace;
            lvSourceList.FullRowSelect = true;
            lvSourceList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvSourceList.Location = new Point(12, 42);
            lvSourceList.MultiSelect = false;
            lvSourceList.Name = "lvSourceList";
            lvSourceList.Size = new Size(600, 289);
            lvSourceList.TabIndex = 2;
            ttPrimary.SetToolTip(lvSourceList, "All current sources registered with Lawful Blade.");
            lvSourceList.UseCompatibleStateImageBehavior = false;
            lvSourceList.View = View.Details;
            // 
            // clSourceURI
            // 
            clSourceURI.Text = "URI";
            clSourceURI.Width = 300;
            // 
            // clSourceDate
            // 
            clSourceDate.Text = "Creation Date";
            clSourceDate.Width = 128;
            // 
            // clSourcePackageCount
            // 
            clSourcePackageCount.Text = "Package Count";
            clSourcePackageCount.TextAlign = HorizontalAlignment.Center;
            clSourcePackageCount.Width = 128;
            // 
            // btSourceAdd
            // 
            btSourceAdd.BackColor = Color.FromArgb(32, 192, 32);
            btSourceAdd.FlatStyle = FlatStyle.Popup;
            btSourceAdd.ForeColor = SystemColors.ButtonFace;
            btSourceAdd.Location = new Point(456, 337);
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
            btSourceRemove.Location = new Point(537, 337);
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
            tbSourceField.Location = new Point(12, 337);
            tbSourceField.Name = "tbSourceField";
            tbSourceField.Size = new Size(438, 23);
            tbSourceField.TabIndex = 3;
            // 
            // ManageSourcesDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(624, 441);
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
            Name = "ManageSourcesDialog";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lawful Blade - Manage Sources...";
            TopMost = true;
            Load += OnDialogLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbTitle;
        private ToolTip ttPrimary;
        private ListView lvSourceList;
        private ColumnHeader clSourceDate;
        private ColumnHeader clSourceURI;
        private ColumnHeader clSourcePackageCount;
        private TextBox tbSourceField;
        private Button btSourceAdd;
        private Button btSourceRemove;
    }
}