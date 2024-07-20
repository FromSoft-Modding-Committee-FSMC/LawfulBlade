using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Forms
{

    // We use these (slightly awkward with Winforms) partial classes to define callbacks for the Menu Bar of MainForm
    public partial class MainForm
    {
        /// <summary>
        /// Called when a user clicks the 'Manage Sources' option
        /// </summary>
        void OnPackagesMenuManageRepositories(object sender, EventArgs e)
        {
            using RepositoryManagerDialog msd = new();
            msd.ShowDialog();
        }

        /// <summary>
        /// Called when a user clicks the 'Create Package.../New' or 'Create Package.../From Existing' option
        /// </summary>
        void OnPackagesMenuCreatePackage(object sender, EventArgs e)
        {
            // Which one did they press?
            Package? package = null;

            // Existing package... The other caller is 'msPackagesCreateNew'
            if (sender == msPackagesCreatePackageFromExisting)
            {
                // We must load the package...
                using OpenFileDialog ofd = new ()
                {
                    Filter = "Possibly A Zip (*.paz)|*.paz",
                    DefaultExt = "paz",
                    FileName = "package.paz",
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                // Try to load the package bundle...
                try
                {
                    package = Package.Load(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }   
            }

            // Spawn the package creator dialog
            using PackageCreateDialog pcd = new(package);

            // Don't continue if creation arguments are null or/and we cancelled
            if (pcd.ShowDialog() != DialogResult.OK || pcd.CreationArguments == null)
                return;

            // About to be busy...
            BusyDialog.Instance.ShowBusy();

            // Create Package
            PackageManager.CreatePackage(pcd.CreationArguments);

            // No longer busy...
            BusyDialog.Instance.HideBusy();
        }

        /// <summary>
        /// Called when a user clicks the 'Create Repository.../New' or 'Create Repository.../From Existing' option
        /// </summary>
        void OnPackagesMenuCreateRepository(object sender, EventArgs e)
        {
            // Which one did they press?
            PackageRepository? repository = null;

            // Existing repository...
            if (sender == msPackagesCreateRepositoryFromExisting)
            {
                using OpenFileDialog ofd = new()
                {
                    Filter = "Package Repository Info (*.inf)|*.inf",
                    DefaultExt = "inf",
                    FileName = "repository.inf",
                };
            }

            using RepositoryCreateDialog rcd = new(repository);

            if (rcd.ShowDialog() != DialogResult.OK || rcd.CreationArguments == null)
                return;

            // About to be -VERY- busy...
            BusyDialog.Instance.ShowBusy();

            PackageManager.CreateRepository(rcd.CreationArguments);

            // No longer busy...
            BusyDialog.Instance.HideBusy();
        }

        /// <summary>
        /// Called when a user clicks the 'Create Package.../Tools.../Delta Directory' option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPackagesMenuDeltaDirectoryTool(object sender, EventArgs e)
        {

        }
    }
}
