using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Package
{
    public abstract class PackageTarget
    {
        /// <summary>
        /// The root data location of the package target
        /// </summary>
        [JsonIgnore]
        public string Root { get; protected set; }

        /// <summary>
        /// If the target is dirty, and needs to be saved.
        /// </summary>
        [JsonIgnore]
        public bool Dirty { get; set; } = false;

        /// <summary>
        /// Any package references currently in the instance...
        /// </summary>
        [JsonIgnore]
        public List<PackageReference> Packages { get; protected set; }

        /// <summary>
        /// Installs a package to the target
        /// </summary>
        /// <param name="package"></param>
        public virtual void InstallPackage(Package package)
        {
            // We need to open up the bundle...
            using ZipArchive bundle = ZipFile.Open(package.Bundle, ZipArchiveMode.Read);

            // And extract each file to the root...
            foreach (PackageFile file in package.Contents)
            {
                ZipArchiveEntry bundleEntry = bundle.GetEntry(file.Path);
                if (bundleEntry == null)
                {
                    Debug.Critical($"Missing Bundle File: '{file.Path}'!");
                    continue;
                }

                // Make the target path for the file...
                string targetPath = Path.Combine(Root, file.Path);

                // Got to make the path for the file if it doesn't exist...
                if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

                // Now actual extraction
                bundleEntry.ExtractToFile(targetPath, true);
            }

            // Finally, add this package to the package references list
            Packages.Add(new PackageReference { UUID = package.UUID, Version = package.Version });
        }

        public virtual bool HasPackageByUUID(string uuid) =>
            Packages.Select(x => x.UUID).Contains(uuid);
    }
}
