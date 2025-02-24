
using System.IO;
using System.Text.Json;

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
        public static RepositoryPackage[] GetPackagesByTag(string tag)
        {
            List<RepositoryPackage> foundPackages = [];

            foreach (Repository repository in Repositories)
                foreach (RepositoryPackage package in repository.PackageLibrary)
                    if (package.Tags.Contains(tag))
                        foundPackages.Add(package);

            return foundPackages.ToArray();
        }
    }
}
