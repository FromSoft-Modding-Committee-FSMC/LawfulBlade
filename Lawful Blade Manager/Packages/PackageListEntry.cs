using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public struct PackageListEntry
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string SourceUri;
    }
}
