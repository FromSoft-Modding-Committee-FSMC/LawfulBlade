using PnnQuant;
using System.Buffers.Text;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public class PackageManager
    {
        // Event Delegates
        public delegate void OnVoidEvent();
        public event OnVoidEvent? OnPackagePrepareCompleted;

        /// <summary>
        /// Defines the current state of the package manager.
        /// </summary>
        public enum PackageManagerState
        {
            None,
            PreparingPackages,
            Ready,
        }

        /// <summary>
        /// Used to store information about packages.
        /// </summary>
        public struct PackageData
        {
            /// <summary>
            /// The default PackageData structure.
            /// </summary>
            public static PackageData Default => new()
            {
                PackageSources = new PackageSource[] { 
                    new PackageSource
                    {
                        CreationDate = DateTime.MinValue,
                        URI          = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages", "defaultPackages.json"),
                        Packages     = Array.Empty<Package>()
                    }
                },

                AvaliablePackages = 0
            };

            [JsonInclude]
            public PackageSource[] PackageSources;

            [JsonInclude]
            public int AvaliablePackages;
        }

        /// <summary>
        /// The path of the package declaration file.
        /// </summary>
        public static string PackagesFile { get; private set; } = Path.Combine(ProgramContext.AppDataPath, @"packages.json");

        /// <summary>
        /// Cache location for downloaded packages.
        /// </summary>
        public static string PackagesCache { get; private set; } = Path.Combine(ProgramContext.AppDataPath, @"PackagesCache");

        /// <summary>
        /// Data for all packages.
        /// </summary>
        public PackageData PackagesData;

        public PackageManagerState State { get; private set; } = PackageManagerState.None;

        /// <summary>
        /// Default Constructor. Responsible for loading packages from the package declaration file.
        /// </summary>
        public PackageManager()
        {
            if (!LoadPackages())
                DefaultPackages();

            Logger.ShortInfo("Preparing packages...");
            Task.Run(PreparePackages)
                .ContinueWith(PreparePackagesFinished);

            // If the package cache directory doesn't exist, we can create it now.
            if (!Directory.Exists(PackagesCache))
            {
                Logger.ShortInfo("Creating packages cache directory...");
                Logger.ShortInfo($"\tPackagesCache = '{PackagesCache}'");

                Directory.CreateDirectory(PackagesCache);
            }
                
            // Save packages on shutdown
            Program.OnShutdown += SavePackages;
        }

        /// <summary>
        /// Load all packages from the package declaration file.
        /// </summary>
        /// <returns>True on success, False otherwise.</returns>
        bool LoadPackages()
        {
            Logger.ShortInfo("Loading packages...");

            if (!File.Exists(PackagesFile))
                return false;

            // Check Each Package List
            PackagesData = JsonSerializer.Deserialize<PackageData>(File.ReadAllText(PackagesFile));

            return true;
        }

        /// <summary>
        /// Saves PackagesData to the file system.
        /// </summary>
        /// <returns></returns>
        bool SavePackages()
        {
            File.WriteAllText(PackagesFile, JsonSerializer.Serialize(PackagesData, JsonSerializerOptions.Default));
            return true;
        }
            
        /// <summary>
        /// Initializes default packages for lawful blade
        /// </summary>
        void DefaultPackages()
        {
            Logger.ShortWarn("Writing Default PackagesData...");

            // Set PackagesData to the default
            PackagesData = PackageData.Default;

            // Grab default packages from resources...
            string jsonSource = string.Empty;

            using (StreamReader sr = new (new MemoryStream(Properties.Resources.defaultPackages)))
                jsonSource = sr.ReadToEnd();

            if (jsonSource == null)
                throw new Exception("Cannot get default packages from resources");

            // Attempt to deserialize the default packages
            PackageSource defaultPackage = JsonSerializer.Deserialize<PackageSource>(jsonSource, JsonSerializerOptions.Default);
            defaultPackage.CreationDate = DateTime.Now;
            defaultPackage.URI          = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages", "defaultPackages.json");

            // Save the first package source to disk
            File.WriteAllText(defaultPackage.URI, JsonSerializer.Serialize(defaultPackage, JsonSerializerOptions.Default));
        }

        /// <summary>
        /// Prepares packages by polling for updates, deleting invalid caches etc...
        /// </summary>
        void PreparePackages()
        {
            // Set State
            State = PackageManagerState.PreparingPackages;

            PackagesData.AvaliablePackages = 0;

            // We must download each package source and compare the creation dates to look for updates.
            for(int i = 0; i < PackagesData.PackageSources.Length; ++i)
            {
                // Grab the current package
                PackageSource currentPackage = PackagesData.PackageSources[i];

                // Convert the URI string into a URI object
                if (!Uri.TryCreate(currentPackage.URI, UriKind.RelativeOrAbsolute, out Uri? sourceUri))
                    Logger.ShortError($"Invalid package source URI = '{currentPackage.URI}'");

                if (sourceUri == null)
                    continue;

                // Attempt to download the package source.
                string localFile;

                try
                {
                    localFile = Program.DownloadManager.DownloadFileSync(sourceUri);
                }
                catch (Exception ex)
                {
                    Logger.ShortError($"Couldn't download package source '{currentPackage.URI}':\n\t{ex.Message}");
                    continue;
                }

                // De serialize the package source
                currentPackage = JsonSerializer.Deserialize<PackageSource>(File.ReadAllText(localFile), JsonSerializerOptions.Default);

                //
                // TO-DO: Update Logic
                //

                // Increment the number of avaliable packages.
                PackagesData.AvaliablePackages += currentPackage.Packages.Length;
            }
        }

        /// <summary>
        /// Called by the prepare packages task when PreparePackages has been completed.
        /// </summary>
        /// <param name="obj"></param>
        void PreparePackagesFinished(Task obj)
        {
            // Set State
            State = PackageManagerState.Ready;

            Logger.ShortInfo($"Loaded {PackagesData.AvaliablePackages} packages(s) from {PackagesData.PackageSources.Length} sources.");

            // Save Packages
            SavePackages();

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
            if (packageDirectory == null)   // Would never happen but C# doesn't STFU
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

            // Create package bundle
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
        /// Creates a package source from a directory of packages.<br/>
        /// For development purposes only.
        /// </summary>
        public static void CreatePackageSource(string uri, string directory)
        {
            // Make sure the directory exists
            if (!Directory.Exists(directory))
                return;

            // Find all package files
            List<Package> packages = new ();

            foreach (string filePath in Directory.EnumerateFileSystemEntries(directory, "*.paz", SearchOption.AllDirectories))
            {
                string jsonContent = string.Empty;

                // Get Package Meta
                using (ZipArchive packageBundle = ZipFile.OpenRead(filePath))
                {
                    // Get the meta file...
                    ZipArchiveEntry? metaFile = packageBundle.GetEntry(@"package.meta.json");

                    // Skip paz files with invalid meta files...
                    if (metaFile == null)
                        continue;

                    // Read the json file
                    using (StreamReader sr = new (metaFile.Open()))
                        jsonContent = sr.ReadToEnd();
                }

                // Make sure json content is valid.
                if (jsonContent == string.Empty)
                    continue;

                // De Serialize the json file
                Package? package = JsonSerializer.Deserialize<Package>(jsonContent, JsonSerializerOptions.Default);

                // Add package to list if it's not null.
                if (package != null)
                    packages.Add(package);
            }

            // Create the package source
            PackageSource source = new ()
            {
                CreationDate = DateTime.Now,
                URI          = uri,
                Packages     = packages.ToArray()
            };

            // Save PackageSource file to directory with packages
            File.WriteAllText(Path.Combine(directory, "packages.json"), JsonSerializer.Serialize(source, JsonSerializerOptions.Default));
        }
    }
}
