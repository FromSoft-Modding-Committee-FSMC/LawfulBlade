using System;
using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public struct PackageLibrary
    {
        // Default Library
        public static PackageLibrary Default => new()
        {
            installedPackages = new Dictionary<string, PackageLibraryEntry>()
        };

        [JsonInclude]
        public Dictionary<string, PackageLibraryEntry> installedPackages;  // UUID, Info

        public bool Contains(string uuid) =>
            installedPackages.ContainsKey(uuid);
    }

    public struct PackageLibraryEntry
    {
        [JsonInclude]
        public string Version;

        [JsonInclude]
        public PackageFile[] Files;
    }
}
