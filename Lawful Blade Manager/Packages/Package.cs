using LawfulBladeManager.Core;
using PnnQuant;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Reflection.Metadata;
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
        public string[] Dependencies  { get; set; } = Array.Empty<string>();

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
            File.Exists(Path.Combine(Program.Preferences.PackageCacheDirectory, BundleSourceUri));

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
        /// Gets the list of files from a package bundle
        /// </summary>
        /// <param name="bundle">The package bundle</param>
        /// <returns>The list of files inside the bundle</returns>
        public static PackageFile[] BundleGetList(ZipArchive bundle)
        {
            // Get the list entry from the bundle...
            ZipArchiveEntry? listEntry = bundle.GetEntry(@"package.list.json") ?? throw new Exception($"Invalid package bundle!");

            // Read the json list from the package...
            string listJson = string.Empty;
            using (StreamReader sr = new(listEntry.Open()))
                listJson = sr.ReadToEnd();

            // Deserialize the json into our file list
            PackageFile[]? listTemp = JsonSerializer.Deserialize<PackageFile[]>(listJson, JsonSerializerOptions.Default);

            // Make sure we got a valid list...
            return listTemp ?? throw new Exception("Couldn't deserialize the file list from the bundle!");
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
    /// An entry in a package targets library...
    /// </summary>
    public struct PackageLibraryEntry
    {
        [JsonInclude]
        public string UUID;

        [JsonInclude]
        public string Version;

        [JsonInclude]
        public PackageFile[] Files;
    }
}
