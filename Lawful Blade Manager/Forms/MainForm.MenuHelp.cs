using LawfulBladeManager.Dialog;
using LawfulBladeManager.Type;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace LawfulBladeManager.Forms
{

    // We use these (slightly awkward with Winforms) partial classes to define callbacks for the Menu Bar of MainForm
    public partial class MainForm
    {
        // Release Notes File
        readonly string releaseNotesFile = Path.Combine(ProgramContext.ProgramPath, $"release-notes-v{ProgramContext.Version.Remove(c:'.')}.txt");

        // Bug Report and Update Polling URLs
        const string reportAProblemURL = @"https://github.com/FromSoft-Modding-Committee-FSMC/Lawful-Blade/issues";
        const string updatesURL        = @"https://raw.githubusercontent.com/FromSoft-Modding-Committee-FSMC/Lawful-Blade/main/version";

        /// <summary>
        /// Called when a user clicks the 'Check For Updates' option
        /// </summary>
        void OnHelpMenuCheckForUpdates(object sender, EventArgs e)
        {
            if(updatesURL == string.Empty)
            {
                MessageBox.Show("'websitesURL' field was not set in 'MainForm.MenuHelp.cs'!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Download the version file...
            string tempFile = Program.DownloadManager.DownloadFileSync(new Uri(updatesURL));

            // Inspect the version - are we up to date?
            string netVLine = File.ReadAllText(tempFile);

            if (netVLine == ProgramContext.Version)
            {
                MessageBox.Show("Up to date!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Not up to date...
            if (MessageBox.Show($"A new version of Lawful Blade is avaliable!\nYours: {ProgramContext.Version}, Avaliable: {netVLine}\nDo you want to update?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            // TO-DO:
            //  Update logic and updater tool...
        }

        /// <summary>
        /// Called when a user clicks the 'Report a Problem' option
        /// </summary>
        void OnHelpMenuReportAProblem(object sender, EventArgs e)
        {
            if (reportAProblemURL == string.Empty)
            {
                MessageBox.Show("'reportAProblemURL' field was not set in 'MainForm.MenuHelp.cs'!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception("No network connection!");

                if (Process.Start(new ProcessStartInfo() { FileName = reportAProblemURL, UseShellExecute = true }) == null)
                    throw new Exception($"Couldn't start browser process with URL:\n'{reportAProblemURL}'");
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Called when a user clicks on 'Release Notes'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnHelpMenuReleaseNotes(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(releaseNotesFile))
                    throw new Exception($"Couldn't find release notes:\n'{releaseNotesFile}'");

                if (Process.Start(new ProcessStartInfo() { FileName = releaseNotesFile, UseShellExecute = true }) == null)
                    throw new Exception($"Couldn't start text editor with file:\n'{releaseNotesFile}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void OnHelpMenuAbout(object sender, EventArgs e)
        {
            using AboutDialog aboutDialog = new();
            aboutDialog.ShowDialog();
        }
            
    }
}
