using System.Diagnostics;
using System.Text.Json.Serialization;

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

            Process? instanceProcess = Process.Start(Path.Combine(StoragePath, "tool", "SOM_EDIT.exe"), startArg);
            if (instanceProcess == null)
                MessageBox.Show("Failed to open project in instance!", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //
        // IPackageTarget Implementation
        //
        [JsonInclude]
        public Dictionary<string, string> Library { get; set; } = new Dictionary<string, string>();

        public string[] CompatiblePackages => new string[] { "Editor", "Instance" };

        public bool InstallPackage(Package package) =>
            false;

        public bool RentingPackage(Package package) =>
            Library.ContainsKey(package.UUID);

        public int CheckForOutdatedPackages() =>
            0;
    }
}
