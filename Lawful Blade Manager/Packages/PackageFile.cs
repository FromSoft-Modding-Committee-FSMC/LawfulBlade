using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public struct PackageFile
    {
        [JsonInclude]
        public string Filename;

        [JsonInclude]
        public string Checksum;
    }
}
