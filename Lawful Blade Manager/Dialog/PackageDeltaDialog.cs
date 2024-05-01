using LawfulBladeManager.Dialog;
using System.Security.Cryptography;

namespace LawfulBladeManager.Forms
{
    public partial class PackageDeltaDialog : Form
    {
        public PackageDeltaDialog()
        {
            InitializeComponent();
        }

        private void btSelectSourceTarget_Click(object sender, EventArgs e)
        {
            // I don't want 50 thousand events for the same code, so this
            // is going to be very naughty as a result.
            using (FolderBrowserDialog fbd = new())
            {
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                if (sender.Equals(btSelectSrcA))    // Selecting Source A
                {
                    tbSourceA.Text = fbd.SelectedPath;
                }
                else
                if (sender.Equals(btSelectSrcB))    // Selecting Source B
                {
                    tbSourceB.Text = fbd.SelectedPath;
                }
                else
                if (sender.Equals(btSelectTarget))   // Selecting Target
                {
                    tbTarget.Text = fbd.SelectedPath;
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            // Close the dialog and return that it was cancelled.
            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }

        private void btGenerateDelta_Click(object sender, EventArgs e)
        {
            // We need to validate, and be extra error safe here.
            try
            {
                // Validate User Input
                if (tbSourceA.Text == string.Empty || !Path.Exists(tbSourceA.Text))
                    throw new Exception("Source A must be a valid path!");
                if (tbSourceB.Text == string.Empty || !Path.Exists(tbSourceB.Text))
                    throw new Exception("Source B must be a valid path!");
                if (tbTarget.Text == string.Empty || Directory.GetFileSystemEntries(tbTarget.Text).Length > 0)
                    throw new Exception("Target directory is either invalid, or already has files!");

                BusyDialog.Instance.ShowBusy();

                // Does the user want to do logging?
                StreamWriter? logWriter = null;
                if (xbGenerateLog.Checked)
                {
                    logWriter = new StreamWriter(File.Open($"{tbTarget.Text}.log", FileMode.CreateNew));
                    logWriter.WriteLine("Key: ");
                    logWriter.WriteLine("    '....' = N/A, 'NEWB' = New Source B File, 'B=RM' = Removed from B, exists in A, 'B!=A' = B Difference");
                    logWriter.WriteLine();
                }

                // Collect information about Source A
                string[] filesSourceA = Directory.GetFileSystemEntries(tbSourceA.Text, "*", SearchOption.AllDirectories);
                string[] filesSourceB = Directory.GetFileSystemEntries(tbSourceB.Text, "*", SearchOption.AllDirectories);

                // Start comparing the directories...
                foreach (string entity in filesSourceB)
                {
                    string entityName = entity.Replace(tbSourceB.Text, "").Trim(Path.DirectorySeparatorChar);
                    string entityDest = Path.Combine(tbTarget.Text, entityName);

                    // Is this a directory ?
                    if ((File.GetAttributes(entity) & FileAttributes.Directory) != 0)
                    {
                        Directory.CreateDirectory(entityDest);
                        logWriter?.WriteLine($"[DIR , ...., B->T]\t{entityDest}");
                    }
                    else
                    {
                        // Does the file exist in sourceA ?
                        string entityPrev = Path.Combine(tbSourceA.Text, entityName);

                        if (File.Exists(entityPrev))
                        {
                            // The file exists in source A and source B
                            Guid saHash = new(MD5.Create().ComputeHash(File.OpenRead(entityPrev)));
                            Guid sbHash = new(MD5.Create().ComputeHash(File.OpenRead(entity)));

                            // If the hashes are different, we copy the file.
                            if (saHash != sbHash)
                            {
                                File.Copy(entity, entityDest, true);
                                logWriter?.WriteLine($"[FILE, B!=A, B->T]\t{entityDest}");
                            }
                        }
                        else
                        {
                            // The file doesn't exist in source A but does in source B... Copy it.
                            File.Copy(entity, entityDest, true);
                            logWriter?.WriteLine($"[FILE, NEWB, B->T]\t{entityDest}");
                        }
                    }
                }

                // Pass 2: Only when we want to copy files removed from Source B, but present in Source A
                if (xbIncludeRemovedFiles.Checked)
                {
                    foreach (string entity in filesSourceA)
                    {
                        string entityName = entity.Replace(tbSourceA.Text, "").Trim(Path.DirectorySeparatorChar);
                        string entityDest = Path.Combine(tbTarget.Text, entityName);

                        if (File.Exists(entityDest))
                            continue;

                        // Is this a directory ?
                        if ((File.GetAttributes(entity) & FileAttributes.Directory) != 0)
                        {
                            // Create the directory.
                            Directory.CreateDirectory(entityDest);
                            logWriter?.WriteLine($"[DIR , B=RM, A->T]\t{entityDest}");
                        }
                        else
                        {
                            // It must be a file?..
                            File.Copy(entity, entityDest, true);
                            logWriter?.WriteLine($"[FILE, B=RM, A->T]\t{entityDest}");
                        }
                    }
                }

                // We're not using "using" statements, so we need to handle the filehandle manually.
                logWriter?.Flush();
                logWriter?.Close();
            }
            catch (Exception ex)
            {
                BusyDialog.Instance.HideBusy();
                MessageBox.Show(ex.Message, "Lawful Blade - Delta Generator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BusyDialog.Instance.HideBusy();

            // Close Dialog with valid result
            MessageBox.Show("Delta Generated Successfully!", "Lawful Blade - Delta Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
            return;
        }
    }
}
