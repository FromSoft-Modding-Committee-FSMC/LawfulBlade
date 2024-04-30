using LawfulBladeManager.Control;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;

using System.IO.Compression;
using System.Text.Json;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerDialog : System.Windows.Forms.Form
    {
        // Private Fields
        List<PackageControl> packageControls = new List<PackageControl>();

        // Public Fields & Properties
        public string[] SuperFilter { get; set; }

        public PackageManagerDialog()
        {
            InitializeComponent();

            // Default value because .NET complains otherwise
            SuperFilter = new string[] { string.Empty };
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            // Don't do when nully ehehehe
            if (Program.PackageManager == null)
                return;

            // Load Packages
            lvPackageFilter.Items.Clear();

            Task packageScanTask = new(() =>
            {
                // Open each archive and grab package.json
                foreach (string lbp in Program.PackageManager.PackageList)
                {
                    using (ZipArchive zip = ZipFile.OpenRead(lbp))
                    {
                        // Load package.json
                        ZipArchiveEntry? packEntry = zip.GetEntry("package.json");
                        if (packEntry == null)
                            throw new Exception($"Package '{Path.GetFileName(lbp)}' does not contain 'package.json'");

                        // Deserialize package.json
                        string packageJson = string.Empty;
                        using (StreamReader sr = new StreamReader(packEntry.Open()))
                            packageJson = sr.ReadToEnd();

                        Package package = JsonSerializer.Deserialize<Package>(packageJson, JsonSerializerOptions.Default);

                        // Check to see if this package is included via the super filter
                        if (!SuperFilter.Contains(package.Tags))
                            continue;

                        // Load icon.png
                        ZipArchiveEntry? iconEntry = zip.GetEntry("icon.png");
                        if (iconEntry == null)
                            throw new Exception($"Package '{Path.GetFileName(lbp)}' does not contain 'icon.png'");

                        // Create Image
                        Image icon = Image.FromStream(iconEntry.Open());

                        // Store information about the package in the mananger
                        Invoke((MethodInvoker)(() =>
                        {
                            // Load Tags
                            foreach (string tag in package.Tags)
                                if (!lvPackageFilter.Items.Contains(tag))
                                    lvPackageFilter.Items.Add(tag, true);

                            // Load Control
                            PackageControl pkgControl = new(package, icon)
                            {
                                Dock = DockStyle.Top,
                            };

                            pkgControl.pcMain.Click += PackageControl_Click;

                            pcPackageList.Controls.Add(pkgControl);
                            packageControls.Add(pkgControl);
                        }));
                    }
                }
            });
            packageScanTask.Start();
        }

        private void PackageControl_Click(object? sender, EventArgs e)
        {
            // Safe cast sender to panel
            if (sender is not Panel senderPanel)
                return;

            // Safe cast to package control
            if (senderPanel.Parent is not PackageControl packageControl)
                return;

            // Load information about the clicked package.
            exInfo.LoadMetadata(packageControl.package);
            exInfo.LoadIcon(packageControl.icon);
        }

        private void lvPackageFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // We must now rescan each package...
            pcPackageList.Controls.Clear();

            // Loop through each package control
            foreach (PackageControl pkgControl in packageControls)
            {
                Package package = pkgControl.package;

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
    }
}
