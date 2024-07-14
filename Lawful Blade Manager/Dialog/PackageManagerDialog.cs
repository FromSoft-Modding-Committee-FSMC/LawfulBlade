using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerDialog : System.Windows.Forms.Form
    {
        // Private Fields
        readonly List<PackageControl> packageControls = new();
        PackageControl? currentPackage = null;

        PackageLibrary library;

        // Public Fields & Properties
        public string[] SuperFilter { get; set; }
        public string InstallationRoot { get; set; }

        public PackageManagerDialog()
        {
            InitializeComponent();

            // Default value because .NET complains otherwise
            SuperFilter = new string[] { string.Empty };
            InstallationRoot = string.Empty;
        }

        void PackageManagerForm_Load(object sender, EventArgs e)
        {
            // Don't do when nully ehehehe
            if (Program.PackageManager == null)
                return;

            // Create the library.json file
            string libraryPath = Path.Combine(InstallationRoot, "library.json");

            if (File.Exists(libraryPath))
                library = JsonSerializer.Deserialize<PackageLibrary>(File.ReadAllText(libraryPath), JsonSerializerOptions.Default);
            else
            {
                // Get default (empty) library.
                library = PackageLibrary.Default;

                // Save library to disk.
                File.WriteAllText(libraryPath, JsonSerializer.Serialize(library, JsonSerializerOptions.Default));

                Console.WriteLine("Package library does not exist... setting up for first time package installation.");
            }

            Console.WriteLine($"Installed Packages [{InstallationRoot}] = {{ \n\t{string.Join(",\n\t", library.installedPackages.Keys)} }}");


            // Load Packages
            lvPackageFilter.Items.Clear();

            // Define a task to scan for avaliable packages
            Task packageScanTask = new(() =>
            {
                // Start the busy dialog
                BusyDialog.Instance.ShowBusy();

                // Open each archive and grab package.json
                foreach (string lbp in Program.PackageManager.PackageList)
                {
                    // Open Zip File
                    ZipArchive zip = ZipFile.OpenRead(lbp);

                    // Load package.json
                    ZipArchiveEntry? packEntry = zip.GetEntry("package.json");
                    if (packEntry == null)
                        throw new Exception($"Package '{Path.GetFileName(lbp)}' does not contain 'package.json'");

                    // Deserialize package.json
                    string packageJson = string.Empty;
                    using (StreamReader sr = new(packEntry.Open()))
                        packageJson = sr.ReadToEnd();

                    Package package = JsonSerializer.Deserialize<Package>(packageJson, JsonSerializerOptions.Default);

                    // Check to see if this package is included via the super filter
                    if (!SuperFilter.Contains(package.Tags))
                        continue;

                    // Load icon.png
                    Image icon;

                    ZipArchiveEntry? iconEntry = zip.GetEntry("icon.png");
                    if (iconEntry != null)
                        icon = Image.FromStream(iconEntry.Open());
                    else
                        icon = new Bitmap(Properties.Resources._128x_package);  // Default package image just in case...

                    // Load package.list.json
                    ZipArchiveEntry? fileListEntry = zip.GetEntry("package.list.json");
                    if (fileListEntry == null)
                        throw new Exception($"Package '{Path.GetFileName(lbp)}' does not contain 'package.list.json'");

                    string packageListJson = string.Empty;
                    using (StreamReader sr = new(fileListEntry.Open()))
                        packageListJson = sr.ReadToEnd();

                    PackageFile[]? packageFiles = JsonSerializer.Deserialize<PackageFile[]>(packageListJson, JsonSerializerOptions.Default);

                    if (packageFiles == null)
                        throw new Exception($"Package '{Path.GetFileName(lbp)}' contains corrupted 'package.list.json'");

                    // Create package control on the main thread.
                    Invoke((MethodInvoker)(() =>
                    {
                        // Load Tags
                        foreach (string tag in package.Tags)
                            if (!lvPackageFilter.Items.Contains(tag))
                                lvPackageFilter.Items.Add(tag, true);

                        // Load Control
                        PackageControl pkgControl = new(package, icon)
                        {
                            Dock       = DockStyle.Top,
                            Files      = packageFiles,
                            PackageZip = zip
                        };

                        pkgControl.OnSelect += PkgControl_OnSelect;

                        pcPackageList.Controls.Add(pkgControl);
                        packageControls.Add(pkgControl);

                        // Does the library already contain this package?
                        if (library.Contains(package.UUID))
                            pkgControl.UpdateStatus(PackageStatus.Installed);

                    }));
                }
            });

            // Start the package scan task.
            packageScanTask.Start();

            // Define a callback which is executed when the package scan task completes...
            packageScanTask.ContinueWith(t =>
            {
                // Load the first package in the list
                if (packageControls.Count > 0)
                    Invoke((MethodInvoker)(() => LoadPackageInfo(packageControls[^1])));  // Invoke on parent thread


                // Scan for conflicts.

                // Each target for packages should have a 'library.json' file ...
                // Load it to get info about installed packages.

                // Check each package to see if it conflicts with one installed.

                // Bind the package info install button.
                exInfo.OnInstallPressed += OnInstallPressed;

                // Hide Busy Pannel
                BusyDialog.Instance.HideBusy();
            });
        }

        void PackageManagerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close all open zips
            foreach (PackageControl pkgControl in packageControls)
            {
                if (pkgControl.PackageZip == null)
                    continue;

                pkgControl.PackageZip.Dispose();
            }
                

            // When we close the form, we save the library
            File.WriteAllText(Path.Combine(InstallationRoot, "library.json"), JsonSerializer.Serialize(library, JsonSerializerOptions.Default));
        }

        void PkgControl_OnSelect(PackageControl packageControl) =>
            LoadPackageInfo(packageControl);

        void LoadPackageInfo(PackageControl packageControl)
        {
            // Set current package
            currentPackage = packageControl;

            // Load Info into info plane
            exInfo.LoadMetadata(packageControl.package);
            exInfo.LoadIcon(packageControl.icon);
            exInfo.LoadStatus(currentPackage.Status);
        }

        void OnUpdateStatus()
        {
            if (currentPackage == null)
                return;

            exInfo.LoadStatus(currentPackage.Status);
        }

        void lvPackageFilter_ItemCheck(object sender, ItemCheckEventArgs e)
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

        void OnInstallPressed()
        {
            // Don't process request when we don't have a selected package...
            if (currentPackage == null)
                return;

            if (currentPackage.PackageZip == null)
                throw new Exception("File storage for current package is invalidated!");

            // Grab package struct from the current package.
            Package package = currentPackage.package;

            // What is the current state of the package? we want to perform different actions depending on that...
            if (currentPackage.Status == PackageStatus.Uninstalled || currentPackage.Status == PackageStatus.Conflicts)
            {
                // If for some reason we're in this branch, but the library already contains this package, exit **now**
                if (library.Contains(package.UUID))
                    return; // Should never happen

                // We're branching on two statuses right now, if we're in conflict we need to show a different message
                // but perform a common action
                DialogResult result = DialogResult.OK;

                if (currentPackage.Status == PackageStatus.Conflicts)
                    result = MessageBox.Show("This package conflicts with one already installed. \nAre you sure you wish to continue?", "Lawful Blade", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result != DialogResult.OK)
                    return; // Exit early if the user doesn't want to install the package

                // We must log installed package files...
                List<PackageFile> installedFiles = new();

                // Lets install that package.
                BusyDialog.Instance.ShowBusy();

                // For each file...
                foreach(PackageFile packageFile in currentPackage.Files)
                {
                    // Make absolute path
                    string outputPath = Path.Combine(InstallationRoot, packageFile.Filename);
                    string? outputDir = Path.GetDirectoryName(outputPath);

                    // Make sure output directories exist (recursively)
                    if (outputDir != null && !Directory.Exists(outputDir))
                        Directory.CreateDirectory(Path.GetFullPath(outputDir));

                    bool shouldInstallFile = true;

                    // Does this file already exist?
                    if(File.Exists(outputPath))
                    {
                        // Does this the existing file match this one?
                        string oldChecksum = Convert.ToBase64String(MD5.HashData(File.ReadAllBytes(outputPath)));

                        shouldInstallFile = package.ExpectOverwrite;

                        if (oldChecksum != packageFile.Checksum && !package.ExpectOverwrite)
                            shouldInstallFile = (MessageBox.Show($"File '{packageFile.Filename}' already exists in '{InstallationRoot}.\nDo you want to replace it?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes);
                    }

                    // Do installation...
                    if(shouldInstallFile)
                    {
                        // Get zip entry...
                        ZipArchiveEntry? entry = currentPackage.PackageZip.GetEntry(packageFile.Filename);

                        // Skip bad entries...
                        if (entry == null)
                            continue;

                        // Extract to output...
                        entry.ExtractToFile(outputPath, true);

                        // t'is installed...
                        installedFiles.Add(packageFile);
                    }
                }

                BusyDialog.Instance.HideBusy();

                // Add to library
                library.installedPackages.Add(package.UUID, new PackageLibraryEntry
                {
                    Version = package.Version,
                    Files = installedFiles.ToArray()
                });

                // Update status...
                currentPackage.UpdateStatus(PackageStatus.Installed);
                OnUpdateStatus();
            }
            else
            if (currentPackage.Status == PackageStatus.Installed)
            {
                // If for some reason we're in this branch, but the library doesn't contain this package, exit **now**
                if (!library.Contains(package.UUID))
                    return; // Also should never happen though

                if (MessageBox.Show($"You are about to uninstall '{package.Name}'!\n" +
                    $"The installation path for this package is apparently: '{InstallationRoot}'.\n\n" +
                    $"Are you sure you wish to continue?",                   
                    "Lawful Blade", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return; // Exit early if the user doesn't want to uninstall the package.
                }
                    

                // We must only remove files owned by the package
                PackageFile[] installedFiles = library.installedPackages[package.UUID].Files;
                List<string> foundDirectories = new();

                // Lets uninstall that package.
                BusyDialog.Instance.ShowBusy();

                foreach (PackageFile packageFile in installedFiles)
                {
                    string outputPath = Path.Combine(InstallationRoot, packageFile.Filename);
                    string? outputDir = Path.GetDirectoryName(outputPath);

                    // Skip files with bad directories...
                    if (outputDir == null)
                        continue;

                    // Does the file exist?..
                    if (File.Exists(outputPath))
                    {
                        // Has this directory been found, and is it our installation root?
                        if(!foundDirectories.Contains(outputDir) && outputDir != InstallationRoot)
                            // It hasn't and isn't, so we can flag it for deletion.
                            foundDirectories.Add(outputDir);

                        // Check for matching checksum before deleting
                        if (Convert.ToBase64String(MD5.HashData(File.ReadAllBytes(outputPath))) == packageFile.Checksum)
                            File.Delete(outputPath);
                    }
                }

                // Now scan for empty source directories to remove
                foreach(string directory in foundDirectories)
                {
                    if (Directory.Exists(directory) && Directory.GetFiles(directory).Length == 0)
                        Directory.Delete(directory, false);                 
                }

                BusyDialog.Instance.HideBusy();

                // Remove from library...
                library.installedPackages.Remove(package.UUID);

                // Update status...
                currentPackage.UpdateStatus(PackageStatus.Uninstalled);
                OnUpdateStatus();
            }

            // Save Library.json to disk
            File.WriteAllText(Path.Combine(InstallationRoot, "library.json"), JsonSerializer.Serialize(library, JsonSerializerOptions.Default));
        }
    }
}
