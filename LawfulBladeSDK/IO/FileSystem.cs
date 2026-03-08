using LawfulBladeSDK.Extensions;
using LawfulBladeSDK.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeSDK.IO
{
    public class FileSystem
    {
        public static bool EnableLogging { get; set; } = true;

        /// <summary>
        /// Creates a directory at path
        /// </summary>
        /// <returns>The created directory</returns>
        public static string CreateDirectory(string path, int logDirLength = 3)
        {
            // If the directory exists, don't do anything...
            if (Directory.Exists(path))
                return path.ToUpperInvariant();

            // Create the directory
            Directory.CreateDirectory(path.ToUpperInvariant());

            // Logging
            if (EnableLogging)
                Console.WriteLine($"Create Directory {{ Target = '{ShortPath(path, logDirLength)}' }}".Colourise(0x4040F0));

            return path.ToUpperInvariant();
        }

        /// <summary>
        /// Copies a directory from source to target
        /// </summary>
        /// <returns>The target directory</returns>
        public static string CopyDirectory(string source, string target, int sourceLogLength = 3, int targetLogLength = 3)
        {
            CopyDirectoryInternal(new DirectoryInfo(source), target);

            // Logging
            if (EnableLogging)
                Console.WriteLine($"Copy Directory {{ Source = {ShortPath(source, sourceLogLength)}, Target = '{ShortPath(target, targetLogLength)}' }}".Colourise(0x4040F0));

            return target.ToUpperInvariant();
        }

        /// <summary>
        /// Internal copy directory which recurses the path
        /// </summary>
        static void CopyDirectoryInternal(DirectoryInfo source, string target)
        {
            // Make sure the root directory exists...
            if (!source.Exists)
                throw new DirectoryNotFoundException();

            // Create the target directory...
            Directory.CreateDirectory(target.ToUpperInvariant());

            // Copy all files inside the directory...
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target, file.Name).ToUpperInvariant(), true);

            // Copy all subdirectories (recursive
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyDirectoryInternal(dir, Path.Combine(target, dir.Name));
        }

        /// <summary>
        /// Copies a file from source to target
        /// </summary>
        /// <returns><b>True</b> if the file was copied</returns>
        public static bool CopyFile(string source, string target, int sourceLogLength = 3, int targetLogLength = 3)
        {
            if (EnableLogging)
                Console.WriteLine($"Copy File {{ Source = '{ShortPath(source, sourceLogLength)}', Target = '{ShortPath(target, targetLogLength)}' }}".Colourise(0x40f040));

            if (!File.Exists(source))
            {
                if (EnableLogging)
                    Console.WriteLine("\t^ ^ ^ SOURCE FILE MISSING !!! ^ ^ ^".Colourise(0xF08080));

                return false;
            }
                
            File.Copy(source, target.ToUpperInvariant(), true);
            return true;
        }

        /// <summary>
        /// Copies a file from source to target, building a path from the supplied parts
        /// </summary>
        public static bool CopyFile(string[] sourceParts, string[] targetParts, int sourceLogLength = 3, int targetLogLength = 3) =>
            CopyFile(Path.Combine(sourceParts), Path.Combine(targetParts), sourceLogLength, targetLogLength);

        /// <summary>
        /// Returns if a file exists at the path
        /// </summary>
        public static bool FileExists(string path) =>
            File.Exists(path);

        /// <summary>
        /// Returns if a file exists at the path, building a path from the supplied parts
        /// </summary>
        public static bool FileExists(string[] pathParts) =>
            File.Exists(Path.Combine(pathParts));

        /// <summary>
        /// Creates a binary file at target
        /// </summary>
        public static void CreateFile(string target, byte[] data, int targetLogLength = 3)
        {
            if (EnableLogging)
                Console.WriteLine($"Create File {{ Target = '{ShortPath(target, targetLogLength)}' }}".Colourise(0x40f040));

            using (FileStream fs = File.Open(target.ToUpperInvariant(), FileMode.Create))
            {
                fs.Write(data);
                fs.Flush();
            }
        }

        /// <summary>
        /// Creates a text file at target
        /// </summary>
        public static void CreateFile(string target, string[] data, int targetLogLength = 3, bool keepLowerCase = false)
        {
            if (EnableLogging)
                Console.WriteLine($"Create File {{ Target = '{ShortPath(target, targetLogLength)}' }}".Colourise(0x40f040));

            string newName = target;
            if (!keepLowerCase)
                newName = newName.ToUpperInvariant();

            using (StreamWriter sw = new StreamWriter(File.Open(newName, FileMode.Create)))
            {
                foreach (string s in data)
                    sw.WriteLine(s);

                sw.Flush();
            }
        }

        public static void RenameFile(string target, string newName, int targetLogLength = 3)
        {
            if (EnableLogging)
                Console.WriteLine($"Rename File {{ Source = '{ShortPath(target, targetLogLength)}', New Name = '{newName}' }}".Colourise(0x40f040));

            // Get the path to the file
            string rootPath = Path.GetDirectoryName(target);

            File.Move(target, $"{Path.Combine(rootPath, newName)}", true);
        }

        /// <summary>
        /// Shortens a path to the last n parts
        /// </summary>
        static string ShortPath(string path, int length)
        {
            string[] parts = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return $".{Path.DirectorySeparatorChar}{Path.Combine(parts.Skip(parts.Length - length).ToArray())}";
        }
    }
}
