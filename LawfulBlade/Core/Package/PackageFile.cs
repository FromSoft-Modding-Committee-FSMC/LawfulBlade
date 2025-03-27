using System.Text.Json.Serialization;

namespace LawfulBlade.Core.Package
{
    public struct PackageFile
    {
        [JsonInclude]
        public string Path;

        [JsonInclude]
        public ulong FNV64;
    }
}
