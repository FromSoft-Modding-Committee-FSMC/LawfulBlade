using ImageMagick;
using LawfulBlade.Core.Extensions;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace LawfulBlade.Core.Package
{
    public class Package
    {
        /// <summary>
        /// Name of the package
        /// </summary>
        [JsonInclude]
        public string Name { get; private set; }

        /// <summary>
        /// Description of the package
        /// </summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>
        /// Version of the package...
        /// </summary>
        [JsonInclude]
        public string Version { get; private set; }

        /// <summary>
        /// Authors of the package
        /// </summary>
        [JsonInclude]
        public string[] Authors  { get; private set; }

        /// <summary>
        /// Any tags assosiated with the package
        /// </summary>
        [JsonInclude]
        public string[] Tags { get; private set; }

        /// <summary>
        /// Any packages required for this package
        /// </summary>
        [JsonInclude]
        public string[] Dependencies { get; private set; }

        /// <summary>
        /// UUID of the package...
        /// </summary>
        [JsonInclude]
        public string UUID { get; private set; }

        /// <summary>
        /// Bundle file path
        /// </summary>
        [JsonInclude]
        public string Bundle { get; private set; }

        /// <summary>
        /// The small image. 48x48...
        /// </summary>
        [JsonIgnore]
        public MagickImage Icon { get; private set; }

        /// <summary>
        /// The contents of the package
        /// </summary>
        [JsonIgnore]
        public PackageFile[] Contents { get; private set; }

        /// <summary>
        /// Creates a package based on the provided arguments
        /// </summary>
        public static Package Create(in PackageCreateArgs args)
        {
            // Package...
            Package package = new ()
            {
                Name        = args.Name,
                Description = args.Description,
                Version     = args.Version,
                Authors     = args.Authors,
                Tags        = args.Tags,
                UUID        = GuidExtensions.GenerateGuid(args.Name, args.Description, DateTime.UtcNow).ToString()
            };

            // Wrangle dependency UUIDs using the names...
            List<string> dependencies = [];
            foreach(string depencencyName in args.Dependencies)
            {
                // In order to resolve the dependency, we must load it...
                string dependencyPath = Path.Combine(args.Root, $"{depencencyName}.IAZ");

                if (!File.Exists(dependencyPath))
                    throw new Exception("Missing dependency!!");

                // We can now store this dependency...
                dependencies.Add(Load(dependencyPath, true).UUID);
            }

            package.Dependencies = dependencies.ToArray();

            // If the icon file doesn't exist, we'll load up the default
            if (!File.Exists(Path.Combine(args.Root, $"{args.PackageName}.png")))
                package.Icon = new MagickImage(Path.Combine(App.ProgramPath, "Resources", "128x128_packagedefault.png"));
            else
                package.Icon = new MagickImage(Path.Combine(args.Root, $"{args.PackageName}.png"));

            if (package.Icon.Width != 128 || package.Icon.Height != 128)
                package.Icon.Resize(128, 128);

            // Quantize the large image to 256 colours
            package.Icon.Quantize(new QuantizeSettings
            {
                Colors = 256,
                ColorSpace = ColorSpace.RGB,
                MeasureErrors = false,
                TreeDepth = 2048,
                DitherMethod = DitherMethod.Riemersma
            });

            // Gather the contents of the package...
            List<PackageFile> files = [];

            // We want to iterate through everything inside the payload...
            string payload = Path.Combine(args.Root, args.PackageName);

            // First we're going to look for any EXE files an enable LAA.
            foreach (FileInfo file in (new DirectoryInfo(payload)).GetFiles("*.exe", SearchOption.AllDirectories))
                LargeAddressAware.SetLAAFlag(file.FullName);

            // Second, create the contents entry for every file.
            foreach (FileInfo file in (new DirectoryInfo(payload)).GetFiles("*", SearchOption.AllDirectories))
            {
                files.Add(new PackageFile
                {
                    Path  = file.FullName.Replace(payload, "")[1..],    // Screw that first backslash off...
                    FNV64 = HashThis.BufferTo64(File.ReadAllBytes(file.FullName))
                });
            }

            package.Contents = files.ToArray();

            return package;
        }

        /// <summary>
        /// Builds the bundle file for use in repositories...
        /// </summary>
        public void Build(in PackageCreateArgs args)
        {
            string targetName = Path.Combine(args.Root, $"{args.PackageName}.IAZ");

            // Deleting the old bundle (if it exists)
            if (File.Exists(targetName))
                File.Delete(targetName);

            // Building the archive bundle...
            using (ZipArchive packageBundle = ZipFile.Open(targetName, ZipArchiveMode.Create))
            {
                // Create the LB.PACKAGE.INFO entry
                using (StreamWriter sw = new(packageBundle.CreateEntry(@"LB.PACKAGE.INFO").Open()))
                    sw.Write(JsonSerializer.Serialize(this));

                // Create the LB.PACKAGE.TREE entry
                using (StreamWriter sw = new(packageBundle.CreateEntry(@"LB.PACKAGE.TREE").Open()))
                    sw.Write(JsonSerializer.Serialize(Contents));

                // Create the LB.PACKAGE.ICON entry
                using (BinaryWriter bw = new(packageBundle.CreateEntry(@"LB.PACKAGE.ICON").Open()))
                    bw.Write(Icon.ToByteArray(MagickFormat.Png));

                // Now we can add all file sources to the bundle...
                foreach (PackageFile file in Contents)
                    packageBundle.CreateEntryFromFile(Path.Combine(args.Root, args.PackageName, file.Path), file.Path);
            }
        }

        /// <summary>
        /// Loads the basic package
        /// </summary>
        public static Package Load(string path, bool liteLoad = false)
        {
            // Open the Zip File...
            using ZipArchive bundle = ZipFile.Open(path, ZipArchiveMode.Read);

            // Get the info entry...
            ZipArchiveEntry lbPackageInfo = bundle.GetEntry("LB.PACKAGE.INFO")
                ?? throw new Exception("Package file does not contain info, it is not a Lawful Blade package!");

            Package package;

            // Deserialize the info file as the package file
            using (StreamReader sr = new(lbPackageInfo.Open()))
            {
                package = JsonSerializer.Deserialize<Package>(sr.ReadToEnd())
                    ?? throw new Exception("Package file contains invalid info, it is corrupt!");
            }

            // icon entry
            ZipArchiveEntry lbPackageIcon = bundle.GetEntry("LB.PACKAGE.ICON")
                ?? throw new Exception("Package file does not contain icon, it is not a Lawful Blade package!");

            using (BinaryReader br = new(lbPackageIcon.Open()))
            {
                package.Icon = new MagickImage(br.ReadBytes((int)lbPackageIcon.Length));
            }

            if (!liteLoad)
            {
                // If liteload is false, we also load everything else....

                // tree entry
                ZipArchiveEntry lbPackageTree = bundle.GetEntry("LB.PACKAGE.TREE")
                    ?? throw new Exception("Package file does not contain tree, it is not a Lawful Blade package!");

                using (StreamReader sr = new(lbPackageTree.Open()))
                {
                    package.Contents = JsonSerializer.Deserialize<PackageFile[]>(sr.ReadToEnd())
                     ?? throw new Exception("Package file contains invalid tree, it is corrupt!");
                }
            }
            
            // Store the bundle path in the package...
            package.Bundle = path;

            return package;
        }
    }
}
