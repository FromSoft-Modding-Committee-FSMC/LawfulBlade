using LawfulBladeManager.Core;
using PnnQuant;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZstdSharp;

namespace LawfulBladeManager.Packages
{
    /// <summary>
    /// Enum declaring different statues a package can be in
    /// </summary>
    [Flags]
    public enum PackageStatusFlag : uint
    {
        OutOfDate   = (1 << 0),
        Installed   = (1 << 1),
        Cached      = (1 << 2),
        Conflicting = (1 << 3),

        None        = 0
    }

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
    /// Represents a single package
    /// </summary>
    public class Package
    {
        [JsonInclude]
        public string Name            { get; set; } = string.Empty;

        [JsonInclude]
        public string Description     { get; set; } = string.Empty;

        [JsonInclude]
        public string Version         { get; set; } = string.Empty;

        [JsonInclude]
        public string[] Authors       { get; set; } = Array.Empty<string>();

        [JsonInclude]
        public string[] Tags          { get; set; } = Array.Empty<string>();

        [JsonInclude]
        public string UUID            { get; set; } = string.Empty;

        [JsonInclude]
        public string BundleSourceUri { get; set; } = string.Empty;

        [JsonInclude]
        public string IconBase64      { get; set; } = string.Empty;

        /// <summary>
        /// Helper to check if the package exists in the cache.
        /// </summary>
        /// <returns></returns>
        public bool PackageExistsInCache() =>
            File.Exists(Path.Combine(Program.Preferences.PackageCacheDirectory, $"{UUID}_{Version}.paz"));

        /// <summary>
        /// Global helper to decode a PNG from a base64 stream
        /// </summary>
        /// <param name="base64">The PNG file as base64</param>
        /// <returns>The PNG as a bitmap object</returns>
        public static Bitmap DecodeIcon(string? base64)
        {
            // When we pass a null or empty icon, we return the default package icon.
            if (base64 == null || base64 == string.Empty)
                return Properties.Resources._128x_package;

            Bitmap? result;

            // Decode base64 to byte array and uncompress the png
            using (MemoryStream stream = new(Convert.FromBase64String(base64)))
                result = new Bitmap(Image.FromStream(stream));
                
            // If the result ends up being null, return the default package icon.
            result ??= Properties.Resources._128x_package;

            return result;
        }

        /// <summary>
        /// Global helper to encode a PNG as a base64 string
        /// </summary>
        /// <param name="bitmap">The source png as a bitmap object</param>
        /// <returns>The png encoded as base64</returns>
        public static string EncodeIcon(Bitmap? bitmap)
        {
            string result = string.Empty;

            // When we pass a null bitmap, we encode the default package icon.
            bitmap ??= Properties.Resources._128x_package;

            // Resize the icon to 128x128
            bitmap = new Bitmap(bitmap, new Size(128, 128));

            // Quantize the icon to an 8bpp palette
            bitmap = new PnnQuantizer().QuantizeImage(bitmap, PixelFormat.Format8bppIndexed, 256, true);

            // Convert the icon to a base64 encoded png
            using (MemoryStream stream = new())
            {
                // Write PNG into the stream
                bitmap.Save(stream, ImageFormat.Png);

                // Encode into Base64
                result = Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.None);
            }

            return result;
        }

