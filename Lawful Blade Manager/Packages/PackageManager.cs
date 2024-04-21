using System.Security.Cryptography;

namespace LawfulBladeManager.Packages
{
    public class PackageManager
    {
        public PackageManager()
        {

        }

        public void PackageCreate(PackageCreateArgs args)
        {
            // In order to generate a unique ID for the package, we store an MD5 Crypto object here
            MD5 accumMD5 = MD5.Create();

            // Scan all files in the source and generate PackageFiles for them
            List<PackageFile> packageFiles = new();
            foreach(string file in Directory.GetFileSystemEntries(args.SourceDirectory))
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
            Package package = new Package
            {
                Name = args.Name,
                Description = args.Description,
                Version = args.Version,
                Authors = args.Authors,
                Tags = args.Tags,
                UUID = new Guid(accumMD5.Hash).ToString(),
                Files = packageFiles.ToArray()
            };

            //
            // ZIP CONSTRUCTION FUN
            //
        }
    }
}
