using System.Diagnostics;
using System.IO.Compression;

namespace LawfulBladeUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Before we start, we must check our argument count. The Updater expects 3 arguments:
            //  1: Lawful Blade Process ID
            //  2: Lawful Blade Process Name (To check if multiple instances are open)
            //  3: The Program Directory (since updater will be moved to the temp folder before working...)

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid argument count!".Colourize(0xF39C12));
                goto UpdateFail;
            }


            // Lets make sure the process that launched the updater is closed...
            try
            {
                Process.GetProcessById(int.Parse(args[0])).WaitForExit();
            }
            catch { };  // We don't actually bother to catch the exception, because we expect it to fail on fast hardware.

            // Now we want to make sure no other instances of Lawful Blade are running...
            if (Process.GetProcessesByName(args[1]).Length > 0)
            {
                Console.WriteLine("Cannot update while you still have a Lawful Blade process open! Please close Lawful Blade!".Colourize(0xF39C12));
                goto UpdateFail;
            }

            // Get the info file path...
            string infoFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version");

            // When the info file doesn't exist... oopsy...
            if (!File.Exists(infoFile))
            {
                Console.WriteLine("Couldn't find version file! The update cannot proceed.".Colourize(0xF39C12));
                goto UpdateFail;
            }

            // Read the info file...
            string versionNumber = string.Empty, updateFileName = string.Empty;
            bool breakingChanges = false;

            using (StreamReader sr = new(File.OpenRead(infoFile)))
            {
                versionNumber   = sr.ReadLine() ?? string.Empty;
                updateFileName  = Path.GetFileName(sr.ReadLine()) ?? string.Empty;
                breakingChanges = bool.Parse(sr.ReadLine() ?? "false");
            }

            // Get the update file path...
            string updateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, updateFileName);

            if (!File.Exists(updateFile))
            {
                Console.WriteLine($"Couldn't find '{updateFile}'! The update cannot proceed.".Colourize(0xF39C12));
                goto UpdateFail;
            }

            Console.WriteLine("Installing Update...".Colourize(0x3498DB));

            using (ZipArchive update = ZipFile.OpenRead(updateFile))
            {
                // Loop through each entry and extract it...
                foreach(ZipArchiveEntry entry in update.Entries)
                {
                    Console.WriteLine($"\tExtracting '{entry.FullName}'...".Colourize(0x3498DB));
                    entry.ExtractToFile(Path.Combine(args[2], entry.FullName), true);
                }
            }

            // We can now clean up by deleting the update dir
            Console.WriteLine($"Cleaning...");
            File.Delete(updateFile);
            File.Delete(infoFile);
            goto UpdateSuccess;

            UpdateSuccess:
            Console.WriteLine($"Success!\nYou are now running Lawful Blade Version {versionNumber}!".Colourize(0x58D68D));
            Console.ReadKey();
            return;

            UpdateFail:
            Console.WriteLine("Failed!!".Colourize(0xE74C3C));
            Console.ReadKey();
            return;
        }
    }

    public static class DirtyCow
    {
        public static string Colourize(this string input, uint colour) =>
            $"\u001b[38;2;{(colour >> 16) & 0xFF};{(colour >> 8) & 0xFF};{(colour >> 0) & 0xFF}m{input}\u001b[0m";
    }
}