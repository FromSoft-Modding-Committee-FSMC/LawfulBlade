using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct InstanceTemplate
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string Description;

        [JsonInclude]
        public string ContentSource;

        [JsonInclude]
        public InstanceTemplateControl[] Controls;
    }

    public struct InstanceTemplateControl
    {
        [JsonInclude]
        public string Type;

        [JsonInclude]
        public string[] Arguments;

        [JsonInclude]
        public string[] Content;

        // MakeDirectory {target}           <-- Make a directory
        // CopyDirectory {source} {target}  <-- Copy a single directory
        // MakeFile {target}                <-- Makes a file
        // CopyFile {source} {target}       <-- Copies a single file
        // CopyContent                      <-- Copies content directory
    }
}
