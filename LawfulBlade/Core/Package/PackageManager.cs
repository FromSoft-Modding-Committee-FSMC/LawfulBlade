
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
        static string RepositoryFile = Path.Combine(App.AppDataPath, "repositories.json");

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
                Repository repo = Repository.Load(repositoryUrl);
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
            File.WriteAllText(RepositoryFile, JsonSerializer.Serialize(Repositories.Select(repo => repo.URI)));
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
                }

                // If foundRepoPackage is set, we found the package and can break...
                if (foundRepoPackage != null)
                    break;
            }

            // If repo package is null now, we have an issue
            if (foundRepoPackage == null || parentRepo == null)
                throw new Exception($"Could not find package with UUID: {UUID}");

            // We've varified we have the package... Is it in the cache?
            string packageBundlePath = Path.Combine(App.PackageCachePath, $"{UUID}.IAZ");
            if (!foundRepoPackage.Value.IsCached)
                DownloadManager.DownloadSync(new Uri(Path.Combine(parentRepo.URI, foundRepoPackage.Value.Bundle)), packageBundlePath);

            // Load the package into memory...
            return Package.Load(packageBundlePath, false);
        }
    }
}