        /// <summary>
        /// Loads a package from disc...
        /// </summary>
        /// <param name="file">The package file</param>
        /// <returns>The package meta</returns>
        public static Package Load(string file)
        {
            // Open the file as a zip archive
            using ZipArchive paz = ZipFile.OpenRead(file);

            // Get the meta entry (or null)
            ZipArchiveEntry? metaEntry = paz.GetEntry(@"package.meta.json") ?? throw new Exception($"Invalid Package: '{file}'");

            // Deserialize the meta entry
            using (StreamReader sr = new (metaEntry.Open()))
            {
                // Read the json
                string metaJson = sr.ReadToEnd();

                // Deserialize it...
                Package? metaTemp = JsonSerializer.Deserialize<Package>(metaJson);

                // Return the loaded package.
                return metaTemp ?? throw new Exception($"Invalid Package: '{file}'");
            }
        }
    }

    /// <summary>
    /// Represents a source of packages (net or local file system)
    /// </summary>
    public struct PackageSource
    {
        /// <summary>
        /// Default PackageSource is used when initializing new sources.
        /// </summary>
        [JsonIgnore]
        public static PackageSource Default =>
            new()
            {
                CreationDate = DateTime.MinValue,
                URI = string.Empty,
                Packages = Array.Empty<Package>()
            };

        /// <summary>
        /// Creation Date is used to poll source updates.
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// URI is the origin of the package. Used when we need to pull updates.
        /// </summary>
        [JsonInclude]
        public string URI { get; set; }

        /// <summary>
        /// Packages contained in the source.
        /// </summary>
        [JsonInclude]
        public Package[] Packages { get; set; }

        /// <summary>
        /// Creates a new package source
        /// </summary>
        /// <param name="uri">the -final- location of the source (i.e your webserver)</param>
        /// <param name="packages">Packages to include in the source</param>
        /// <returns>A package source</returns>
        public static PackageSource Create(string uri, List<Package> packages)
        {
            // Create the package source
            PackageSource source = new()
            {
                CreationDate = DateTime.Now,
                URI = uri,
                Packages = packages.ToArray()
            };

            return source;
        }

        /// <summary>
        /// Opens a previously created source and returns a buffer
        /// </summary>
        /// <param name="file">The package source file</param>
        /// <returns>A buffer containing the content of the source</returns>
        public static byte[] Open(string file)
        {
            if (!File.Exists(file))
                throw new Exception($"File does not exist: '{file}'");

            return File.ReadAllBytes(file);
        }

        /// <summary>
        /// Compresses a package source with zstd, and returns a buffer with the compressed data
        /// </summary>
        /// <param name="source">The source to compress</param>
        /// <returns>Compressed buffer</returns>
        public static byte[] Compress(ref PackageSource source)
        {
            // Serialize The Source
            string serializedSource = JsonSerializer.Serialize(source, JsonSerializerOptions.Default);

            // Get the source as bytes
            byte[] bufferedSource = Encoding.UTF8.GetBytes(serializedSource);

            // Compress as ZSTD and return it
            using (Compressor zstdCompressor = new (8))
                return zstdCompressor.Wrap(bufferedSource).ToArray();
        }

        /// <summary>
        /// Decompresses a package source and returns it.
        /// </summary>
        /// <param name="source">compressed package source buffer</param>
        /// <returns>The decompressed package source</returns>
        public static PackageSource Decompress(ref byte[] source)
        {
            byte[] decompressedSource;

            // Decompress the byte stream from ZSTD
            using (Decompressor zstdDecompressor = new ())
                decompressedSource = zstdDecompressor.Unwrap(source).ToArray();

            // Get the source as a string...
            string serializedSource = Encoding.UTF8.GetString(decompressedSource);

            // Deserialize the source and return it
            return JsonSerializer.Deserialize<PackageSource>(serializedSource, JsonSerializerOptions.Default);
        }
    }

    /// <summary>
    /// Represents a file in a package or package target
    /// </summary>
    public struct PackageFile
    {
        [JsonInclude]
        public string Filename;

        [JsonInclude]
        public string Checksum;

        [JsonInclude]
        public string Source;
    }

    /// <summary>
    /// Used in the package manager to store information about packages.
    /// </summary>
    public struct PackageData
    {
        /// <summary>
        /// The default PackageData structure.
        /// </summary>
        public static PackageData Default => new()
        {
            PackageSources = new List<PackageSource>() {
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
        public List<PackageSource> PackageSources;

        [JsonInclude]
        public int AvaliablePackages;
    }
}
