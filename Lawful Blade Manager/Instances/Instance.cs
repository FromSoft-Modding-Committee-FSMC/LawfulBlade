using System.Text.Json.Serialization;

namespace LawfulBladeManager.Instances
{
    public struct Instance
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string Description;

        [JsonInclude]
        public string InstanceUUID;

        [JsonInclude]
        public string StoragePath;

        [JsonInclude]
        public string[] Tags;

        // To facilitate selection boxes, we must have an override for ToString.
        public override string ToString() =>
            Name;
    }
}
