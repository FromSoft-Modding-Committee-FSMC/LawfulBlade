using LawfulBladeManager.Dialog;

namespace LawfulBladeManager.Forms
{

    // We use these (slightly awkward with Winforms) partial classes to define callbacks for the Menu Bar of MainForm
    public partial class MainForm
    {
        /// <summary>
        /// Called when a user clicks the 'preferences' option
        /// </summary>
        void OnFileMenuPreferences(object sender, EventArgs e)
        {
            // Open the preferences dialog
            using PreferencesDialog preferencesDialog = new();

            // We copy the current user preferences...
            preferencesDialog.CopyPreferences(Program.Preferences);

            // Show the dialog. When it returns, if the result is OK (confirm button), we store the new preferences
            if (preferencesDialog.ShowDialog() == DialogResult.OK)
                preferencesDialog.StorePreferences(Program.Preferences);
        }

        /// <summary>
        /// Called when a user clicks the 'exit' option
        /// </summary>
        void OnFileMenuExit(object sender, EventArgs e) =>
            Application.Exit();
    }
}
