using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerDialog : Form
    {
        // Private Fields
        readonly List<PackageControl> packageControls = new();
        PackageControl? currentPackage = null;

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
            // Scan for packages to display in the manager...
            Task packageSearchTask = new(() =>
            {
                // Busy...
                BusyDialog.Instance.ShowBusy();

                // We go through each package source and package currently in the mananger...
                foreach (PackageRepository repo in Program.PackageManager.Repositories)
                {
                    // With each source...
                    foreach (Package package in repo.Packages)
                    {
                        // Is this package included by the super filter?
                        if (!SuperFilter.Contains(package.Tags))
                            continue;

                        // Load the package into the manager... We must do this on the main thread.
                        Invoke(() =>
                        {
                            // Tags
                            foreach (string tag in package.Tags)
                            {
                                if (lvPackageFilter.Items.Contains(tag))
                                    continue;

                                lvPackageFilter.Items.Add(tag, true);
                            }

                            // Package Control Flags
                            PackageStatusFlag packageFlags = PackageStatusFlag.None;

                            if (package.PackageExistsInCache())
                                packageFlags |= PackageStatusFlag.Cached;

                            // Construct a new package control
                            PackageControl packageControl = new(package, packageFlags)
                            {
                                // Style Configuration
                                Dock = DockStyle.Top,
                            };

                            // Bind to selection event
                            packageControl.OnSelect += OnPackageSelect;

                            // Add control to lists...
                            pcPackageList.Controls.Add(packageControl);
                            packageControls.Add(packageControl);            // TO-DO: clean this up. (Enumerate Packages)
                        });
                    }
                }
            });

            // After the scan completes, we set some initial data in the info panel
            packageSearchTask.ContinueWith(t =>
            {
                // Load the first package into the info panel - on main thread.
                if (packageControls.Count > 0)
                    Invoke(() => OnPackageSelect(packageControls[^1]));

                // Bind the install button...
                exInfo.OnInstallPressed += OnPackageAction;

                // We're no longer busy
                BusyDialog.Instance.HideBusy();
            });

            // Start the scanning task.
            packageSearchTask.Start();
        }

        /// <summary>
        /// When the form closes...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnExitDialog(object sender, FormClosingEventArgs e) =>
            throw new NotImplementedException();

        /// <summary>
        /// When a package is selected, we need to update the info pannel
        /// </summary>
        /// <param name="packageControl">The selected package control</param>
        void OnPackageSelect(PackageControl packageControl) =>
            exInfo.LoadPackageControl(currentPackage = packageControl);

        /// <summary>
        /// When a package filter is checked/unchecked we need to re-enumurate the package controls.
        /// </summary>
        void OnPackageFilterChecked(object sender, ItemCheckEventArgs e)
        {
            // We must now rescan each package...
            pcPackageList.Controls.Clear();

            // Loop through each package control
            foreach (PackageControl pkgControl in packageControls)
            {
                Package package = pkgControl.PackageReference;

                // Store check state
                bool packageFiltered = true;

                // Loop through each tag in the package
                foreach (string tag in package.Tags)
                {
                    // Get index of tag
                    int tagIndex = lvPackageFilter.Items.IndexOf(tag);

                    // Is this tag valid? This whole mess is here because WinForms fucking sucks.
                    if (tagIndex >= 0 & tagIndex != e.Index)
                    {
                        // Is this tag checked?
                        packageFiltered &= (!(lvPackageFilter.GetItemChecked(tagIndex)));
                    }
                    else
                    if (tagIndex == e.Index)
                    {
                        packageFiltered &= (e.NewValue == CheckState.Unchecked);
                    }

                    if (!packageFiltered)
                        break;
                }

                if (!packageFiltered)
                    pcPackageList.Controls.Add(pkgControl);
            }
        }

        /// <summary>
        /// When the download/install/uninstall button is pressed, this event is ran.
        /// </summary>
        void OnPackageAction()
        {
            // If the package is not cached, we must download it...

        }
    }
}
