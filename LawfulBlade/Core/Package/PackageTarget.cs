using System.IO;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Windows;

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
        /// The file tree of the target
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, ulong> Tree { get; protected set; }

        /// <summary>
        /// Installs a package to the target
        /// </summary>
        public virtual void InstallPackage(Package package)
        {
            // We need to open up the bundle...
            using ZipArchive bundle = ZipFile.Open(package.Bundle, ZipArchiveMode.Read);

            // Pass 1: Check for any conflicts with the current target...
            bool hasConflict = false;

            foreach (PackageFile file in package.Contents)
            {
                // Check if the tree contains this file...
                if (Tree.ContainsKey(file.Path))
                {
                    Debug.Warn($"Conflicting Bundle File: '{file.Path}'");
                    hasConflict |= true;
                }
            }

            // In the case of a conflict, ask the user if they would like to continue...
            if (!Message.WarningYesNo("This package has conflicts with the target! Are you sure you want to install it?", hasConflict))
                return;

            // Pass 2: Now actually install the package.
            foreach (PackageFile file in package.Contents)
            {
                // Get the zip entry from the bundle. If it's missing, skip it.
                ZipArchiveEntry bundleEntry = bundle.GetEntry(file.Path);

                if (bundleEntry == null)
                {
                    Debug.Critical($"Missing Bundle File: '{file.Path}'!");
                    continue;
                }

                // Get the target absolute path
                string targetFile = Path.Combine(Root, file.Path);
                string targetPath = Path.GetDirectoryName(targetFile);

                // If the directory does not exist, it must be created...
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                // Add the file to the tree, overwritting if needed...
                Tree[file.Path] = file.FNV64;

                // Extract the file to the target file
                bundleEntry.ExtractToFile(targetFile, true);
            }

            // Add a package reference
            Packages.Add(
                new PackageReference
                {
                    UUID                   = package.UUID,
                    Version                = package.Version,
                    Files                  = package.Contents,
                    InstalledWithConflicts = hasConflict
                }
            );

            // Mark the target as dirty...
            Dirty = true;
        }

        /// <summary>
        /// Uninstalls a package from the target
        /// </summary>
        public virtual void UninstallPackage(Package package)
        {
            // Probably dangerous...
            PackageReference installedPackage = Packages.Where(x => (x.UUID == package.UUID)).ToArray()[0];

            // Now we need to compare each file of the installed package, with the files installed to the target
            // and remove them if the checksums are equal...
            foreach (PackageFile file in installedPackage.Files)
            {
                if (!Tree.ContainsKey(file.Path))
                    continue;

                // Check the FNV of the tree file against the package file...
                // If the checksums don't match, we can assume this file is now owned by another package.
                if (file.FNV64 == Tree[file.Path])
                {
                    string targetFile = Path.Combine(Root, file.Path);
                    string targetPath = Path.GetDirectoryName(targetFile);

                    // Remove the file...
                    File.Delete(targetFile);

                    // Check the folder - if it is now empty we can remove that too!
                    if (Directory.GetFileSystemEntries(targetPath).Length == 0)
                        Directory.Delete(targetPath);

                    // Remove the tree reference...
                    Tree.Remove(file.Path);
                }
            }

            // Now all owned files have been removed, strip the package reference...
            Packages.Remove(installedPackage);

            // Mark as dirty..
            Dirty = true;
        }

        public virtual bool HasPackageByUUID(string uuid) =>
            Packages.Select(x => x.UUID).Contains(uuid);
    }
}
