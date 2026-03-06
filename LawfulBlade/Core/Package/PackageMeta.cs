using System.Text.Json.Serialization;

namespace LawfulBlade.Core.Package
{
    public struct PackageMeta
    {
        [JsonInclude]
        public Dictionary<string, InstanceCommand> Commands { get; private set; }

        [JsonInclude]
        public InstanceVariable[] Variables { get; private set; }

        [JsonInclude]
        public InstanceTemplate[] Templates { get; private set; }

        [JsonInclude]
        public InstanceTool[] Tools { get; private set; }
    }
}
