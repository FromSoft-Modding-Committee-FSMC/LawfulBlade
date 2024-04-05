using LawfulBladeManager.Dialog;
using LawfulBladeManager.Project;

namespace LawfulBladeManager
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        #region Main Tab View - Projects
        private void btNewProject_Click(object sender, EventArgs e)
        {
            // Show Dialog
            using (ProjectCreateDialog pcd = new())
            {
                switch (pcd.ShowDialog())
                {
                    case DialogResult.OK:

                        // Create the project
                        Program.Context.projectManager.CreateProject(pcd.TargetPath, pcd.ProjectName, pcd.ProjectDescription);

                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
        }
        #endregion

        #region Menu Strip - File
        private void msMainExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}