using LawfulBladeManager.Forms;
using LawfulBladeManager.Projects;
using LawfulBladeManager.Tagging;

namespace LawfulBladeManager.Control
{
    public partial class ProjectControl : UserControl
    {
        // Delegate for events
        public delegate void OnProjectEvent(Project project);

        // Event Declaration
        public event OnProjectEvent? OnProjectDelete;
        public event OnProjectEvent? OnProjectSettings;
        public event OnProjectEvent? OnProjectManagePackages;
        public event OnProjectEvent? OnProjectGenerateRuntime;
        public event OnProjectEvent? OnProjectOpen;

        public Project Project { get; private set; }

        /// <summary>
        /// Default Constructor.<br/>
        /// Wrap winforms callbacks with our own events
        /// </summary>
        public ProjectControl(Project project)
        {
            // Winforms crap
            InitializeComponent();

            // Load the project
            LoadProject(Project = project);

            // Winforms redirection
            tsFuncDelete.Click += (_, _) => OnProjectDelete?.Invoke(project);
            tsFuncSettings.Click += (_, _) => OnProjectSettings?.Invoke(project);
            tsFuncPackages.Click += (_, _) => OnProjectManagePackages?.Invoke(project);
            tsFuncExport.Click += (_, _) => OnProjectGenerateRuntime?.Invoke(project);
            tsFuncOpen.Click += (_, _) => OnProjectOpen?.Invoke(project);
        }

        /// <summary>
        /// Loads data from a project into the project control
        /// </summary>
        /// <param name="project">the project to load from</param>
        public void LoadProject(Project project)
        {
            // Copy basic details
            lbTitle.Text = project.Name;
            lbDescription.Text = project.Description;

            // Generate tags
            Graphics graph = Graphics.FromHwnd(Handle);

            foreach (string tag in project.Tags)
            {
                flTagList.Controls.Add(new Label
                {
                    Text = tag,
                    BackColor = Tags.MakeBackgroundColour(tag),
                    ForeColor = Tags.MakeForegroundColour(tag),
                    Size = new Size((int)graph.MeasureString(tag, lbDescription.Font).Width, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            // Load icon file (or the default if non exists...)
            string iconFile = Path.Combine(project.StoragePath, "icon.png");

            if (File.Exists(iconFile))
                pbIcon.Image = Image.FromFile(iconFile);
            else
                pbIcon.Image = Properties.Resources.defaultProjectIcon;
        }

        /// <summary>
        /// Removes any loaded data, so the project control returns to a new-like state.
        /// </summary>
        public void Reset()
        {
            lbTitle.Text = string.Empty;
            lbDescription.Text = string.Empty;
            flTagList.Controls.Clear();

            // Only clear the icon when it's not null, and not the default
            if (pbIcon.Image != null && pbIcon.Image != Properties.Resources.defaultProjectIcon)
            {
                pbIcon.Image.Dispose();
                pbIcon.Image = null;
            }
        }
    }
}
