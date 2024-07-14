using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Forms
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
            // This won't happen, but VS doesn't STFU about it.
            if (Program.ProjectManager == null)
                return;

            // Show Dialog
            using (ProjectCreateDialog pcd = new())
            {
                switch (pcd.ShowDialog())
                {
                    case DialogResult.OK:

                        // Create the project
                        BusyDialog.Instance.ShowBusy();

                        // refactor this shit jfc...
                        Program.ProjectManager.CreateProject(new ProjectCreationArgs { Name = pcd.ProjectName, Description = pcd.ProjectDescription, Destination = pcd.TargetPath, InstanceUUID = pcd.TargetInstance, CreateEmpty = pcd.EmptyProject });

                        BusyDialog.Instance.HideBusy();
                        break;
                }
            }

            EnumurateProjectsInList();
        }

        private void btLocalProject_Click(object sender, EventArgs e)
        {
            if (Program.ProjectManager == null)
                return;

            // Show Dialog
            using (OpenFileDialog ofd = new())
            {
                ofd.Filter = "Sword of Moonlight Project (*.som)|*.som";

                switch (ofd.ShowDialog())
                {
                    case DialogResult.OK:
                        Program.ProjectManager.ImportProject(ofd.FileName);
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
        #region Menu Strip - Packages
        private void msMainCreatePackage_Click(object sender, EventArgs e)
        {
            if (Program.PackageManager == null)
                return;

            using (PackageCreateDialog pcd = new())
            {
                if (pcd.ShowDialog() == DialogResult.OK)
                {
                    BusyDialog.Instance.ShowBusy();
                    PackageManager.PackageCreate(new PackageCreateArgs
                    {
                        Name = pcd.PackageName,
                        Description = pcd.PackageDescription,
                        Version = pcd.PackageVersion,
                        Authors = pcd.PackageAuthors,
                        Tags = pcd.PackageTags,
                        Icon = pcd.PackageIcon,

                        SourceDirectory = pcd.PackageSource,
                        TargetFile = pcd.PackageOutput
                    });
                    BusyDialog.Instance.HideBusy();
                }
            }
        }

        private void msMainPackageToolGDD_Click(object sender, EventArgs e)
        {
            using (PackageDeltaDialog pdf = new())
                pdf.ShowDialog();
        }
        #endregion

        #region Project Callbacks
        void EnumurateProjectsInList()
        {
            // Exit early if the project list is null.
            if (Program.ProjectManager == null || Program.ProjectManager.Projects == null)
                return;

            // Clear previous project controls
            DestroyProjectsInList();

            foreach (Project project in Program.ProjectManager.Projects)
            {
                ProjectControl projectControl = new(project)
                {
                    Dock = DockStyle.Top
                };

                projectControl.OnProjectDelete += OnProjectDelete;

                pcProjectList.Controls.Add(projectControl);
            }
        }

        void DestroyProjectsInList()
        {
            foreach(ProjectControl projectControl in pcProjectList.Controls)
            {
                projectControl.OnProjectDelete -= OnProjectDelete;

                projectControl.ClearRetainedData();
                projectControl.Dispose();
            }

            pcProjectList.Controls.Clear();
        }


        void OnProjectDelete(ProjectControl projectControl)
        {
            DestroyProjectsInList();
            EnumurateProjectsInList();
        }
        #endregion
    }
}