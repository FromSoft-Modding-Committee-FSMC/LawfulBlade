using System.Diagnostics;
using System.IO.Compression;

using LawfulBladeSDK.Extensions;

namespace LawfulBladeUpdater
{
    /// <summary>
    /// Lawful Blade Updater entry point class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point method
        /// </summary>
        static void Main(string[] args)
        {
            // Argument mapping:
            //  1: Lawful Blade Process ID
            //  2: Lawful Blade Process Name (To check if multiple instances are open)
            //  3: The Program Directory (since updater will be moved to the temp folder before working...)

            try
            {
                // Make sure 3 or more arguments are provided.
                if (args.Length < 3)
                    throw new Exception("Invalid argument count!".Colourise(0xF39C12));

                //
                // Parse our arguments, and perform some basic validity checks.
                //
                if (!int.TryParse(args[0], out int lawfulBladeProcessId))
                    throw new Exception("Invalid process ID provided!".Colourise(0xF39C12));

                string lawfulBladeProcessName = args[1];
                string lawfulBladeDirectory = args[2];

                if (!Directory.Exists(lawfulBladeDirectory))
                    throw new Exception("Invalid program directory provided!".Colourise(0xF39C12));

                // Now we want to wait for the calling process to exit. This might fail on faster hardware, so we'll throw it in it's own empty try catch.
                try { Process.GetProcessById(lawfulBladeProcessId).WaitForExit(); } catch { }

                if (Process.GetProcessesByName(lawfulBladeProcessName).Length > 0)
                    throw new Exception("Multiple instances of Lawful Blade were open! Please ensure that all instances of Lawful Blade are closed.".Colourise(0xF39C12));

                string updatePayloadFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updates.zip");

                if (!File.Exists(updatePayloadFile))
                    throw new Exception($"Couldn't find update payload: '{updatePayloadFile}'".Colourise(0xF39C12));

                //
                // Perform the update
                //
                Console.WriteLine("Installing update...".Colourise(0x3498DB));

                using ZipArchive updatePayload = ZipFile.OpenRead(updatePayloadFile);

                foreach (ZipArchiveEntry updateEntry in updatePayload.Entries)
                {
                    // Create the target path for the entry
                    string updateTarget = Path.GetDirectoryName(Path.Combine(lawfulBladeDirectory, updateEntry.FullName)) ?? throw new Exception("Invalid update target path".Colourise(0xF39C12));

                    // Ensure that a valid directory exists...
                    if (!Directory.Exists(updateTarget))
                    {
                        Console.WriteLine($"\tCreating Directory: '{updateTarget}'".Colourise(0x34DB98));
                        Directory.CreateDirectory(updateTarget);
                    }

                    // This entry could itself just be a directory. We've already handled those, so don't try to create them as files.
                    if (updateEntry.Name == string.Empty)
                        continue;

                    string updateTargetFile = Path.Combine(updateTarget, updateEntry.Name);

                    if (File.Exists(updateTargetFile))
                        Console.WriteLine($"\tReplacing File: '{updateTargetFile}'");
                    else
                        Console.WriteLine($"\tAdding File: '{updateTargetFile}'");

                    updateEntry.ExtractToFile(updateTargetFile, true);
                }

                //
                // Clean up
                //
                File.Delete(updatePayloadFile);

                //
                // Relaunch 
                //
                Process.Start(new ProcessStartInfo(Path.Combine(lawfulBladeDirectory, "LawfulBlade.exe")));
            }
            catch (Exception ex)
            {
                // The .NET STD is pretty good with having very specific exceptions.
                // We'll assume if an exception is thrown using the base class, it's ours - if it isn't, we also output the stack trace.
                if (ex.GetType() == typeof(Exception))
                    Console.WriteLine(ex.Message);
                else
                {
                    Console.WriteLine(ex.Message.Colourise(0xF08080));
                    Console.WriteLine(ex.StackTrace);
                }

                // We don't want to automatically close the terminal if the update failed, so we can learn why.
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}