using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void Copy(this DirectoryInfo root, string destination)
        {
            // Make sure the root directory exists...
            if (!root.Exists)
                throw new DirectoryNotFoundException();

            // Create the target directory...
            Directory.CreateDirectory(destination);

            // Copy all files inside the directory...
            foreach (FileInfo file in root.GetFiles())
                file.CopyTo(Path.Combine(destination, file.Name), true);

            // Copy all subdirectories (recursive
            foreach (DirectoryInfo dir in root.GetDirectories())
                Copy(dir, Path.Combine(destination, dir.Name));
        }
    }
}
