using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;

namespace LawfulBladeManager.Dialog
{
    public partial class RepositoryCreateDialog : Form
    {
        public PackageRepositoryCreateArgs? CreationArguments { get; private set; }

        /// <summary>
        /// Default Constructor.<br/>
        /// Can take an optional repository as a parameter, 
        /// </summary>
        /// <param name="repository">Optional. Loads information from the repository as the defaults.</param>
        public RepositoryCreateDialog(PackageRepository? repository = null)
        {
            InitializeComponent();

            // Set the default dialog result to Cancel
            DialogResult = DialogResult.Cancel;

            // If repository is null, we are creating one from scratch.
            if (repository == null)
                return;

            // Copy simple data from old repository...
            tbName.Text = repository.Info.Name;
            tbDescription.Text = repository.Info.Description;
        }

        /// <summary>
        /// Callback event for when the user clicks the "Create" button
        /// </summary>
        void OnClickCreate(object sender, EventArgs e)
        {
            try
            {
                // Basic Validation
                if (tbName.Text == string.Empty)
                    throw new Exception("'Name' field cannot be empty!");
                if (tbName.Text.Length > 32)
                    throw new Exception("'Name' field value is too long! 'Name' can be a max of 32 characters.");
                if (!tbName.Text.IsAlphanumeric())
                    throw new Exception("'Name' field must be alphanumeric (only use A-Z, a-z or 0-9!)");

                if (tbDescription.Text == string.Empty)
                    throw new Exception("'Description' field cannot be empty!");
                if (tbDescription.Text.Length > 128)
                    throw new Exception("Value in 'Description' field is too long! 'Description' can be a max of 128 characters.");

                // Make sure each package still exists
                foreach (ListViewItem lvItem in lvBundles.Items)
                {
                    if (!File.Exists(lvItem.Text))
                        throw new Exception($"Bundle file does not exist: '{lvItem.Text}'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Find the folder we want to store the repository in...
            using FolderBrowserDialog fbd = new()
            {
                ShowNewFolderButton = true
            };

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            // Construct Creation Arguments...
            CreationArguments = new PackageRepositoryCreateArgs
            {
                OutputDirectory = fbd.SelectedPath,
                BundleSources = lvBundles.Items.Cast<ListViewItem>().Select(lvItem => lvItem.Text).ToArray(), // Lamo, say what microsoft? twats
                Name = tbName.Text,
                Description = tbDescription.Text
            };


            // Finish with the dialog by setting the result to OK and closing it.
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Callback event for when the user clicks the "Cancel" button
        /// </summary>
        void OnClickCancel(object sender, EventArgs e) =>
            Close();

        /// <summary>
        /// Callback event for when a user clicks either the "Add..." or "Remove..." buttons...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddRemoveBundle(object sender, EventArgs e)
        {
            // Are we adding or removing a bundle?
            if (sender == btBundleAdd)
                OnBundleAdd();
            else
            if (sender == btBundleRemove)
                OnBundleRemove();
        }

        /// <summary>
        /// Callback to add a bundle to the bundle list
        /// </summary>
        void OnBundleAdd()
        {
            // Create OpenFileDialog
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Possibly A Zip (*.paz)|*.paz",
                DefaultExt = "paz",
                CheckPathExists = true,
            };

            // Open the OpenFileDialog, and check if we selected a file
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            //
            // TO-DO:
            //  Format validation...
            //

            // Does this file already exist in the bundles list?
            foreach (ListViewItem lvItem in lvBundles.Items)
            {
                if (ofd.FileName != lvItem.Text)
                    continue;

                MessageBox.Show("Cannot add duplicate bundle!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add the bundle file to the list...
            lvBundles.Items.Add(new ListViewItem(new[] { ofd.FileName, $"{new FileInfo(ofd.FileName).Length / 1024 / 1024}" }));
        }

        /// <summary>
        /// Callback to remove a bundle from the bundle list
        /// </summary>
        void OnBundleRemove()
        {
            // Remove each selected bundle (backwards)
            for (int i = lvBundles.Items.Count - 1; i >= 0; i--)
            {
                if (!lvBundles.Items[i].Selected)
                    continue;

                // Remove the item because it is selected
                lvBundles.Items.RemoveAt(i);
            }
        }
    }
}
