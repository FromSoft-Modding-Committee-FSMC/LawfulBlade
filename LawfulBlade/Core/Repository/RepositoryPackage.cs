using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBlade.Core
{
    public struct RepositoryPackage
    {
        /// <summary>
        /// Name of the package
        /// </summary>
        [JsonInclude]
        public string Name { get; private set; }

        /// <summary>
        /// Description of the package
        /// </summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>
        /// Version of the package...
        /// </summary>
        [JsonInclude]
        public string Version { get; private set; }

        /// <summary>
        /// Authors of the package
        /// </summary>
        [JsonInclude]
        public string[] Authors { get; private set; }

        /// <summary>
        /// Any tags assosiated with the package
        /// </summary>
        [JsonInclude]
        public string[] Tags { get; private set; }

        /// <summary>
        /// UUID of the package...
        /// </summary>
        [JsonInclude]
        public string UUID { get; private set; }

        /// <summary>
        /// The small image. 48x48...
        /// </summary>
        [JsonInclude]
        public string Icon { get; private set; }

        /// <summary>
        /// True if the package bundle is locally cached...
        /// </summary>
        [JsonIgnore]
        public readonly bool IsCached => File.Exists(Path.Combine(App.PackageCachePath, $"{UUID}.IAZ"));

        /// <summary>
        /// Creates a RepositroyPackage from a Package
        /// </summary>
        public static RepositoryPackage FromPackage(Package.Package package)
        {
            RepositoryPackage repoPackage = new RepositoryPackage
            {
                Name        = package.Name,
                Description = package.Description,
                Version     = package.Version,
                Authors     = package.Authors,
                Tags        = package.Tags,
                UUID        = package.UUID,
                Icon        = package.Icon.ToBase64(MagickFormat.Png)
            };

            return repoPackage;
        }
    }
}
