using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerDialog : Form
    {
        // Private Fields
        readonly Queue<PackageControl> packageControlCache = new Queue<PackageControl>();
        PackageControl? currentPackage;

        /// <summary>
        /// Target for installation (Project, Instance etc)
        /// </summary>
        public IPackageTarget InstallationTarget { get; private set; }

        /// <summary>
        /// The super filter is a list of tags, which controls which packages can be managed for the target.
        /// </summary>
        public string[] SuperFilter { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PackageManagerDialog(IPackageTarget target)
        {
            // Winforms Initialization
            InitializeComponent();

            // Set the installation target
            InstallationTarget = target;
            SuperFilter = target.CompatiblePackages;
        }

        /// <summary>
        /// When the form first loads, we need to create controls for all the possible packages...
        /// </summary>
        void OnLoadDialog(object sender, EventArgs e)
        {
            // Show Busy
            BusyDialog.Instance.ShowBusy();

            // Loads all avaliable tags...
            EnumuratePackageTags();

            // Loads all avaliable packages...
            EnumuratePackageRepositories();

            // Bind the action button of exinfo
            exInfo.OnAction += OnPackageAction;

            // Hide Busy
            BusyDialog.Instance.HideBusy();
        }

        /// <summary>
        /// Scans all repositories and their contained packages for tags...
        /// </summary>
        void EnumuratePackageTags()
        {
            // Clear the package filter of all items.
            lvPackageFilter.Items.Clear();

            // Each repo...
            foreach (PackageRepository repo in Program.PackageManager.Repositories)
            {
                // Each package...
                foreach (Package package in repo.Packages)
                {
                    // Don't bother if the super filter doesn't contain any of the tags...
                    if (!SuperFilter.Contains(package.Tags))
                        continue;

                    // Load the tags
                    foreach (string tag in package.Tags)
                    {
                        if (lvPackageFilter.Items.Contains(tag))
                            continue;

                        lvPackageFilter.Items.Add(tag, true); lvPackageFilter.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Enumurates packages from all avaliable repositories
        /// </summary>
        void EnumuratePackageRepositories()
        {
            // Restore all package controls to the cache...
            foreach (PackageControl packageControl in pcPackageList.Controls)
                packageControlCache.Enqueue(packageControl);

            // Clear the package list of controls...
            pcPackageList.Controls.Clear();

            // Start enumurating each repository...
            foreach (PackageRepository repository in Program.PackageManager.Repositories)
                EnumuratePackages(repository);

            // Select the last package in the list (which is at the top)
            if (pcPackageList.Controls.Count > 0)
                OnPackageSelect((PackageControl)pcPackageList.Controls[^1]);
        }

        /// <summary>
        /// Enumurates packages from a package repository
        /// </summary>
        /// <param name="repository"></param>
        void EnumuratePackages(PackageRepository repository)
        {
            foreach (Package package in repository.Packages)
            {
                // If the super filter doesn't contain any of the packages tags, we skip it...
                if (!SuperFilter.Contains(package.Tags))
                    continue;

                // Check if the package is being filtered by tags...
                bool isIncluded = false;

                foreach (string tag in package.Tags)
                {
                    // Get index of tag
                    int tagIndex = lvPackageFilter.Items.IndexOf(tag);

                    // Skip non included tags...
                    if (tagIndex < 0)
                        continue;

                    isIncluded |= lvPackageFilter.GetItemChecked(tagIndex);

                    if (isIncluded)
                        break;
                }

                // If the package isn't included, skip enumurating it...
                if (!isIncluded)
                    continue;

                // Calculate the package status...
                PackageStatusFlag flags = PackageStatusFlag.None;
                if (package.PackageExistsInCache())
                    flags |= PackageStatusFlag.Cached;
                if (InstallationTarget.RentingPackage(package))
                    flags |= PackageStatusFlag.Installed;

                // Try to grab a package control from the cache...
                PackageControl? packageControl = null;

                if (!packageControlCache.TryDequeue(out packageControl) || packageControl == null)
                {
                    // When we could dequeue, we make a new package control
                    packageControl = new(package, flags)
                    {
                        // Style Configuration
                        Dock = DockStyle.Top,
                    };

                    // Bind Event
                    packageControl.OnSelect += OnPackageSelect;
                }
                else
                    packageControl.LoadPackage(package, flags);

                // Add the control to the list...
                pcPackageList.Controls.Add(packageControl);
            }
        }

        /// <summary>
        /// When the form closes...
        /// </summary>
        void OnExitDialog(object sender, FormClosingEventArgs e)
        { }

        /// <summary>
        /// When a package is selected, we need to update the info pannel
        /// </summary>
        /// <param name="packageControl">The selected package control</param>
        void OnPackageSelect(PackageControl packageControl) =>
            exInfo.LoadPackageControl(currentPackage = packageControl);

        /// <summary>
        /// When a package filter is checked/unchecked we need to re-enumurate the package controls.
        /// </summary>
        void OnPackageFilterChecked(object sender, EventArgs e)
        {
            EnumuratePackageRepositories();

            // Remove the selected index
            lvPackageFilter.SelectedIndex = -1;
        }


        /// <summary>
        /// When the download/install/uninstall/update button is pressed, this event is ran.
        /// </summary>
        void OnPackageAction()
        {
            if (currentPackage == null)
                return;

            if (currentPackage.GetFlag(PackageStatusFlag.Installed) && currentPackage.GetFlag(PackageStatusFlag.OutOfDate))
                Logger.ShortInfo("Update Package!");
            else
            if (currentPackage.GetFlag(PackageStatusFlag.Installed))
                OnActionUninstall(currentPackage.PackageReference);
            else
            if (currentPackage.GetFlag(PackageStatusFlag.Cached))
                OnActionInstall(currentPackage.PackageReference);
            else
                OnActionDownload(currentPackage.PackageReference);

            // Re-Enumurate...
            EnumuratePackageRepositories();

            // No more busy
            BusyDialog.Instance.HideBusy();
        }

        void OnActionUninstall(Package package)
        {
            // Warn about dodgy uninstalls...
            if (MessageBox.Show("Uninstalling a package is very likely to just destroy the project/instance at the moment!\n\nAre you sure you want to do it?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            BusyDialog.Instance.ShowBusy();
            InstallationTarget.UninstallPackage(package);
        }

        void OnActionInstall(Package package)
        {
            BusyDialog.Instance.ShowBusy();
            InstallationTarget.InstallPackage(package);
        }

        void OnActionDownload(Package package)
        {
            BusyDialog.Instance.ShowBusy();
            Program.PackageManager.DownloadBundle(package);
        }
    }
}
