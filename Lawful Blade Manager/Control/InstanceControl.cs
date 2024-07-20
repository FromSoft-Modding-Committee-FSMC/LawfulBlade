using LawfulBladeManager.Forms;
using LawfulBladeManager.Instances;
using LawfulBladeManager.Projects;
using LawfulBladeManager.Tagging;
using System.Diagnostics;

namespace LawfulBladeManager.Control
{
    public partial class InstanceControl : UserControl
    {
        // Delegate for events
        public delegate void OnInstanceEvent(Instance project);

        // Event Declaration
        public event OnInstanceEvent? OnInstanceDelete;
        public event OnInstanceEvent? OnInstancePackages;
        public event OnInstanceEvent? OnInstanceOpen;

        public Instance Instance { get; private set; }

        /// <summary>
        /// Default Constructor.<br/>
        /// Wrap winforms callbacks with our own events
        /// </summary>
        public InstanceControl(Instance instance)
        {
            // Winforms crap
            InitializeComponent();

            // Load the instance
            LoadInstance(Instance = instance);

            // Winforms redirection
            tsFuncDelete.Click += (_, _) => OnInstanceDelete?.Invoke(Instance);
            tsFuncPackages.Click += (_, _) => OnInstancePackages?.Invoke(Instance);
            tsFuncOpen.Click += (_, _) => OnInstanceOpen?.Invoke(Instance);
        }

        /// <summary>
        /// Loads data from a instance into the instance control
        /// </summary>
        /// <param name="instance">the instance to load from</param>
        public void LoadInstance(Instance instance)
        {
            // Copy basic details
            lbTitle.Text = instance.Name;
            lbDescription.Text = instance.Description;
            pbIcon.Image = Program.InstanceManager.InstanceIcons[instance.IconID];

            // Generate tags
            Graphics graph = Graphics.FromHwnd(Handle);

            foreach (string tag in instance.Tags)
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
        }

        /// <summary>
        /// Removes any loaded data, so the project control returns to a new-like state.
        /// </summary>
        public void Reset()
        {
            lbTitle.Text = string.Empty;
            lbDescription.Text = string.Empty;
            flTagList.Controls.Clear();
        }

        /// <summary>
        /// Called from the context menu when the user wants to view the instance in the file explorer...
        /// </summary>
        void OnMenuShowInExplorer(object sender, EventArgs e) =>
            Process.Start("explorer.exe", Instance.StoragePath);
    }
}
