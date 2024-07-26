using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Instances
{
    public class Instance : IPackageTarget
    {
        [JsonInclude]
        public string Name        { get; set; } = string.Empty;

        [JsonInclude]
        public string Description { get; set; } = string.Empty;

        [JsonInclude]
        public int IconID         { get; set; } = 0;

        [JsonInclude]
        public string UUID        { get; set; } = string.Empty;

        [JsonInclude]
        public string StoragePath { get; set; } = string.Empty;

        [JsonInclude]
        public string[] Tags      { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Uses an instance to open a project
        /// </summary>
        /// <param name="project">The project to open...</param>
        public void OpenProject(Project project)
        {
            // We must configure the current instance in the registry
            ProgramRegistry.SOMInstDir = StoragePath;

            // Create the special start argument for SOM_EDIT.exe ('instance'/'project')
            string startArg = $"{StoragePath}/{project.StoragePath}";

            // We must use a ProcessStartInfo object so we can use spaces in the filepath.
            Process? instanceProcess = Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(StoragePath, "tool", "SOM_EDIT.exe"),
                Arguments = $"\"{startArg}\"",

                // I want to capture STDOUT... This doesn't appear to be the complete way to do it though...
                RedirectStandardOutput = true
            });

            if (instanceProcess == null)
            {
                MessageBox.Show("Failed to open project in instance!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Wait for the process to exit...
            instanceProcess.WaitForExitAsync()
                .ContinueWith((Task obj) => // When the process does exit, we want to kill SOM_MAIN
                {
                    Process[] processes = Process.GetProcessesByName("SOM_MAIN");
                    if (processes.Length > 0)
                        processes[^1].Kill();
                });
        }

        /// <summary>
        /// Opens an instance directly
        /// </summary>
        public void Open()
        {
            // We must configure the current instance in the registry
            ProgramRegistry.SOMInstDir = StoragePath;

            // Start the SOM_MAIN process
            Process? instanceProcess = Process.Start(Path.Combine(StoragePath, "tool", "SOM_MAIN.exe"));
            if(instanceProcess == null)
                MessageBox.Show("Failed to open instance!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Check if the instance has the requested tag
        /// </summary>
        /// <param name="tag">requested tag</param>
        /// <returns>True if yes, false if no</returns>
        public bool HasTag(string tag) =>
            Tags.Contains(tag);

        /// <summary>
        /// Check if the instance has a core package.
        /// </summary>
        /// <returns>True if yes, false if no</returns>
        public bool HasCorePackage()
        {
            // Scan each package in the library to see if we've got a core package...
            foreach(string key in Library.Keys)
            {
                // Find the package this name
                if (!Program.PackageManager.FindPackageByName(key, out Package? package) || package == null)
                    return false;

                if (package.Tags.Contains("Core"))
                    return true;
            }

            // we return true if we find a core package in the above loop. If we exit without finding it, though...
            return false;
        }

        //
        // IPackageTarget Implementation
        //
        [JsonInclude]
        public Dictionary<string, PackageLibraryEntry> Library { get; set; } = new Dictionary<string, PackageLibraryEntry>();

        [JsonIgnore]
        public string[] CompatiblePackages => new string[] { "Editor", "Instance" };

        public bool UninstallPackage(Package package)
        {
            // Check if any other packages are dependant on this package...
            if (Program.PackageManager.FindDependantPackages(package, out Package[] dependents) > 0)
            {
                // Now check which of those packages are currently installed in this instance...
                List<string> dependentPackageNames = new List<string>();

                foreach(Package dependent in dependents)
                    if (RentingPackage(dependent))
                        dependentPackageNames.Add(dependent.Name);

                // When more than one is installed, we must show the dependant package warning dialog.
                if (dependentPackageNames.Count > 0)
                {
                    using PackageDependentsWarning pdw = new()
                    {
                        Title = $"Lawful Blade - Dependent Packages!",
                        Brief = "The package you are trying to uninstall has the following dependents:",
                        Warning = "If you continue with the installation you may completely brick your instance! \r\nPlease carefully consider before continuing.",
                        Dependents = dependentPackageNames.ToArray(),
                        AllowContinue = true
                    };

                    // If we didn't select continue, we exit now.
                    if (pdw.ShowDialog() != DialogResult.OK)
                        return false;
                }
            }

            // Get the library entry
            PackageLibraryEntry entry = Library[package.Name];

            // Delete each file
            foreach(PackageFile file in entry.Files)
            {
                string  targetFile      = Path.Combine(StoragePath, file.Filename);

                // Because of uninstall conflicts we have to check it exists first...
                if (File.Exists(targetFile))
                    File.Delete(targetFile);
            }

            // Remove the library entry...
            Library.Remove(package.Name);

            Program.InstanceManager.SaveInstances();

            return true;
        }

        public bool InstallPackage(Package package)
        {
            // Check if we have all of the packages dependencies already installed...
            List<string> uninstalledDependencies = new();
            foreach (string dependency in package.Dependencies)
            {
                if (dependency != string.Empty && !RentingPackage(dependency))
                    uninstalledDependencies.Add(dependency);
            }

            // When we have more than 0 dependencies uninstalled, we need to warn the user...
            if (uninstalledDependencies.Count > 0)
            {
                using PackageDependentsWarning pdw = new()
                {
                    Title = $"Lawful Blade - Missing Dependencies!",
                    Brief = "The package you are trying to install requires the following to be installed first:",
                    Warning = "Please install the above packages, and then install this one!",
                    Dependents = uninstalledDependencies.ToArray(),
                    AllowContinue = false
                };

                // If we didn't select continue, we exit now.
                if (pdw.ShowDialog() != DialogResult.OK)
                    return false;
            }


            // Open up the package bundle from the cache...
            using ZipArchive bundle = ZipFile.OpenRead(Path.Combine(Program.Preferences.PackageCacheDirectory, package.BundleSourceUri));

            // Get the file list
            PackageFile[] fileList = Package.BundleGetList(bundle);

            // Install each file to the target directory...
            foreach (PackageFile file in fileList)
            {
                // Combine the file with the storage path
                string targetFile      = Path.Combine(StoragePath, file.Filename);
                string targetDirectory = Path.GetDirectoryName(targetFile) ?? "POOPYC#LOL"; // This will never happen. shut the FUCK up...

                // Make sure the directory does exist when we need it...
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                // Install the file...
                bundle.GetEntry(file.Filename)?.ExtractToFile(Path.Combine(StoragePath, file.Filename), true);
            }
                
            // Create new library entry...
            Library.Add(package.Name, new PackageLibraryEntry
            {
                UUID = package.UUID,
                Version = package.Version,
                Files = fileList
            });

            // This is supposed to happen on shut down...
            Program.InstanceManager.SaveInstances();

            return true;
        }

        public bool RentingPackage(Package package) =>
            Library.ContainsKey(package.Name);

        public bool RentingPackage(string name) =>
            Library.ContainsKey(name);

        public int CheckForOutdatedPackages() =>
            0;
    }
}
