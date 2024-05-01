using System.Drawing.Imaging;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace LawfulBladeManager.Packages
{
    public class PackageManager
    {
        // Private Fields
        List<string> packageList = new List<string>();

        // Public Fields
        public List<string> PackageList => packageList;

        public PackageManager()
        {
            // Find Default Packages
            foreach (string file in Directory.GetFiles(Path.Combine(ProgramContext.ProgramPath, "Packages"), "*.LBP", SearchOption.AllDirectories))
                packageList.Add(file);
        }

        public void PackageCreate(PackageCreateArgs args)
        {
            // In order to generate a unique ID for the package, we store an MD5 Crypto object here
            MD5 accumMD5 = MD5.Create();

            // Scan all files in the source and generate PackageFiles for them
            List<PackageFile> packageFiles = new();
            foreach(string file in Directory.GetFileSystemEntries(args.SourceDirectory, "*", SearchOption.AllDirectories))
            {
                // Only operate when this is a file
                if (!File.Exists(file))
                    continue;

                // Generate an MD5 of the file
                byte[] checksumBytes = accumMD5.ComputeHash(File.ReadAllBytes(file));

                packageFiles.Add(new PackageFile
                {
                    Filename = file.Replace(args.SourceDirectory, "")[1..],
                    Checksum = Convert.ToBase64String(checksumBytes)
                });
            }

            if (accumMD5.Hash == null)
                throw new Exception("MD5 cryptography weirdly doesn't exist");

            // Create the package structure
            Package package = new()
            {
                Name = args.Name,
                Description = args.Description,
                Version = args.Version,
                Authors = args.Authors,
                Tags = args.Tags,
                UUID = new Guid(accumMD5.Hash).ToString(),
                ExpectOverwrite = args.ExpectOverwrite
            };

            //
            // ZIP CONSTRUCTION FUN
            //
            using(ZipArchive zip = ZipFile.Open($"{args.TargetFile}", ZipArchiveMode.Create))
            {
                // Add each file that was found in the directory.
                foreach(PackageFile file in packageFiles)
                    zip.CreateEntryFromFile(Path.Combine(args.SourceDirectory, file.Filename), file.Filename);

                // Now add our 'icon.png', 'package.json', 'package.list.json'
                ZipArchiveEntry iconEntry = zip.CreateEntry(@"icon.png");
                using (Stream s = iconEntry.Open())
                    args.Icon.Save(s, ImageFormat.Png);

                ZipArchiveEntry packEntry = zip.CreateEntry(@"package.json");
                using (StreamWriter sw = new(packEntry.Open()))
                    sw.Write(JsonSerializer.Serialize(package, JsonSerializerOptions.Default));

                ZipArchiveEntry listEntry = zip.CreateEntry(@"package.list.json");
                using (StreamWriter sw = new(listEntry.Open()))
                    sw.Write(JsonSerializer.Serialize(packageFiles, JsonSerializerOptions.Default));
            }
        }
    }
}
