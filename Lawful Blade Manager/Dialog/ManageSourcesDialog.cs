using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Dialog
{
    public partial class ManageSourcesDialog : Form
    {
        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        public ManageSourcesDialog() => InitializeComponent();

        /// <summary>
        /// Enumerate all sources in the package manager, and display them to the user
        /// </summary>
        void EnumerateSources()
        {
            // First, clear the list view
            lvSourceList.Items.Clear();

            // Add each source as an item...
            foreach(PackageSource source in Program.PackageManager.PackagesData.PackageSources)
                lvSourceList.Items.Add(new ListViewItem(new[] { source.URI, $"{source.CreationDate}", $"{source.Packages.Length}" }));
        }

        /// <summary>
        /// Callback event for when the dialog loads
        /// </summary>
        void OnDialogLoad(object sender, EventArgs e) =>
            EnumerateSources();

        /// <summary>
        /// Callback event for when user presses the 'add' button
        /// </summary>
        void OnClickSourceAdd(object sender, EventArgs e)
        {
            // Make sure the text box is valid...
            if (tbSourceField.Text == string.Empty)
                return;

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
                MessageBox.Show($"Invalid package source URI: '{tbSourceField.Text}'", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }         

            // Make sure the source doesn't exist in our list already
            if (Program.PackageManager.ContainsPackageSource(tbSourceField.Text))
            {
                MessageBox.Show($"Package source already exists: '{tbSourceField.Text}'", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // We are now sure it is a valid URL. Add it.
            Program.PackageManager.AddPackageSource(sourceUri);

            // Re-Enumurate with new sources added...
            EnumerateSources();
        }
    }
}
