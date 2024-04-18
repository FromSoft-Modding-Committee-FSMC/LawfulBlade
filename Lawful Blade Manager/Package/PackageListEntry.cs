using System.Text.Json.Serialization;

namespace LawfulBladeManager.Package
{
    public struct PackageListEntry
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string SourceUri;
    }
}
