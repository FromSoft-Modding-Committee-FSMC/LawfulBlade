using System.Text.Json.Serialization;

namespace LawfulBladeManager.Projects
{
    public struct Project
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string Description;

        [JsonInclude]
        public string Author;

        [JsonInclude]
        public string InstanceUUID;

        [JsonInclude]
        public string LastEditData;

        [JsonInclude]
        public string StoragePath;

        [JsonInclude]
        public bool IsManaged;

        [JsonInclude]
        public List<int> TagIDs;
    }
}
