using LawfulBladeManager.Instances;

namespace LawfulBladeManager.Dialog
{
    public partial class InstanceCreateDialog : Form
    {
        /// <summary>
        /// Arguments for the create instance call.
        /// </summary>
        public InstanceCreateArgs? CreationArguments { get; private set; }

        // We store the current Icon ID in here...
        int IconID = 0;

        /// <summary>
        /// Default Constructor<br/>
        /// </summary>
        public InstanceCreateDialog()
        {
            // Winforms Garbage...
            InitializeComponent();

            // Default dialog result to cancel...
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Callback event for when the user clicks the "Create" button
        /// </summary>
        void OnClickCreate(object sender, EventArgs e)
        {
            try
            {
                // Simple Validation
                if (tbName.Text == string.Empty)
                    throw new Exception("You must enter an instance name!");

                if (tbTargetPath.Text == string.Empty)
                    throw new Exception("You must enter a target path!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Construct creation arguments...
            CreationArguments = new InstanceCreateArgs
            {
                Name        = tbName.Text,
                Description = tbDescription.Text,
                Destination = tbTargetPath.Text,
                IconID      = IconID
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

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            tbTargetPath.Text = fbd.SelectedPath;
        }

        /// <summary>
        /// Callback event for when the user clicks either the "<" or ">" icon buttons...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChangeIcon(object sender, EventArgs e)
        {
            if (sender == btIconLeft)
                IconID -= 1;
            else
            if (sender == btIconRight)
                IconID += 1;

            // Wrap the ID if it's less than 0
            if (IconID < 0)
                IconID = (Program.InstanceManager.InstanceIcons.Length - 1);

            // Wrap the ID if it's above the max icon count
            IconID %= Program.InstanceManager.InstanceIcons.Length;

            // Set the image in the picture box..
            pbIcon.Image = Program.InstanceManager.InstanceIcons[IconID];
        }
    }
}
