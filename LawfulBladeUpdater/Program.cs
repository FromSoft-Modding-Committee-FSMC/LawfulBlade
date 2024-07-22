using System.IO.Compression;
using System.Reflection;

namespace LawfulBladeUpdater
{
    // This is so crap, but who cares?..

    internal class Program
    {
        static string ProgramDir = AppDomain.CurrentDomain.BaseDirectory;
        static string UpdateDir  = Path.Combine(ProgramDir, "update");

        static void Main(string[] args)
        {
            Console.WriteLine("Updating...");

            // Wait 2 seconds before updating, so lawful blade can fully close.
            Thread.Sleep(2000);

            // Check if the update directory exists, if it doesn't - we exit.
            Console.WriteLine($"Source: {UpdateDir}");

            if (!Directory.Exists(UpdateDir))
                return;

            // Read version file from the update dir
            string verNum, verFile, verShit;

            using (StreamReader sr = new StreamReader(File.OpenRead(Path.Combine(Path.Combine(UpdateDir, "version")))))
            {
                verNum = sr.ReadLine() ?? "0.00X";
                verFile = Path.GetFileName(sr.ReadLine() ?? string.Empty);
                verShit = sr.ReadLine() ?? "false";
            }

            Console.WriteLine($"New: {verNum}, File: {verFile}, Breaking Changes?: {verShit}");

            // Installing the update is a simple matter of extracting the zip.
            Console.WriteLine($"Extracting...");
            using (ZipArchive updateZip = ZipFile.OpenRead(Path.Combine(UpdateDir, verFile)))
            {
                foreach (var entry in updateZip.Entries)
                {
                    // Skip updater.exe... DON'T INCLUDE THIS MUPPET...
                    if (entry.FullName == "updater.exe")
                        continue;

                    entry.ExtractToFile(Path.Combine(ProgramDir, entry.FullName), true);
                }
            }

            // We can now clean up by deleting the update dir
            Console.WriteLine($"Cleaning...");
            Directory.Delete(UpdateDir, true);

            Console.WriteLine($"Done!");
            Thread.Sleep(1000);
        }
    }
}