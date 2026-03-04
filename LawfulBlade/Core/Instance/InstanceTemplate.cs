using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct InstanceTemplate
    {
        [JsonInclude]
        public string Type;

        [JsonInclude]
        public string[] Content;
    }
}
