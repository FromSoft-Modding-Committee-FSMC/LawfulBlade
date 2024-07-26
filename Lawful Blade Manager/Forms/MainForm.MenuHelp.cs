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
        void OnHelpMenuCheckForUpdates(object sender, EventArgs e) =>
            CheckForUpdates();

        
        void CheckForUpdates()
        {
            if (updatesURL == string.Empty)
            {
                MessageBox.Show("'websitesURL' field was not set in 'MainForm.MenuHelp.cs'!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Start a custom busy dialog
            BusyDialog.Instance.ShowBusy("Checking for Updates...", "So long, fare thee well. Pip pip cheerio - \r\nWe'll be back soon!");

            // Create a temporary directory to perform the update from...
            string tempDir = Path.Combine(Path.GetTempPath(), "LawfulUpdate");

            // Does the temporary directroy exist? if so, delete it...
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);

            // Download the info file and move it to the LawfulUpdate path...
            string tempFile = Program.DownloadManager.DownloadFileSync(new Uri(updatesURL));
            File.Move(tempFile, Path.Combine(tempDir, "version"));

            // Now, check if we actually need any updates.
            string[] versionLines = File.ReadAllLines(Path.Combine(tempDir, "version"));

            if (versionLines[0] == ProgramContext.Version)  // TO-DO: This needs some > logic.
            {
                BusyDialog.Instance.HideBusy();
                MessageBox.Show("Up to date!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Does the user want to update right now?
            if (MessageBox.Show($"A new version of Lawful Blade is avaliable!\nYours: {ProgramContext.Version}, Avaliable: {versionLines[0]}\nDo you want to update?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                BusyDialog.Instance.HideBusy();
                return;
            }

            // The user wants to update. Download the update zip and move it to the update directory...
            string updateZip = Program.DownloadManager.DownloadFileSync(new Uri(versionLines[1]));
            File.Move(updateZip, Path.Combine(tempDir, Path.GetFileName(versionLines[1])), true);

            // Copy the updater to the update directory...
            File.Copy(Path.Combine(ProgramContext.ProgramPath, "updaterer.exe"), updateZip, true);

            // Hide Busy...
            BusyDialog.Instance.Hide();

            // Start to perform the update...
            Process.Start(new ProcessStartInfo
            {
                FileName  = Path.Combine(ProgramContext.ProgramPath, "updaterer.exe"),
                Arguments = $"{Process.GetCurrentProcess().Id} {Process.GetCurrentProcess().ProcessName} \"{ProgramContext.ProgramPath}\""
            });

            // Shutdown Lawful Blade
            Program.Shutdown();
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

        /// <summary>
        /// Called when a user clicks on 'about'
        /// </summary>
        void OnHelpMenuAbout(object sender, EventArgs e)
        {
            using AboutDialog aboutDialog = new();
            aboutDialog.ShowDialog();
        }        
    }
}
