using LawfulBladeManager.Type;
using System.Diagnostics;
using System.IO.Compression;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LawfulBladeManager.Packages
{
    public class PackageManager
    {
        // Event Delegates
        public delegate void OnVoidEvent();
        public event OnVoidEvent? OnPackagePrepareCompleted;

        /// <summary>
        /// Current state of the package manager...
        /// </summary>
        public PackageManagerState State { get; private set; } = PackageManagerState.None;

        /// <summary>
        /// Path to the repositories file.
        /// </summary>
        public static string RepositoriesFile { get; private set; } = Path.Combine(ProgramContext.AppDataPath, @"repositories.json");

        /// <summary>
        /// A list of repositories
        /// </summary>
        public List<PackageRepository> Repositories { get; private set; } = new();

        /// <summary>
        /// The number of repositories
        /// </summary>
        public int RepositoryCount { get; private set; } = 0;

        /// <summary>
        /// The number of packages
        /// </summary>
        public int PackageCount    { get; private set; } = 0;

        /// <summary>
        /// Default Constructor. Responsible for loading cached repositories
        /// </summary>
        public PackageManager()
        {
            // If the package cache directory doesn't exist, we can create it now.
            if (!Directory.Exists(Program.Preferences.PackageCacheDirectory))
            {
                Logger.ShortInfo("Creating packages cache directory...");
                Logger.ShortInfo($"\tPackagesCache = '{Program.Preferences.PackageCacheDirectory}'");

                Directory.CreateDirectory(Program.Preferences.PackageCacheDirectory);
            }

            // Attempt to load cached repositories. If we fail, initialize default ones...
            if (!LoadRepositories())
                DefaultRepositories();

            // Start a task to prepare our repositories...
            Logger.ShortInfo("Preparing repositories...");
            Task.Run(PrepareRepositories)
                .ContinueWith(PrepareRepositoriesFinished);

            // Save packages on shutdown
            Program.OnShutdown += SaveRepositories;
        }

        /// <summary>
        /// Load all repositories from the file system
        /// </summary>
        /// <returns>True on success, False otherwise.</returns>
        bool LoadRepositories()
        {
            Logger.ShortInfo("Loading cached repositories...");

            if (!File.Exists(RepositoriesFile))
                return false;

            // Load the repositories file...
            PackageRepository[]? tempRepositories = JsonSerializer.Deserialize<PackageRepository[]>(File.ReadAllText(RepositoriesFile), JsonSerializerOptions.Default);

            // Check if we successfully loaded the file...
            if (tempRepositories == null)
            {
                Logger.ShortError("Couldn't load cached repositories! The file is probably corrupt!");
                return false;
            }

            // We did successfully load it.
            Repositories = tempRepositories.ToList();

            // Success - return true.
            return true;
        }

        /// <summary>
        /// Saves cached repositories to the file system.
        /// </summary>
        bool SaveRepositories()
        {
            File.WriteAllText(RepositoriesFile, JsonSerializer.Serialize(Repositories, JsonSerializerOptions.Default));
            return true;
        }
            
        /// <summary>
        /// Initializes default repositories for lawful blade
        /// </summary>
        void DefaultRepositories()
        {

        }

        /// <summary>
        /// Prepares repositories by polling for updates etc
        /// </summary>
        void PrepareRepositories()
        {
            // Set State for preparing packages...
            State = PackageManagerState.PreparingPackages;

            // Set the number of repositories
            RepositoryCount = Repositories.Count;

            // Count the number of packages...
            foreach (PackageRepository repo in Repositories)
                PackageCount += repo.Packages.Length;
        }

        /// <summary>
        /// Called by the prepare repositories task on completion.
        /// </summary>
        void PrepareRepositoriesFinished(Task obj)
        {
            // Set State
            State = PackageManagerState.Ready;

            Logger.ShortInfo($"Loaded {PackageCount} packages(s) from {RepositoryCount} repositories!");

            // Invoke any events waiting for packages to complete...
            OnPackagePrepareCompleted?.Invoke();
        }

        /// <summary>
        /// Creates a new package and saves it to the file system.
        /// </summary>
        /// <param name="args">Properties to create the package with</param>
        public static void CreatePackage(PackageCreateArgs args)
        {
            // Get the directory of the package
            string? packageDirectory = Path.GetDirectoryName(args.TargetFile);
            if (packageDirectory == null)   // Should never happen but C# doesn't STFU
                return;

            // Find package files
            List<PackageFile> packageList = new ();
            foreach(string filePath in Directory.EnumerateFileSystemEntries(args.SourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                // If the file doesn't exist, it must be a directory - which we don't want...
                if (!File.Exists(filePath))
                    continue;

                packageList.Add(new PackageFile
                {
                    Filename = filePath.Replace(args.SourceDirectory, "")[1..],
                    Checksum = Convert.ToBase64String(MD5.HashData(File.ReadAllBytes(filePath)))
                });
            }

            // Create package meta
            Package packageMeta = new()
            {
                // Create the basic package by copying info from the args
                Name        = args.Name,
                Description = args.Description,
                Version     = args.Version,
                Authors     = args.Authors,
                Tags        = args.Tags,

                // Generate the package UUID using a MD5 hash of the name
                UUID = new Guid(MD5.HashData(Encoding.UTF8.GetBytes(args.Name))).ToString(),

                // Bundle source is a relative URI  (paz = 'possibily a zip'... because it's a zip =D )
                BundleSourceUri = $"{args.TargetFile.Replace(packageDirectory, "")[1..]}",
                
                // The Base64 encoded icon (can also be an empty string)
                IconBase64 = Package.EncodeIcon(args.IconSource)
            };

            // Create package bundle -- if the file exists, delete it.
            if (File.Exists(args.TargetFile))
                File.Delete(args.TargetFile);

            using (ZipArchive packageBundle = ZipFile.Open(args.TargetFile, ZipArchiveMode.Create))
            {
                // Write meta to the bundle
                ZipArchiveEntry metaEntry = packageBundle.CreateEntry(@"package.meta.json");
                using (StreamWriter sw = new(metaEntry.Open()))
                    sw.Write(JsonSerializer.Serialize(packageMeta, JsonSerializerOptions.Default));

                // Write file list to the bundle
                ZipArchiveEntry listEntry = packageBundle.CreateEntry(@"package.list.json");
                using (StreamWriter sw = new(listEntry.Open()))
                    sw.Write(JsonSerializer.Serialize(packageList, JsonSerializerOptions.Default));

                // Add files to the package
                foreach (PackageFile file in packageList)
                    packageBundle.CreateEntryFromFile(Path.Combine(args.SourceDirectory, file.Filename), file.Filename);
            }
        }

        /// <summary>
        /// Creates a new package repository in the file system.
        /// </summary>
        /// <param name="args">Properties to create the package repository with</param>
        public static void CreateRepository(PackageRepositoryCreateArgs args)
        {
            // Generate a safe file name using the package name...
            string repoFilename = args.Name.Replace(" ", "_");    // Replace spaces with underscores....

            // Generate directories and filepaths
            string infFile = Path.Combine(args.OutputDirectory, $"{repoFilename}.inf");
            string cpkFile = Path.Combine(args.OutputDirectory, $"{repoFilename}.cpk");
            string bundles = Path.Combine(args.OutputDirectory, repoFilename);

            if (!Directory.Exists(bundles))
                Directory.CreateDirectory(bundles);

            // Create the info data for serialization, then serialize and write it to the file system.
            PackageRepositoryInfo info = new PackageRepositoryInfo
            {
                Name         = args.Name,
                Description  = args.Description,
                CreationDate = DateTime.Now,
                // The URI field is only used for the local cache.
                URI          = string.Empty
            };

            File.WriteAllText(infFile, JsonSerializer.Serialize(info, JsonSerializerOptions.Default));

            // Create the package list and copy bundles...
            List<Package> bundleList = new();

            foreach(string bundleFile in args.BundleSources)
            {
                // Load the package
                Package tempPackage = Package.Load(bundleFile);

                // Get the filename and target path for the bundle
                string? bundleFileName = Path.GetFileName(bundleFile) ?? throw new Exception($"Couldn't get package filename! '{bundleFile}'");
                string  bundleFilePath = Path.Combine(bundles, bundleFileName);

                // Alter the URI to be relative to the inf and cpk files...
                tempPackage.BundleSourceUri = bundleFileName;

                // Add to the list of packages to get bundle-ized
                bundleList.Add(tempPackage);

                // Copy the bundle file relative to the inf and cpk files...
                File.Copy(bundleFile, Path.Combine(bundles, bundleFileName), true);
            }
                
            PackageRepository.SaveBundle(cpkFile, bundleList.ToArray());
        }

        /// <summary>
        /// Adds a new repository to the package manager
        /// </summary>
        /// <param name="uri">Base URI of the repository</param>
        public void AddRepository(Uri uri)
        {
            // Download the repository...
            Uri fileUri       = uri.Append(".inf");
            string fileInfo   = Program.DownloadManager.DownloadFileSync(fileUri);

            Uri bundleUri     = uri.Append(".cpk");
            string fileBundle = Program.DownloadManager.DownloadFileSync(bundleUri);

            // Create a repository object with the downloaded things...
            PackageRepositoryInfo info   = PackageRepository.LoadInfo(fileInfo);
            Package[] bundle             = PackageRepository.LoadBundle(fileBundle);

            // Add URI to the info field for the local cache...
            info.URI = uri.ToString();

            PackageRepository repository = new PackageRepository(info, bundle);

            // Add repository to the repository list...
            Repositories.Add(repository);

            // Save cached repositories...
            SaveRepositories();
        }

        /// <summary>
        /// Check if a repository already exists in the manager.
        /// </summary>
        /// <param name="uri">URI of the source</param>
        /// <param name="repository">[OUT] Reference to the package repository</param>
        /// <returns>True if it is contained, false otherwise</returns>
        public bool FindRepositoryByURI(string uri, out PackageRepository? repository)
        {
            foreach (PackageRepository repo in Repositories)
            {
                if (repo.Info.URI != uri)
                    continue;

                repository = repo;
                return true;
            }

            repository = null;
            return false;
        }
    }
}
