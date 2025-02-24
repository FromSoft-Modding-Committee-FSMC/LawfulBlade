using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct RepositoryCreateArgs
    {
        [JsonInclude]
        public string   Name;

        [JsonInclude]
        public string   Description;

        [JsonInclude]
        public string   PackageRoot;

        [JsonInclude]
        public string   CreateRoot;

        [JsonInclude]
        public string[] IncludedPackages;

        public static RepositoryCreateArgs FromFile(string file)
        {
            RepositoryCreateArgs newArgs =
                JsonSerializer.Deserialize<RepositoryCreateArgs>(File.ReadAllText(file));


            newArgs.CreateRoot  = Path.GetDirectoryName(file);
            newArgs.PackageRoot = Path.Combine(newArgs.CreateRoot, newArgs.PackageRoot);

            return newArgs;
        }
    }
}
