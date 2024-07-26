using LawfulBladeManager.Core;

namespace LawfulBladeManager.Dialog
{
    public partial class PreferencesDialog : Form
    {
        /// <summary>
        /// Default Constructor.<br/> WinForms fuckery.
        /// </summary>
        public PreferencesDialog() => InitializeComponent();

        /// <summary>
        /// Stores new preferences into the Preferences class.
        /// </summary>
        /// <param name="preferences">The current preferences</param>
        public void StorePreferences(Preferences preferences)
        {
            // Packages...
            preferences.PackageCacheDirectory = tbPackageCacheLocation.Text;

            // Sandboxing...
            preferences.EnableSandboxing = xbEnableSandboxing.Checked;
            preferences.SandboxedPath = tbSandboxLocation.Text;

            // Program...
            preferences.ShowConsoleOnStartup = xbShowConsole.Checked;
            preferences.AutomaticallyCheckUpdates = xbCheckUpdates.Checked;
        }

        /// <summary>
        /// Copies current preferences from the Preferences class
        /// </summary>
        /// <param name="preferences">The current preferences</param>
        public void CopyPreferences(Preferences preferences)
        {
            // Packages...
            tbPackageCacheLocation.Text = preferences.PackageCacheDirectory;

            // Sandboxing...
            tbSandboxLocation.Text = preferences.SandboxedPath;
            xbEnableSandboxing.Checked = preferences.EnableSandboxing;

            // Program...
            xbShowConsole.Checked = preferences.ShowConsoleOnStartup;
            xbCheckUpdates.Checked = preferences.AutomaticallyCheckUpdates;
        }

        /// <summary>
        /// Called when the confirm button is pressed in the dialog
        /// </summary>
        void OnDialogConfirm(object sender, EventArgs e)
        {
            // We use try/catch to validate our data cleanly
            try
            {
                if (tbPackageCacheLocation.Text == string.Empty || !Directory.Exists(tbPackageCacheLocation.Text))
                    throw new Exception("Please select a valid location for the Package Cache!\nThe selected location is either empty or doesn't exist!");

                if (tbSandboxLocation.Text == string.Empty || !Directory.Exists(tbSandboxLocation.Text))
                    throw new Exception("Please select a valid location for the Sandbox!\nThe selected location is either empty or doesn't exist!");
            }
            catch (Exception ex)
            {
                // Show a message box about the bad data...
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Exit early on bad data...
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Called when the cancel button is pressed in the dialog
        /// </summary>
        void OnDialogCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Called when any path select button is pressed.
        /// </summary>
        void OnPathSelect(object sender, EventArgs e)
        {
            // Select a directory using the folder browser dialog
            using FolderBrowserDialog fbd = new();

            // Depending on the sender we want to do different actions
            if (sender == btPackageCacheSelect)
                OnSelectPackageCache(fbd);
            else
            if (sender == btSandboxSelect)
                OnSelectSandbox(fbd);
        }

        void OnSelectSandbox(FolderBrowserDialog fbd)
        {
            // Set the current path
            fbd.SelectedPath = tbSandboxLocation.Text;

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            if (Directory.GetFileSystemEntries(fbd.SelectedPath).Length > 0)
                throw new Exception("In order to prevent accidental deletions, the Sandbox location must be empty! Please select an empty path!");

            // Warn the user about side effects, make sure they're okay with them
            if (MessageBox.Show("Are you sure you want to change the sandbox location?\nYou will not be able to modify any existing projects or instances!", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Set the package cache location option
                tbSandboxLocation.Text = fbd.SelectedPath;
            }
        }

        void OnSelectPackageCache(FolderBrowserDialog fbd)
        {
            // Set the current path
            fbd.SelectedPath = tbPackageCacheLocation.Text;

            // Show the dialog, check the result
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            // Warn the user about side effects, make sure they're okay with them
            if (MessageBox.Show("Are you sure you want to change the package cache location?\nYou will need to re download any cached packages!", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Set the package cache location option
                tbPackageCacheLocation.Text = fbd.SelectedPath;
            }
        }
    }
}
