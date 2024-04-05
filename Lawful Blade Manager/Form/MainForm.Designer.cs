namespace LawfulBladeManager
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            msMain = new MenuStrip();
            msMainFile = new ToolStripMenuItem();
            msMainExit = new ToolStripMenuItem();
            msMainEdit = new ToolStripMenuItem();
            msMainPackages = new ToolStripMenuItem();
            msMainCreatePackage = new ToolStripMenuItem();
            tcMain = new TabControl();
            tpProjects = new TabPage();
            pcProjectList = new Panel();
            pcProjectButtons = new Panel();
            button1 = new Button();
            btNewProject = new Button();
            tpInstances = new TabPage();
            msMain.SuspendLayout();
            tcMain.SuspendLayout();
            tpProjects.SuspendLayout();
            pcProjectButtons.SuspendLayout();
            SuspendLayout();
            // 
            // msMain
            // 
            msMain.Items.AddRange(new ToolStripItem[] { msMainFile, msMainEdit, msMainPackages });
            msMain.Location = new Point(0, 0);
            msMain.Name = "msMain";
            msMain.Size = new Size(800, 24);
            msMain.TabIndex = 0;
            msMain.Text = "menuStrip1";
            // 
            // msMainFile
            // 
            msMainFile.DropDownItems.AddRange(new ToolStripItem[] { msMainExit });
            msMainFile.Name = "msMainFile";
            msMainFile.Size = new Size(37, 20);
            msMainFile.Text = "File";
            // 
            // msMainExit
            // 
            msMainExit.Name = "msMainExit";
            msMainExit.Size = new Size(180, 22);
            msMainExit.Text = "Exit";
            msMainExit.Click += msMainExit_Click;
            // 
            // msMainEdit
            // 
            msMainEdit.Name = "msMainEdit";
            msMainEdit.Size = new Size(39, 20);
            msMainEdit.Text = "Edit";
            // 
            // msMainPackages
            // 
            msMainPackages.DropDownItems.AddRange(new ToolStripItem[] { msMainCreatePackage });
            msMainPackages.Name = "msMainPackages";
            msMainPackages.Size = new Size(68, 20);
            msMainPackages.Text = "Packages";
            // 
            // msMainCreatePackage
            // 
            msMainCreatePackage.Name = "msMainCreatePackage";
            msMainCreatePackage.Size = new Size(164, 22);
            msMainCreatePackage.Text = "Create Package...";
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpProjects);
            tcMain.Controls.Add(tpInstances);
            tcMain.Dock = DockStyle.Fill;
            tcMain.Location = new Point(0, 24);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.Size = new Size(800, 426);
            tcMain.TabIndex = 1;
            // 
            // tpProjects
            // 
            tpProjects.BackColor = Color.FromArgb(16, 16, 16);
            tpProjects.Controls.Add(pcProjectList);
            tpProjects.Controls.Add(pcProjectButtons);
            tpProjects.Location = new Point(4, 24);
            tpProjects.Name = "tpProjects";
            tpProjects.Padding = new Padding(3);
            tpProjects.Size = new Size(792, 398);
            tpProjects.TabIndex = 0;
            tpProjects.Text = "Projects";
            // 
            // pcProjectList
            // 
            pcProjectList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pcProjectList.BackColor = Color.FromArgb(8, 8, 8);
            pcProjectList.BorderStyle = BorderStyle.FixedSingle;
            pcProjectList.Location = new Point(236, 3);
            pcProjectList.Name = "pcProjectList";
            pcProjectList.Size = new Size(553, 392);
            pcProjectList.TabIndex = 1;
            // 
            // pcProjectButtons
            // 
            pcProjectButtons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pcProjectButtons.BackColor = Color.FromArgb(8, 8, 8);
            pcProjectButtons.BorderStyle = BorderStyle.FixedSingle;
            pcProjectButtons.Controls.Add(button1);
            pcProjectButtons.Controls.Add(btNewProject);
            pcProjectButtons.Location = new Point(3, 3);
            pcProjectButtons.Name = "pcProjectButtons";
            pcProjectButtons.Padding = new Padding(2, 5, 2, 5);
            pcProjectButtons.Size = new Size(230, 392);
            pcProjectButtons.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(48, 48, 48);
            button1.Dock = DockStyle.Top;
            button1.FlatAppearance.BorderColor = Color.Black;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Image = Properties.Resources.browsefolder_lb;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(2, 51);
            button1.Margin = new Padding(3, 3, 3, 10);
            button1.Name = "button1";
            button1.Padding = new Padding(10, 0, 0, 0);
            button1.Size = new Size(224, 46);
            button1.TabIndex = 1;
            button1.Text = "    &Import Project";
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btNewProject
            // 
            btNewProject.BackColor = Color.FromArgb(48, 48, 48);
            btNewProject.Dock = DockStyle.Top;
            btNewProject.FlatAppearance.BorderColor = Color.Black;
            btNewProject.FlatStyle = FlatStyle.Flat;
            btNewProject.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btNewProject.ForeColor = SystemColors.ButtonFace;
            btNewProject.Image = Properties.Resources.newfile_lb;
            btNewProject.ImageAlign = ContentAlignment.MiddleLeft;
            btNewProject.Location = new Point(2, 5);
            btNewProject.Margin = new Padding(3, 3, 3, 10);
            btNewProject.Name = "btNewProject";
            btNewProject.Padding = new Padding(10, 0, 0, 0);
            btNewProject.Size = new Size(224, 46);
            btNewProject.TabIndex = 0;
            btNewProject.Text = "    &Create Project";
            btNewProject.TextImageRelation = TextImageRelation.ImageBeforeText;
            btNewProject.UseVisualStyleBackColor = false;
            btNewProject.Click += btNewProject_Click;
            // 
            // tpInstances
            // 
            tpInstances.Location = new Point(4, 24);
            tpInstances.Name = "tpInstances";
            tpInstances.Padding = new Padding(3);
            tpInstances.Size = new Size(792, 398);
            tpInstances.TabIndex = 1;
            tpInstances.Text = "Instances";
            tpInstances.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(800, 450);
            Controls.Add(tcMain);
            Controls.Add(msMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = msMain;
            Name = "FormMain";
            Text = "Lawful Blade Manager";
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            tcMain.ResumeLayout(false);
            tpProjects.ResumeLayout(false);
            pcProjectButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip msMain;
        private ToolStripMenuItem msMainFile;
        private TabControl tcMain;
        private TabPage tpProjects;
        private TabPage tpInstances;
        private ToolStripMenuItem msMainEdit;
        private Panel pcProjectButtons;
        private Button btNewProject;
        private Button button1;
        private Panel pcProjectList;
        private ToolStripMenuItem msMainPackages;
        private ToolStripMenuItem msMainCreatePackage;
        private ToolStripMenuItem msMainExit;
    }
}