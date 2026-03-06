using LawfulBlade.Core.Package;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public class Repository
    {
        /// <summary>Name of the repository</summary>
        [JsonInclude]
        public string Name { get; private set; }

        /// <summary>Description of the repository</summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>Root of the repository files (relative)</summary>
        [JsonInclude]
        public string PackageRoot { get; private set; }

        /// <summary>The number of packages in the repository</summary>
        [JsonInclude]
        public long PackageCount { get; private set; }

        /// <summary>The access mode of the repository. Reserved for future use.</summary>
        [JsonInclude, JsonConverter(typeof(JsonStringEnumConverter))]
        public RepositoryAccessMode AccessMode { get; private set; }

        /// <summary>Package Info for each package included in the repository </summary>
        [JsonIgnore]
        public List<RepositoryPackage> PackageLibrary { get; private set; }

        /// <summary>The source URI of the repository</summary>
        [JsonIgnore]
        public string URI { get; private set; }

        /// <summary>
        /// Creates a repository based on the provided arguments
        /// </summary>
        public static Repository Create(RepositoryCreateArgs args)
        {
            // Create the repository...
            Repository repository = new Repository
            {
                Name           = args.Name,
                Description    = args.Description,
                PackageRoot    = Path.GetFileName(args.PackageRoot),
                PackageLibrary = new List<RepositoryPackage>(args.IncludedPackages.Length),
                PackageCount   = args.IncludedPackages.Length,
                AccessMode     = RepositoryAccessMode.FileSystem
            };
        
            // Make sure each requested package exists, and load it...
            foreach (string packageFile in args.IncludedPackages)
            {
                string packagePath = Path.Combine(args.CreateRoot, $"{packageFile}.IAZ");

                if (!File.Exists(Path.Combine(args.CreateRoot, $"{packageFile}.IAZ")))
                    throw new Exception($"Missing package: {packagePath}!");

                RepositoryPackage repoPackage = RepositoryPackage.FromPackage(Package.Package.Load(packagePath, true));
                repoPackage.Bundle = $"{packageFile}.IAZ";

                repository.PackageLibrary.Add(repoPackage);
            }            

            return repository;
        }

        /// <summary>
        /// Loads a repository from a source
        /// </summary>>
        public static bool Load(string sourceUri, out Repository repository)
        {
            // Turn our URI string into a URI object
            Uri repositoryURI = PackageManager.ExpandURI(sourceUri);

            // We need to download some files if it's not a local repository, assume files are local (???)
            if (!repositoryURI.IsFile)
            {
                DownloadManager.DownloadSync(new Uri($"{repositoryURI}.REP"), Path.Combine(App.TemporaryPath, "TEMP.REP"));
                DownloadManager.DownloadSync(new Uri($"{repositoryURI}.LIB"), Path.Combine(App.TemporaryPath, "TEMP.LIB"));
                repositoryURI = new Uri(Path.Combine(App.TemporaryPath, "TEMP"));
            }

            try
            {
                // Check if the definition and library exist
                if (!File.Exists($"{repositoryURI.LocalPath}.REP"))
                    throw new Exception($"Repository Definition missing! {repositoryURI.LocalPath}.REP");

                if (!File.Exists($"{repositoryURI.LocalPath}.LIB"))
                    throw new Exception($"Repository Library missing! {repositoryURI.LocalPath}.LIB");

                // Load repository definition
                repository     = JsonSerializer.Deserialize<Repository>(File.ReadAllText($"{repositoryURI.LocalPath}.REP"));
                repository.URI = sourceUri;

                // Load repository library
                using MemoryStream memoryStream = new (File.ReadAllBytes($"{repositoryURI.LocalPath}.LIB"));
                using BrotliStream brotliStream = new (memoryStream, CompressionMode.Decompress);
                using MemoryStream decompStream = new ();

                // Copy from brotli to the decomp stream, which is decompiling the data
                brotliStream.CopyTo(decompStream);

                // Convert the decompiled stream back into text, and deserialise it.
                repository.PackageLibrary = JsonSerializer.Deserialize<List<RepositoryPackage>>(Encoding.UTF8.GetString(decompStream.ToArray()));
            } 
            catch (Exception ex)
            {
                Debug.Critical(ex.Message);
                repository = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Builds the repository for distribution
        /// </summary>
        public void Build(RepositoryCreateArgs args)
        {
            // Create the {PackageName}.REP file
            File.WriteAllText(Path.Combine(args.CreateRoot, $"{PackageRoot}.REP"), JsonSerializer.Serialize(this));

            // Create the {PackageName}.LIB file
            byte[] encodedLib = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(PackageLibrary));

            using FileStream outputStream   = File.OpenWrite(Path.Combine(args.CreateRoot, $"{PackageRoot}.LIB"));
            using BrotliStream brotliStream = new (outputStream, CompressionLevel.SmallestSize);
            brotliStream.Write(encodedLib);
            brotliStream.Flush();
        }
    }
}
