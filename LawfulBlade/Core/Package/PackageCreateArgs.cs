using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core.Package
{
    public struct PackageCreateArgs
    {
        [JsonInclude]
        public string   Name;

        [JsonInclude]
        public string   Description;

        [JsonInclude]
        public string   Version;

        [JsonInclude]
        public string[] Authors;

        [JsonInclude]
        public string[] Tags;

        [JsonInclude]
        public string[] Dependencies;

        [JsonIgnore]
        public string   Root;

        [JsonIgnore]
        public string   PackageName;

        public static PackageCreateArgs FromFile(string file)
        {
            PackageCreateArgs newArgs =
                JsonSerializer.Deserialize<PackageCreateArgs>(File.ReadAllText(file));

            newArgs.Root        = Path.GetDirectoryName(file);
            newArgs.PackageName = Path.GetFileNameWithoutExtension(file);

            return newArgs;
        }
    }
}
