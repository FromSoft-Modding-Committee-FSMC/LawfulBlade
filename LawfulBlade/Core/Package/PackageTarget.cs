using LawfulBlade.Core.Extensions;
using System.IO;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core.Package
{
    public abstract class PackageTarget
    {
        /// <summary>The root data location of the package target</summary>
        [JsonIgnore]
        public string Root { get; protected set; }

        /// <summary>If the target is dirty, and needs to be saved.</summary>
        [JsonIgnore]
        public bool Dirty { get; set; } = false;

        /// <summary>Any package references currently in the instance...</summary>
        [JsonIgnore]
        public List<PackageReference> Packages { get; protected set; }

        /// <summary>The file tree of the target</summary>
        [JsonIgnore]
        public Dictionary<string, string[]> Tree { get; protected set; }


        /// <summary>
        /// Installs a package to the target
        /// </summary>
        public virtual void InstallPackage(Package package)
        {
            // Open up the package bundle
            using ZipArchive bundle = ZipFile.Open(package.Bundle, ZipArchiveMode.Read);

            // Install package files
            bool packageHadConflict = false;

            foreach (PackageFile packageFile in package.Contents)
            {
                // Check for install conflicts
                if (PackageFileConflicting(packageFile))
                {
                    Debug.Warn($"Package file conflict {{ Package = {package.Name}, File = {packageFile.Path} }}");
                    packageHadConflict = true;
                }

                // Try to install the file.
                if (!PackageFileInstall(bundle, packageFile))
                    Debug.Error($"Package file not installed {{ Package = {package.Name}, File = {packageFile.Path} }}");
            }

            // Add a package reference
            Packages.Add(
                new PackageReference
                {
                    UUID                   = package.UUID,
                    Version                = package.Version,
                    Files                  = package.Contents,
                    InstalledWithConflicts = packageHadConflict
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
            // First, we want to find the package by its UUID
            PackageReference[] matchingPackages = Packages.Where(x => (x.UUID == package.UUID)).ToArray();

            if (matchingPackages.Length != 1)
            {
                if (matchingPackages.Length == 0)
                    Debug.Critical("Could not uninstall the package! It is not registered as installed! (REPORT THIS!!)");
                else
                if (matchingPackages.Length >= 2)
                    Debug.Critical("Could not uninstall the package! Multiple packages with the same UUID exist!");

                return;
            }

            // Now we remove each file owned by the package, so long as it still belongs to the package
            foreach (PackageFile file in matchingPackages[0].Files)
            {
                if (!PackageFileUninstall(file))
                    Debug.Critical($"Error uninstalling the package! {{ File = {file.Path} }}");
            }

            // Now all owned files have been removed, strip the package reference...
            Packages.Remove(matchingPackages[0]);

            // Mark as dirty..
            Dirty = true;
        }

        /// <summary>
        /// Checks if a package has been installed on to the target
        /// </summary>
        public virtual bool HasPackageByUUID(string uuid) =>
            Packages.Select(x => x.UUID).Contains(uuid);

        /// <summary>
        /// Checks if a file within a package has conflicts with the target
        /// </summary>
        bool PackageFileConflicting(PackageFile file)
        {
            // If the file doesn't exist in the tree at all, that's an immediate no.
            if (!Tree.TryGetValue(file.Path, out string[] leafNodes))
                return false;

            // If there is only one file in the tree, the file must be from the core.
            // there for, it's not a conflict ??
            if (leafNodes.Length == 1)
                return false;

            // Now check if the FNV64 of head matches with the current file
            // This shouldn't happen, but it's not a conflict either.
            if ($"{file.FNV64}" == leafNodes.Back())
                return false;

            // If we're here - it's a conflicting file.
            return true;
        }

        /// <summary>
        /// Installs a package file to the target
        /// </summary>
        bool PackageFileInstall(ZipArchive bundle, PackageFile file)
        {
            // Get the entry, make sure it exists.
            ZipArchiveEntry bundleEntry = bundle.GetEntry(file.Path);

            if (bundleEntry == null)
                return false;

            // Get the target absolute path, and directory path
            string targetFile = Path.Combine(Root, file.Path);
            string targetPath = Path.GetDirectoryName(targetFile);

            // Make sure the target path exists... If it doesn't, create it.
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            // Tree magic
            if (Tree.TryGetValue(file.Path, out string[] leafNodes))
            {
                // Get the history absolute path, and directory path
                string historyFile = Path.Combine(Root, ".history", file.Path);
                historyFile = historyFile.Replace(Path.GetFileName(historyFile), $"{leafNodes.Back()}_{Path.GetFileName(historyFile)}");
                string historyPath = Path.GetDirectoryName(historyFile);

                // Create the history directory if it does not exist...
                if (!Directory.Exists(historyPath))
                    Directory.CreateDirectory(historyPath);

                // Move the old file into history...
                File.Move(targetFile, historyFile, true);

                // Add this file to the tree on the current leaf.
                Tree[file.Path] = Tree[file.Path].Merge($"{file.FNV64:X16}");
            }
            else
                // Create a new leaf for this file.
                Tree[file.Path] = [$"{file.FNV64:X16}"];

            // Extract the package file from the bundle
            bundleEntry.ExtractToFile(targetFile, true);

            return true;
        }

        /// <summary>
        /// Uninstalls a package file from the target
        /// </summary>
        bool PackageFileUninstall(PackageFile file)
        {
            if (!Tree.TryGetValue(file.Path, out var leafNodes))
                return false;

            // Target file path
            string targetFile = Path.Combine(Root, file.Path);

            // Get the hash of the file as a string...
            string fileHash = $"{file.FNV64:X16}";

            if (leafNodes.Back() != fileHash)
            {
                // The head of the leaf is not owned by this package anymore, remove the leaf reference and the history file
                string historyFile = Path.Combine(Root, ".history", file.Path);
                historyFile = historyFile.Replace(Path.GetFileName(historyFile), $"{fileHash}_{Path.GetFileName(historyFile)}");

                File.Delete(historyFile);

                int indexOfNode = leafNodes.IndexOf(fileHash);

                if (indexOfNode != -1)
                    Tree[file.Path] = leafNodes.Remove(indexOfNode);
            }
            else
            {
                // There is only one file - which means this was added by the package, and can be removed.
                if (leafNodes.Length == 1)
                {
                    File.Delete(targetFile);
                    Tree.Remove(file.Path);
                } 
                else
                {
                    // There is more than one file, and we're the head. Pop this off and go back in history.
                    leafNodes = leafNodes.Remove(leafNodes.Length - 1);
                    Tree[file.Path] = leafNodes;

                    // History file path
                    string historyFile = Path.Combine(Root, ".history", file.Path);
                    historyFile = historyFile.Replace(Path.GetFileName(historyFile), $"{leafNodes.Back()}_{Path.GetFileName(historyFile)}");

                    // Replace target with history!
                    File.Move(historyFile, targetFile, true);
                }
            }

            return true;
        }
    }
}
