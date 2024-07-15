using PnnQuant;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
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
        /// Helper to decode a PNG from a gzip'ed base64 stream
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
            using (GZipStream gzip = new (new MemoryStream(Convert.FromBase64String(base64)), CompressionMode.Decompress))
            {
                // Create bitmap from the memory stream
                result = new Bitmap(Image.FromStream(gzip));
            }
                
            // If the result ends up being null, return the default package icon.
            result ??= Properties.Resources._128x_package;

            return result;
        }

        /// <summary>
        /// Helper to encode a PNG as a gzip'ed base64 stream
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
                // Encode PNG into a gzip compressed memory stream
                using (GZipStream gzip = new (stream, CompressionLevel.SmallestSize, true))
                    bitmap.Save(gzip, ImageFormat.Png);

                // Convert to base64 string
                result = Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.None);
            }

            return result;
        }
    }

    /// <summary>
    /// Additional package properties for ones already cached.
    /// </summary>
    public class LocalPackage : Package
    {
        [JsonInclude]
        public bool IsBundleCached   { get; set; } = false;

        [JsonInclude]
        public string BundleFilePath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a source of packages (net or local file system)
    /// </summary>
    public struct PackageSource
    {
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
