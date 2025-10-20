
using LawfulBlade.Core.Extensions;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows.Controls.Primitives;

namespace LawfulBlade.Core.Package
{
    public static class PackageManager
    {
        public static List<Repository> Repositories { get; private set; }

        /// <summary>
        /// The number of repositories avaliable
        /// </summary>
        public static long RepositoryCount => Repositories.Count;

        /// <summary>
        /// The number of packages avaliable
        /// </summary>
        public static long PackageCount { get; private set; }

        /// <summary>
        /// The file storing our repository links
        /// </summary>
        static readonly string RepositoryFile = Path.Combine(App.AppDataPath, "repositories.json");

        /// <summary>
        /// Initializes the package manager
        /// </summary>
        public static void Initialize()
        {
            // Create the package cache directory
            if (!Directory.Exists(App.PackageCachePath))
                Directory.CreateDirectory(App.PackageCachePath);

            string[] repositoryUrls;

            // Load currently active repositories... If there is no repository file, we add StandardLibrary...
            if (File.Exists(Path.Combine(App.AppDataPath, "repositories.json")))
                repositoryUrls = JsonSerializer.Deserialize<string[]>(File.ReadAllText(RepositoryFile));
            else
                repositoryUrls = [$"file:///{Path.Combine(App.ProgramPath, "StandardLibrary")}"];

            // Load the basic info for each repository...
            Repositories = [];

            foreach (string repositoryUrl in repositoryUrls)
            {
                if (!Repository.Load(repositoryUrl, out Repository repo))
                    continue;

                PackageCount += repo.PackageCount;
                Repositories.Add(repo);
            }
        }

        /// <summary>
        /// Call to shutdown the package manager...
        /// </summary>
        public static void Shutdown()
        {
            // Save the repositories file...
            // File.WriteAllText(RepositoryFile, JsonSerializer.Serialize(Repositories.Select(repo => repo.URI)));
        }

        /// <summary>
        /// Returns an array of packages which have the provided tag
        /// </summary>
        public static RepositoryPackage[] GetRepositoryPackagesByTag(string tag)
        {
            List<RepositoryPackage> foundPackages = [];

            foreach (Repository repository in Repositories)
                foreach (RepositoryPackage package in repository.PackageLibrary)
                    if (package.Tags.Contains(tag))
                        foundPackages.Add(package);

            return foundPackages.ToArray();
        }

        /// <summary>
        /// Gets all packages avaliable
        /// </summary>
        public static RepositoryPackage[] GetRepositoryPackages()
        {
            List<RepositoryPackage> foundPackages = [];

            foreach (Repository repo in Repositories)
                foundPackages.AddRange(repo.PackageLibrary);

            return foundPackages.ToArray();
        }

        public static Package GetPackageByUUID(string UUID)
        {
            Repository parentRepo = null;
            RepositoryPackage? foundRepoPackage = null;

            // Locate the package according to the UUID.
            foreach (Repository repo in Repositories)
            {
                // Scan through each package in the repo,
                foreach (RepositoryPackage repoPackage in repo.PackageLibrary)
                {
                    if (repoPackage.UUID != UUID)
                        continue;

                    parentRepo       = repo;
                    foundRepoPackage = repoPackage;

                    // We've found the package, so we can progress...
                    goto PackageScanSuccess;
                }
            }

            // If we're here, we failed to find the package. This shouldn't happen..
            throw new Exception($"Could not find package with UUID: {UUID}");

            // When we find a package in the above loop, we break early with a goto
            PackageScanSuccess:

            // Get the cache location for the package...
            string packageBundlePath = Path.Combine(App.PackageCachePath, $"{UUID}.IAZ");

            // Store the loaded package here...
            Package result;

            // Is the package cached? If it is, it might be old...
            if (foundRepoPackage.Value.IsCached)
            {
                Debug.Info($"Package exists in cache: '{foundRepoPackage.Value.Name}'");

                // We must lite load the cached version to see if we need to update the cache
                result = Package.Load(packageBundlePath);

                // Get the two versions as decimals
                decimal oldVersion = decimal.Parse(result.Version.GetDigits(), CultureInfo.InvariantCulture);
                decimal newVersion = decimal.Parse(foundRepoPackage.Value.Version.GetDigits(), CultureInfo.InvariantCulture);

                // Compare the two versions
                if (newVersion > oldVersion)
                {
                    // Re download the stale package...
                    Debug.Warn($"Package is stale. Reacquiring. [New Version = {foundRepoPackage.Value.Version}, Old Version = {result.Version})");
                    DownloadManager.DownloadSync(new Uri(Path.Combine(parentRepo.URI, foundRepoPackage.Value.Bundle)), packageBundlePath);

                    // Reload the package
                    result = Package.Load(packageBundlePath);
                }
            } 
            else
            {
                Debug.Info($"Downloading Package: '{foundRepoPackage.Value.Name}'");
                DownloadManager.DownloadSync(new Uri(Path.Combine(parentRepo.URI, foundRepoPackage.Value.Bundle)), packageBundlePath);

                // Load the downloaded package...
                result = Package.Load(packageBundlePath);

            }

            // return the found package.
            return result;
        }
    }
}
