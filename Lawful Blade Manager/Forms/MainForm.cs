using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Instances;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;
using System.Diagnostics;

namespace LawfulBladeManager.Forms
{
    public partial class MainForm : Form
    {
        public MainForm() =>
            InitializeComponent();

        /// <summary>
        /// Enumurate any projects managed by lawful blade
        /// </summary>
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
                    projectControl.OnProjectSettings += OnProjectSettings;
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

        /// <summary>
        /// Cache all current project controls for reuse in enumuration
        /// </summary>
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

        /// <summary>
        /// Enumurate any instances managed by lawful blade
        /// </summary>
        void EnumurateInstances()
        {
            // Restore instance controls to cache...
            CacheInstanceControls();

            foreach (Instance instance in Program.InstanceManager.Instances)
            {
                // Before we create new controls, try and grab one from the cache
                InstanceControl? instanceControl;

                if (!instanceControlCache.TryDequeue(out instanceControl))
                {
                    // We failed to de-queue, so we must create a new control...
                    instanceControl = new InstanceControl(instance)
                    {
                        Dock = DockStyle.Top,
                    };

                    instanceControl.OnInstanceDelete += OnInstanceDelete;
                    instanceControl.OnInstancePackages += OnInstancePackages;
                    instanceControl.OnInstanceOpen += OnInstanceOpen;
                }
                else
                    instanceControl.LoadInstance(instance);

                pcInstanceList.Controls.Add(instanceControl);
            }
        }

        /// <summary>
        /// Cache all current instance controls for reuse in enumuration
        /// </summary>
        void CacheInstanceControls()
        {
            // Add each instance control back into the cache
            foreach (InstanceControl instanceControl in pcInstanceList.Controls)
            {
                // Clear it...
                instanceControl.Reset();


                // Queue it...
                instanceControlCache.Enqueue(instanceControl);
            }

            // Clear the project controls list...
            pcInstanceList.Controls.Clear();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            // Check for updates automatically if the user wants to...
            if (Program.Preferences.AutomaticallyCheckUpdates)
                CheckForUpdates();
        }

        /// <summary>
        /// Called when we change the tab view tab...
        /// </summary>
        void OnTabChange(object sender, EventArgs e)
        {
            // Which tab did we select?
            if (tcMain.SelectedTab == tpInstances)
                EnumurateInstances();
            else
            if (tcMain.SelectedTab == tpProjects)
                EnumurateProjects();
        }
    }
}