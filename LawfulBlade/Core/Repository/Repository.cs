using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public class Repository
    {
        /// <summary>
        /// Name of the repository
        /// </summary>
        [JsonInclude]
        public string Name { get; private set; }

        /// <summary>
        /// Description of the repository
        /// </summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>
        /// Root of the repository files (relative)
        /// </summary>
        [JsonInclude]
        public string PackageRoot { get; private set; }

        /// <summary>
        /// The number of packages in the repository
        /// </summary>
        [JsonInclude]
        public long PackageCount { get; private set; }

        /// <summary>
        /// Package Info for each package included in the repository 
        /// </summary>
        [JsonIgnore]
        public List<RepositoryPackage> PackageLibrary { get; private set; }

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
                PackageCount   = args.IncludedPackages.Length
            };
        
            // Make sure each requested package exists, and load it...
            foreach (string packageFile in args.IncludedPackages)
            {
                string packagePath = Path.Combine(args.CreateRoot, $"{packageFile}.IAZ");

                if (!File.Exists(Path.Combine(args.CreateRoot, $"{packageFile}.IAZ")))
                    throw new Exception($"Missing package: {packagePath}!");

                repository.PackageLibrary.Add(
                    RepositoryPackage.FromPackage(Package.Package.Load(packagePath, true))
                    );
            }

            

            return repository;
        }

        public static Repository Load(string sourceUri)
        {
            // First we must download the REP file... (do it sync)
            DownloadManager.DownloadSync(new Uri($"{sourceUri}.REP"), Path.Combine(App.TemporaryPath, "temp.rep"));
 
            Repository repository = JsonSerializer.Deserialize<Repository>(File.ReadAllText(Path.Combine(App.TemporaryPath, "temp.rep")));
            repository.URI = sourceUri;

            // Download the lib file
            DownloadManager.DownloadSync(new Uri($"{sourceUri}.LIB"), Path.Combine(App.TemporaryPath, "temp.lib"));

            // Now we must uncompress it...
            using MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(Path.Combine(App.TemporaryPath, "temp.lib")));
            using BrotliStream brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress);
            using MemoryStream decompStream = new MemoryStream();

            brotliStream.CopyTo(decompStream);

            // Turn it back into valid text and read it...
            repository.PackageLibrary = JsonSerializer.Deserialize<List<RepositoryPackage>>(Encoding.UTF8.GetString(decompStream.ToArray()));

            return repository;
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
