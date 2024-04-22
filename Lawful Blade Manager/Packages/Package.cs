using LawfulBladeManager.Tagging;
using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public struct Package
    {
        // Serialized Fields
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string Description;

        [JsonInclude]
        public string Version;

        [JsonInclude]
        public string[] Authors;

        [JsonInclude]
        public Tag[] Tags;

        [JsonInclude]
        public string UUID;
    }
}
