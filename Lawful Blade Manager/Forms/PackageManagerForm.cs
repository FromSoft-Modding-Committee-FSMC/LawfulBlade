using LawfulBladeManager.Packages;
using LawfulBladeManager.Tagging;
using System.IO.Compression;
using System.Text.Json;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerForm : System.Windows.Forms.Form
    {
        public PackageManagerForm()
        {
            InitializeComponent();
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            // Don't do when nully ehehehe
            if (Program.PackageManager == null)
                return;

            // Load Filters
            List<string> tagFilters = new();

            // Scan each package
            Task packageScanTask = new(() =>
            {
                // Open each archive and grab package.json
                foreach (string lbp in Program.PackageManager.PackageList)
                {
                    using (ZipArchive zip = ZipFile.OpenRead(lbp))
                    {
                        ZipArchiveEntry? packEntry = zip.GetEntry("package.json");
                        if (packEntry == null)
                            throw new Exception($"Package '{Path.GetFileName(lbp)}' does not contain 'package.json'");

                        // Deserialize package.json
                        string packageJson = string.Empty;
                        using (StreamReader sr = new StreamReader(packEntry.Open()))
                            packageJson = sr.ReadToEnd();

                        Package package = JsonSerializer.Deserialize<Package>(packageJson, JsonSerializerOptions.Default);

                        // Store information about the package in the mananger
                        foreach (Tag tag in package.Tags)
                            tagFilters.Add(tag.Text);
                    }

                    // Here we should add a new package control to the list.

                    Console.WriteLine($"Package [Source: {lbp}]");
                }

                this.Invoke((MethodInvoker)(() =>
                {
                    lvPackageFilter.Items.Clear();
                    foreach (string tag in tagFilters)
                    {
                        lvPackageFilter.Items.Add(tag);
                        lvPackageFilter.SetItemChecked(lvPackageFilter.Items.Count - 1, true);
                    }

                }));

                Console.WriteLine($"Tags Loaded = {tagFilters.Count}");
            });
            packageScanTask.Start();

            // Now copy the filter


        }
    }
}
