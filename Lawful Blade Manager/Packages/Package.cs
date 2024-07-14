using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public class Package
    {
        [JsonInclude]
        public string Name            { get; set; } = string.Empty;

        [JsonInclude]
        public string Description     { get; set; } = string.Empty;

        [JsonInclude]
        public string Version         { get; set; } = string.Empty;

        [JsonInclude]
        public string[] Authors       { get; set; } = Array.Empty<string>();

        [JsonInclude]
        public string[] Tags          { get; set; } = Array.Empty<string>();

        [JsonInclude]
        public string UUID            { get; set; } = string.Empty;

        [JsonInclude]
        public string BundleSourceUri { get; set; } = string.Empty;

        [JsonInclude]
        public string IconBase64      { get; set; } = string.Empty;
    }

    // Local Packages have additional information.
    public class LocalPackage : Package
    {
        [JsonInclude]
        public bool IsBundleCached   { get; set; } = false;

        [JsonInclude]
        public string BundleFilePath { get; set; } = string.Empty;
    }
}
