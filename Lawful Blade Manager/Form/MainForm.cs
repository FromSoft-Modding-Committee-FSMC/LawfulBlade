using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Project;

namespace LawfulBladeManager
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            EnumurateProjectsInList();
        }

        #region Main Tab View - Projects
        private void btNewProject_Click(object sender, EventArgs e)
        {
            // This won't happen, but VS doesn't stfu about it.
            if (Program.ProjectManager == null)
                return;

            // Show Dialog
            using (ProjectCreateDialog pcd = new())
            {
                switch (pcd.ShowDialog())
                {
                    case DialogResult.OK:

                        // Create the project
                        Program.ProjectManager.CreateProject(pcd.TargetPath, pcd.ProjectName, pcd.ProjectDescription);

                        break;
                }
            }

            EnumurateProjectsInList();
        }

        private void btLocalProject_Click(object sender, EventArgs e)
        {
            // Show Dialog
            using (OpenFileDialog ofd = new())
            {
                ofd.Filter = "Sword of Moonlight Project (*.som)|*.som";

                switch (ofd.ShowDialog())
                {
                    case DialogResult.OK:

                        break;
                }
            }

            EnumurateProjectsInList();
        }

        private void btPackageProject_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Menu Strip - File
        private void msMainExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        void EnumurateProjectsInList()
        {
            // Exit early if the project list is null.
            if (Program.ProjectManager == null || Program.ProjectManager.Projects == null)
                return;

            // Clear previous project controls
            pcProjectList.Controls.Clear();

            foreach (Project.Project project in Program.ProjectManager.Projects)
            {
                pcProjectList.Controls.Add(new ProjectControl(project)
                {
                    Dock = DockStyle.Top
                });
            }
        }
    }
}