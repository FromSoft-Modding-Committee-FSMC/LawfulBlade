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

                AvaliablePackages = 0,
                ReadyPackages = new List<LocalPackage>()
            };

            [JsonInclude]
            public PackageSource[] PackageSources;

            [JsonInclude]
            public int AvaliablePackages;

            [JsonInclude]
            public List<LocalPackage> ReadyPackages;
        }

        /// <summary>
        /// The path of the package declaration file.
        /// </summary>
        public string PackagesFile { get; private set; } = Path.Combine(ProgramContext.AppDataPath, @"packages.json");

        /// <summary>
        /// Cache location for downloaded packages.
        /// </summary>
        public string PackagesCache { get; private set; } = Path.Combine(ProgramContext.AppDataPath, @"PackagesCache");

        /// <summary>
        /// Data for all packages.
        /// </summary>
        public PackageData PackagesData;

        public PackageManagerState State { get; private set; } = PackageManagerState.None;

        // cache our image quanitizer here...
        static readonly PnnQuantizer ImageQuantizer = new ();

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

            using (StreamReader sr = new StreamReader(new MemoryStream(Properties.Resources.defaultPackages)))
                jsonSource = sr.ReadToEnd();

            if (jsonSource == null)
                throw new Exception("Cannot get default packages from resources");

            // Attempt to deserialize the default packages
            PackageSource defaultPackage = JsonSerializer.Deserialize<PackageSource>(jsonSource, JsonSerializerOptions.Default);
            defaultPackage.CreationDate = DateTime.Now;
            defaultPackage.URI          = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages");

            // Save the first package source to disk
            File.WriteAllText(Path.Combine(defaultPackage.URI, "defaultPackages.json"), JsonSerializer.Serialize(defaultPackage, JsonSerializerOptions.Default));
        }

        /// <summary>
        /// Prepares packages by polling for updates, deleting invalid caches etc...
        /// </summary>
        void PreparePackages()
        {
            // Set State
            State = PackageManagerState.PreparingPackages;

            // We must download each package source and compare the creation dates to look for updates.
            for(int i = 0; i < PackagesData.PackageSources.Length; ++i)
            {
                // Grab the current package
                PackageSource currentPackage = PackagesData.PackageSources[i];

                // Convert the URI string into a URI object
                Uri? sourceUri;

                if (!Uri.TryCreate(currentPackage.URI, UriKind.RelativeOrAbsolute, out sourceUri))
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
                PackageSource temporaryPackage = JsonSerializer.Deserialize<PackageSource>(File.ReadAllText(localFile), JsonSerializerOptions.Default);

                // Is the loaded packageSource newer than our current one?
                if (currentPackage.CreationDate.AddDays(7) < temporaryPackage.CreationDate)
                {
                    // Remove any local packages with a matching UUID
                    foreach(Package sourcePackage in currentPackage.Packages)
                    {
                        // Loop through any local packages with matching UUID and non matching version
                        foreach(LocalPackage invalidatedPackage in PackagesData.ReadyPackages.Where(x => (x.UUID == sourcePackage.UUID && x.Version != sourcePackage.Version)))
                        {
                            // Delete cached bundle
                            if (invalidatedPackage.IsBundleCached)
                                File.Delete(invalidatedPackage.BundleFilePath);

                            // Remove from local packages list.
                            PackagesData.ReadyPackages.Remove(invalidatedPackage);
                        }
                    }

                    // Replace the current package in our package sources...
                    PackagesData.PackageSources[i] = temporaryPackage;

                    // Replace the current package copy
                    currentPackage = temporaryPackage;

                    Logger.ShortWrite("PkgM".Colourize(0x008000), $"\tUpdated Package '{temporaryPackage.URI}'".Colourize(0xFFFF00));
                }

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

            Logger.ShortInfo($"Loaded {PackagesData.AvaliablePackages} packages(s) from {PackagesData.PackageSources.Length} sources. [{PackagesData.ReadyPackages.Count} ready].");

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

            // Generate a base64 encoded icon
            string iconBase64 = string.Empty;

            // Get the target icon or the default icon
            args.IconSource ??= Properties.Resources._128x_package; // We don't want a null icon.

            // Resize the icon to 128x128
            args.IconSource = new Bitmap(args.IconSource, new Size(128, 128));

            // Quantize the icon to an 8bpp palette
            args.IconSource = ImageQuantizer.QuantizeImage(args.IconSource, PixelFormat.Format8bppIndexed, 256, true);

            // Convert the icon to a base64 encoded png
            using(MemoryStream stream = new())
            {
                // Encode PNG into a gzip compressed memory stream
                using (GZipStream gzip = new GZipStream(stream, CompressionLevel.SmallestSize, true))
                    args.IconSource.Save(gzip, ImageFormat.Png);

                // Convert to base64 string
                iconBase64 = Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.None);
            }

            // Find package files
            List<PackageFile> packageList = new List<PackageFile>();
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
                IconBase64 = iconBase64
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
            List<Package> packages = new List<Package>();

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
                    using (StreamReader sr = new StreamReader(metaFile.Open()))
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
            PackageSource source = new PackageSource
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
