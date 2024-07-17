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
            File.Exists(Path.Combine(PackageManager.PackagesCache, $"{UUID}_{Version}.paz"));

        /// <summary>
        /// Global helper to decode a PNG from a gzip'ed base64 stream
        /// </summary>
        /// <param name="base64">The PNG file as compressed base64.</param>
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
        /// Global helper to encode a PNG as a gzip'ed base64 stream
        /// </summary>
        /// <param name="bitmap">The source png as a bitmap object</param>
        /// <returns>The png encoded as base64 and compressed with gzip</returns>
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

        public static byte[] Compress(ref PackageSource source)
        {
            // Serialize The Source
            string serializedSource = JsonSerializer.Serialize(source, JsonSerializerOptions.Default);

            // Get the source as bytes
            byte[] bufferedSource = Encoding.UTF8.GetBytes(serializedSource);

            // Compress as ZSTD and return it
            using (Compressor zstdCompressor = new Compressor(5))
                return zstdCompressor.Wrap(bufferedSource).ToArray();
        }

        public static PackageSource Decompress(ref byte[] source)
        {
            byte[] decompressedSource;

            // Decompress the byte stream from ZSTD
            using (Decompressor zstdDecompressor = new Decompressor())
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
}
