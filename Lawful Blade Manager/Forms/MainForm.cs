using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            EnumurateProjects();
        }

        void EnumurateProjects()
        {
            // Restore project controls to cache...
            CacheProjectControls();

            foreach (Project project in Program.ProjectManager.Projects)
            {
                // Before we create new controls, try and grab one from the cache
                ProjectControl? projectControl;

                if (!projectControlCache.TryDequeue(out projectControl))
                {
                    // We failed to de-queue, so we must create a new control...
                    projectControl = new ProjectControl(project)
                    {
                        Dock = DockStyle.Top,
                    };

                    projectControl.OnProjectDelete += OnProjectDelete;
                    projectControl.OnProjectManagePackages += OnProjectPackages;
                    projectControl.OnProjectGenerateRuntime += OnProjectGenerateRuntime;
                    projectControl.OnProjectOpen += OnProjectOpen;
                }
                else
                    // We recovered a project control from the cache, load it with info
                    projectControl.LoadProject(project);

                // Add project control to list
                pcProjectList.Controls.Add(projectControl);
            }
        }

        void CacheProjectControls()
        {
            // Add each project control back into the cache
            foreach (ProjectControl projectControl in pcProjectList.Controls)
            {
                // Clear it...
                projectControl.Reset();

                // Queue it...
                projectControlCache.Enqueue(projectControl);
            }

            // Clear the project controls list...
            pcProjectList.Controls.Clear();
        }
    }
}