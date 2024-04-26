using LawfulBladeManager.Control;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Tagging;
using System.IO.Compression;
using System.Text.Json;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerForm : System.Windows.Forms.Form
    {
        List<PackageControl> packageControls = new List<PackageControl>();

        public PackageManagerForm()
        {
            InitializeComponent();
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
                            foreach (Tag tag in package.Tags)
                                if (!lvPackageFilter.Items.Contains(tag.Text))
                                    lvPackageFilter.Items.Add(tag.Text, true);

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
            if (sender == null)
                return;

            //Console.WriteLine($"Hello from {((PackageControl)(((Panel)sender).Parent)).package.Name}");
            exInfo.LoadMetadata(((PackageControl)(((Panel)sender).Parent)).package);
            exInfo.LoadIcon(((PackageControl)(((Panel)sender).Parent)).icon);
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
                foreach (Tag tag in package.Tags)
                {
                    // Get index of tag
                    int tagIndex = lvPackageFilter.Items.IndexOf(tag.Text);

                    // Is this tag valid?
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
