using LawfulBladeManager.Packages;
using System.Numerics;
using System.Security.Cryptography;

namespace LawfulBladeManager.Dialog
{
    public partial class RepositoryManagerDialog : Form
    {
        string lastRepositoryCheckSum = "ABCD";
        string currRepositoryCheckSum = "ABCD";

        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        public RepositoryManagerDialog()
        {
            // Winforms garbage
            InitializeComponent();

            // Get the current checksum
            if (File.Exists(PackageManager.RepositoriesFile))
            {
                lastRepositoryCheckSum = new Guid(MD5.HashData(File.ReadAllBytes(PackageManager.RepositoriesFile))).ToString();
                currRepositoryCheckSum = lastRepositoryCheckSum;
            }
        }

        /// <summary>
        /// Enumerate all sources in the package manager, and display them to the user
        /// </summary>
        void EnumerateRepositories()
        {
            // First, clear the list view
            lvSourceList.Items.Clear();

            // Add each repository as an item...
            foreach (PackageRepository repo in Program.PackageManager.Repositories)
                lvSourceList.Items.Add(new ListViewItem(new[] { repo.Info.Name, repo.Info.Description, $"{repo.Info.CreationDate}", repo.Info.URI, $"{repo.Packages.Length}" }));
        }

        /// <summary>
        /// Callback event for when the dialog loads
        /// </summary>
        void OnDialogLoad(object sender, EventArgs e) =>
            EnumerateRepositories();

        /// <summary>
        /// Callback event for when user presses the 'add' button
        /// </summary>
        void OnClickSourceAdd(object sender, EventArgs e)
        {
            // Make sure the text box is valid...
            if (tbSourceField.Text == string.Empty)
                return;

            Logger.ShortInfo(tbSourceField.Text);

            // Try to make a URI from the text box
            if (!Uri.TryCreate(tbSourceField.Text, UriKind.RelativeOrAbsolute, out Uri? sourceUri))
            {
                MessageBox.Show($"Bad URI: '{tbSourceField.Text}'", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sourceUri == null)
                return;

            // Is the URI either a valid web address or valid file?
            if (!sourceUri.IsFile && !Program.DownloadManager.DownloadFileExists(sourceUri))
            {
                MessageBox.Show($"Invalid repository URI: '{tbSourceField.Text}'", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Make sure the source doesn't exist in our list already
            if (Program.PackageManager.FindRepositoryByURI(tbSourceField.Text, out _))
            {
                MessageBox.Show($"Duplicate repository: '{tbSourceField.Text}'", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // We are now sure it is a valid URL. Add it.
            Program.PackageManager.AddRepository(sourceUri);

            // Calculate a new checksum of the repository file
            currRepositoryCheckSum = new Guid(MD5.HashData(File.ReadAllBytes(PackageManager.RepositoriesFile))).ToString();

            // Re-Enumurate with new sources added...
            EnumerateRepositories();
        }

        /// <summary>
        /// Callback event for when the user presses the 'done' button
        /// </summary>
        void OnClickDone(object sender, EventArgs e)
        {
            // Check for invalidated repositories file...
            if (lastRepositoryCheckSum != currRepositoryCheckSum)
            {
                if (MessageBox.Show("You must restart Lawful Blade to apply changes!\n\nDo you want to restart now?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    Program.Shutdown();
            }

            // Set result and close the dialog
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
