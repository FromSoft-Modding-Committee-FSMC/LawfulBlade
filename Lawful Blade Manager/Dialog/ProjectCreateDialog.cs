using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Dialog
{
    public partial class ProjectCreateDialog : Form
    {
        /// <summary>
        /// Arguments for the create project call.
        /// </summary>
        public ProjectCreateArgs? CreationArguments { get; private set; }

        /// <summary>
        /// Default Constructor<br/>
        /// </summary>
        public ProjectCreateDialog()
        {
            // Winforms Garbage...
            InitializeComponent();

            // Default dialog result to cancel...
            DialogResult = DialogResult.Cancel;

            // We must enumurate any avaliable instances...
            cbTargetInstance.DataSource = Program.InstanceManager.Instances.Select(i => new KeyValuePair<string, string>(i.Name, i.UUID)).ToList();
            cbTargetInstance.DisplayMember = "Key";
            cbTargetInstance.ValueMember = "Value";
            cbTargetInstance.SelectedIndex = 0;
        }

        /// <summary>
        /// Callback event for when the user clicks the "Create" button
        /// </summary>
        void OnClickCreate(object sender, EventArgs e)
        {
            try
            {
                // Simple Validation
                if (tbProjectName.Text == string.Empty)
                    throw new Exception("You must enter a project name!");

                if (tbTargetPath.Text == string.Empty)
                    throw new Exception("You must enter a target path!");

                if (cbTargetInstance.SelectedValue == null)
                    throw new Exception("You must select an instance!");

                if (Program.Preferences.EnableSandboxing == true && !tbTargetPath.Text.Contains(Program.Preferences.SandboxedPath))
                    throw new Exception("You can only create a project in the sandboxed path!\nIf you dislike this behaviour, you can disable it within the preferences.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Construct creation arguments...
            CreationArguments = new ProjectCreateArgs
            {
                Name = tbProjectName.Text,
                Description = tbDescription.Text,
                Destination = tbTargetPath.Text,
                CreateEmpty = xbCreateEmpty.CheckState == CheckState.Checked,
                InstanceUUID = (string)cbTargetInstance.SelectedValue
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Callback event for when the user clicks the "Cancel" button
        /// </summary>
        void OnClickCancel(object sender, EventArgs e) =>
            Close();

        /// <summary>
        /// Callback event for when the user clicks the "Select" button
        /// </summary>
        void OnClickSelect(object sender, EventArgs e)
        {
            // Create and show the dialog...
            using FolderBrowserDialog fbd = new();

            // When sandboxing is enabled, set the initial path...
            if (Program.Preferences.EnableSandboxing == true)
                fbd.SelectedPath = Program.Preferences.SandboxedPath;

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            tbTargetPath.Text = fbd.SelectedPath;
        }
    }
}
