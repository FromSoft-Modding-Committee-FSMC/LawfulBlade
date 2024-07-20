using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using ZstdSharp;

namespace LawfulBladeManager.Packages
{
    /// <summary>
    /// Represents repository information
    /// </summary>
    public class PackageRepositoryInfo
    {
        /// <summary>
        /// Default package repository info
        /// </summary>
        [JsonIgnore]
        public static PackageRepositoryInfo Default => new();

        /// <summary>
        /// Name of the source, 32 max characters
        /// </summary>
        [JsonInclude]
        public string Name           { get; set; } = string.Empty;

        /// <summary>
        /// Description of the source, 128 max characters
        /// </summary>
        [JsonInclude]
        public string Description    { get; set; } = string.Empty;

        /// <summary>
        /// The date the source was created.
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The root directory of the package source.
        /// </summary>
        [JsonInclude]
        public string URI            { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a repository of packages
    /// </summary>
    public class PackageRepository
    {
        /// <summary>
        /// Stores information about the repository
        /// </summary>
        [JsonInclude]
        public PackageRepositoryInfo Info { get; private set; } = PackageRepositoryInfo.Default;

        /// <summary>
        /// Array of packages included in the repository
        /// </summary>
        [JsonInclude]
        public Package[] Packages         { get; private set; } = Array.Empty<Package>();

        /// <summary>
        /// Constructor for a package repository, when JSON is being deserialized.
        /// </summary>
        [JsonConstructor]
        public PackageRepository() { }

        /// <summary>
        /// Constructor for a package repository...
        /// </summary>
        /// <param name="repositoryInfo">Information about the repository...</param>
        /// <param name="packages"></param>
        public PackageRepository(PackageRepositoryInfo info, Package[] bundle)
        {
            Info     = info;
            Packages = bundle;
        }

        /// <summary>
        /// Loads the information file from a package repository...
        /// </summary>
        /// <param name="file">The file containing the info...</param>
        /// <returns>The repository info</returns>
        public static PackageRepositoryInfo LoadInfo(string file)
        {
            // Does the file exist?
            if (!File.Exists(file))
                throw new Exception($"Couldn't find repository.inf: '{file}'");

            // Load serialized info
            string serializedInfo = File.ReadAllText(file);

            // Deserialize info
            PackageRepositoryInfo? tempInfo;
            tempInfo = JsonSerializer.Deserialize<PackageRepositoryInfo>(serializedInfo, JsonSerializerOptions.Default);

            // Check if load was successful
            if(tempInfo == null)
                throw new Exception($"Couldn't load repository.inf from: '{file}'");

            // Return the valid info...
            return tempInfo;
        }

        /// <summary>
        /// Loads the bundle file from a package repository...
        /// </summary>
        /// <param name="file">The file containing packages</param>
        /// <returns>An array of packages</returns>
        public static Package[] LoadBundle(string file)
        {
            // Does the file exist?
            if (!File.Exists(file))
                throw new Exception($"Couldn't find repository.cpk: '{file}'");

            // Load the compressed file...
            byte[] compressedBuffer   = File.ReadAllBytes(file);
            byte[] decompressedBuffer;

            // Decompress the file
            using Decompressor zstdDecompressor = new();
            decompressedBuffer = zstdDecompressor.Unwrap(compressedBuffer).ToArray();

            // Recreate the serialized string...
            string serializedBundle = Encoding.UTF8.GetString(decompressedBuffer);

            // Deserialize the package list
            Package[]? tempBundle;
            tempBundle = JsonSerializer.Deserialize<Package[]>(serializedBundle, JsonSerializerOptions.Default);

            if (tempBundle == null)
                throw new Exception($"Couldn't load repository.cpk from: '{file}'");

            return tempBundle;
        }

        /// <summary>
        /// Saves an array of packages as a repository bundle (cpk)
        /// </summary>
        /// <param name="filename">the output file name and path</param>
        /// <param name="packages">the packages</param>
        public static void SaveBundle(string filename, Package[] packages)
        {
            // Serialize the packages as a bundle...
            string serializedBundle = JsonSerializer.Serialize(packages, JsonSerializerOptions.Default);

            // Convert the serialized bundle to a decompressed buffer of bytes
            byte[] decompressedBuffer = Encoding.UTF8.GetBytes(serializedBundle);
            byte[] compressedBuffer;

            // Compress the buffer
            using Compressor zstdCompressor = new();
            compressedBuffer = zstdCompressor.Wrap(decompressedBuffer).ToArray();

            // Write to the file system
            File.WriteAllBytes(filename, compressedBuffer);
        }
    }

    /// <summary>
    /// Arguments for creating repositories
    /// </summary>
    public class PackageRepositoryCreateArgs
    {
        /// <summary>
        /// Target path for repository creation
        /// </summary>
        public string OutputDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Bundle source files
        /// </summary>
        public string[] BundleSources { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Chosen name for the repository
        /// </summary>
        public string Name        { get; set; } = string.Empty;

        /// <summary>
        /// Chosen description for the repository
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
