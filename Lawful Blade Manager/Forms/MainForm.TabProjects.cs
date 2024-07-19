using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Forms
{

    // We use these (slightly awkward with Winforms) partial classes to define callbacks for various tabs
    public partial class MainForm
    {
        /// <summary>
        /// We use a cache so we're not hitting GC as much
        /// </summary>
        readonly Queue<ProjectControl> projectControlCache = new ();

        /// <summary>
        /// Called when the user chooses to create a new project
        /// </summary>
        void OnProjectNew(object sender, EventArgs args)
        {
            // Create and open the project create dialog
            using ProjectCreateDialog pcd = new();

            if (pcd.ShowDialog() != DialogResult.OK)
                return;

            // We're getting busy...
            BusyDialog.Instance.ShowBusy();

            // Create the project -- still TODO: refactor
            Program.ProjectManager.CreateProject(new ProjectCreateArgs { Name = pcd.ProjectName, Description = pcd.ProjectDescription, Destination = pcd.TargetPath, InstanceUUID = pcd.TargetInstance, CreateEmpty = pcd.EmptyProject });

            // No longer busy
            BusyDialog.Instance.HideBusy();

            // We must now re-enumurate
            EnumurateProjects();
        }

        /// <summary>
        /// Called when the user chooses to import a legacy project
        /// </summary>
        void OnProjectImport(object sender, EventArgs args)
        {
            // Create and show an open file dialog
            using OpenFileDialog ofd = new()
            {
                Filter          = "Sword of Moonlight Project (*.som)|*.som",
                CheckPathExists = true,
                CheckFileExists = true
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            // Import the project
            Program.ProjectManager.ImportProject(ofd.FileName);

            // We must now re-enumurate
            EnumurateProjects();
        }

        /// <summary>
        /// Called when the user chooses to delete a project
        /// </summary>
        /// <param name="projectControl"></param>
        void OnProjectDelete(Project project)
        {
            // Prompt the user on if they're sure
            if (MessageBox.Show($"Are you sure you want to delete '{project.Name}' in '{project.StoragePath}'?\n\nYou will not be able to recover the project if you do!", "Lawful Blade", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            // WIP...
            return;

            // Try to delete the project...
            try
            {
                Program.ProjectManager?.DeleteProject(ref project);
            }
            catch (Exception ex)
            {
                Logger.ShortError(ex.Message);
            }

            // enumurate projects again
            EnumurateProjects();
        }

        /// <summary>
        /// Called when the user chooses to manage a projects packages
        /// </summary>
        void OnProjectPackages(Project project)
        {
            // Create and open the package manager
            using PackageManagerDialog pmd = new(project);
            pmd.ShowDialog();
        }

        /// <summary>
        /// Called when the user chooses to generate a runtime for a project
        /// </summary>
        void OnProjectGenerateRuntime(Project project)
        {

        }

        /// <summary>
        /// Called when a user chooses to open a project
        /// </summary>
        void OnProjectOpen(Project project)
        {

        }
    }
}
