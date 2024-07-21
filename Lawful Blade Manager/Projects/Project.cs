using System.Text.Json.Serialization;

using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Projects
{
    public class Project : IPackageTarget
    {
        [JsonInclude]
        public string Name          { get; set; } = string.Empty;

        [JsonInclude]
        public string Description   { get; set; } = string.Empty;

        [JsonInclude]
        public string Author        { get; set; } = string.Empty;

        [JsonInclude]
        public string InstanceUUID  { get; set; } = string.Empty;

        [JsonInclude] 
        public string LastEditDate  { get; set; } = string.Empty;

        [JsonInclude]
        public string StoragePath   { get; set; } = string.Empty;

        [JsonInclude]
        public string[] Tags        { get; set; } = Array.Empty<string>();


        //
        // IPackageTarget Implementation
        //
        [JsonInclude]
        public Dictionary<string, PackageLibraryEntry> Library { get; set; } = new Dictionary<string, PackageLibraryEntry>();

        [JsonIgnore]
        public string[] CompatiblePackages => new string[] { "Project", "Sample" };

        public bool UninstallPackage(Package package) =>
            false;

        public bool InstallPackage(Package package) =>
            false;

        public bool RentingPackage(Package package) =>
            Library.ContainsKey(package.Name);

        public int CheckForOutdatedPackages() =>
            0;
    }
}
