using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Instances;
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
            // Make sure we have at least once instance before allowing project creation...
            if(Program.InstanceManager.InstanceCount <= 0)
            {
                MessageBox.Show("You must create an instance first!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create and open the project create dialog
            using ProjectCreateDialog pcd = new();

            if (pcd.ShowDialog() != DialogResult.OK || pcd.CreationArguments == null)
                return;

            // We're getting busy...
            BusyDialog.Instance.ShowBusy();

            // Create the project
            Program.ProjectManager.CreateProject(pcd.CreationArguments);

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
            // Make sure we have at least once instance before allowing project creation...
            if (Program.InstanceManager.InstanceCount <= 0)
            {
                MessageBox.Show("You must create an instance first!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
        /// <param name="project">The target project</param>
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
        /// Called when the user choses to configure a project
        /// </summary>
        /// <param name="project">The target project</param>
        void OnProjectSettings(Project project)
        {
            MessageBox.Show("Unimplemented", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Called when the user chooses to manage a projects packages
        /// </summary>
        /// <param name="project">The target project</param>
        void OnProjectPackages(Project project)
        {
            // Create and open the package manager
            using PackageManagerDialog pmd = new(project);
            pmd.ShowDialog();
        }

        /// <summary>
        /// Called when the user chooses to generate a runtime for a project
        /// </summary>
        /// <param name="project">The target project</param>
        void OnProjectGenerateRuntime(Project project)
        {
            // Get the instance connected with this project...
            Instance? projectInstance;

            if (!Program.InstanceManager.FindInstanceByUUID(project.InstanceUUID, out projectInstance) || projectInstance == null)
            {
                MessageBox.Show($"Couldn't find instance with UUID '{project.InstanceUUID}'! Cannot generate runtime!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Unimplemented", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Called when a user chooses to open a project
        /// </summary>
        /// <param name="project">The target project</param>
        void OnProjectOpen(Project project)
        {
            // Get the instance connected with this project...
            Instance? projectInstance;

            if(!Program.InstanceManager.FindInstanceByUUID(project.InstanceUUID, out projectInstance) || projectInstance == null)
            {
                MessageBox.Show($"Couldn't find instance with UUID '{project.InstanceUUID}'! Cannot open project!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // If the instance doesn't have a core package, we can't open a project with it...
            if(!projectInstance.HasCorePackage())
            {
                MessageBox.Show($"The instance doesn't have a core package, you must install one with the package manager!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }            

            // Now we have an instance reference, set it in the registery
            projectInstance.OpenProject(project);
        }
    }
}
