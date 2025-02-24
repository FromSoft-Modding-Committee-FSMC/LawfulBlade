using ImageMagick;
using LawfulBlade.Core.Extensions;
using LawfulBlade.Core.Package;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace LawfulBlade.Core.Instance
{
    public class Instance : PackageTarget
    {
        /// <summary>
        /// The name of the instance
        /// </summary>
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// The description of the instance
        /// </summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>
        /// The unique identifer of the instance
        /// </summary>
        [JsonInclude]
        public string UUID { get; private set; }

        /// <summary>
        /// Array of tags applied to the project
        /// </summary>
        [JsonInclude]
        public string[] Tags { get; private set; }

        /// <summary>
        /// The icon data for this instance
        /// </summary>
        [JsonIgnore]
        public MagickImage IconImage { get; private set; }

        /// <summary>
        /// JSON only constructor
        /// </summary>
        [JsonConstructor]
        Instance() { }

        /// <summary>
        /// Creates a new instance from creation arguments
        /// </summary>
        /// <param name="creationArgs">The instance configuration</param>
        /// <returns>A new instance</returns>
        public static Instance Create(in InstanceCreateArgs creationArgs)
        {
            // Generate a UUID for the instance
            Guid instanceGuid = GuidExtensions.GenerateGuid(creationArgs.Name, creationArgs.Description, DateTime.Now);

            // Creating instance... instance.
            Instance instance = new()
            {
                Name         = creationArgs.Name,
                Description  = creationArgs.Description,
                UUID         = instanceGuid.ToString(),
                Root         = Path.Combine(App.InstancePath, instanceGuid.ToString()),
                Packages     = [],
                Tags         = [.. (new string[] { "Managed" }), .. creationArgs.Tags],
                Dirty        = true // Created instances are always marked dirty...
            };

            // Attempt to create the instance icon, and write it to disc
            if (creationArgs.IconFilePath != string.Empty && File.Exists(creationArgs.IconFilePath))
                instance.IconImage = new(creationArgs.IconFilePath);
            else
                instance.IconImage = new(Path.Combine(App.ResourcePath, "256x256_instancedefault.png"));

            if (instance.IconImage.Width != 128 || instance.IconImage.Height != 128)
                instance.IconImage.Resize(128, 128);

            return instance;
        }

        /// <summary>
        /// Attempts to import a folder as an instance
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Instance Import(string directory)
        {
            Instance instance = null;

            // 1: Is this just a (new) Lawful Blade instance?..
            if (File.Exists(Path.Combine(directory, "instance.json")))
            {
                instance = Load(directory);

                // Must override the root, UUID and dirty state...
                if (instance != null)
                {
                    instance.UUID  = GuidExtensions.GenerateGuid(instance.Name, instance.Description, DateTime.Now).ToString();
                    instance.Root  = Path.Combine(App.InstancePath, instance.UUID);
                    instance.Tags = [.. instance.Tags, .. new string[] { "Imported" }];
                    instance.Dirty = true;
                }   
            }
            else
            // 2: Is this a classic SoM instance?
            if (Directory.Exists(Path.Combine(directory, "TOOL")) && Directory.Exists(Path.Combine(directory, "DATA")))
            {
                // Generate a UUID for the instance, using parts of the directory...
                string[] splits = directory.Split(Path.DirectorySeparatorChar);
                string dirSection1 = splits[3135 % splits.Length];
                string dirSection2 = splits[7771 % splits.Length];

                Guid instanceGuid = GuidExtensions.GenerateGuid(dirSection1, dirSection2, DateTime.Now);

                // Creating instance... instance.
                instance = new()
                {
                    Name        = Path.GetFileName(directory) ?? "Imported Instance",
                    Description = @"An imported Sword of Moonlight instance.",
                    UUID        = instanceGuid.ToString(),
                    Root        = Path.Combine(App.InstancePath, instanceGuid.ToString()),
                    Packages    = [],
                    Tags        = ["Managed", "Imported"],
                    IconImage   = new(Path.Combine(App.ResourcePath, "256x256_instancedefault.png")),
                    Dirty       = true // Created instances are always marked dirty
                };
            }

            // Make sure the first import step was completed before doing a data copy...
            if (instance == null)
                throw new Exception("Instance could not be imported!");

            if (instance.IconImage.Width != 128 || instance.IconImage.Height != 128)
                instance.IconImage.Resize(128, 128);

            // Since we are _importing, we must copy the context of the directory to its new home...
            new DirectoryInfo(directory).Copy(instance.Root);

            return instance;
;        }

        /// <summary>
        /// Loads an instance from a directory
        /// </summary>
        /// <param name="directory">The directory the instance is contained in</param>
        /// <returns></returns>
        public static Instance Load(string directory)
        {
            Instance instance;

            try
            {
                string targetFile;

                // Load instance.json
                targetFile = Path.Combine(directory, "instance.json");
                if (!File.Exists(targetFile))
                    throw new Exception($"File does not exist: '{targetFile}'");

                instance = JsonSerializer.Deserialize<Instance>(File.ReadAllText(targetFile))
                    ?? throw new Exception($"Couldn't deserialize file: '{targetFile}'!");

                // Load packages.json
                targetFile = Path.Combine(directory, "packages.json");
                if (!File.Exists(targetFile))
                    throw new Exception($"File does not exist: '{targetFile}'");

                instance.Packages = JsonSerializer.Deserialize<List<PackageReference>>(File.ReadAllText(targetFile))
                    ?? throw new Exception($"Couldn't deserialize file: '{targetFile}'!");

                // Load instance.ico
                targetFile = Path.Combine(directory, "instance.ico");
                if (!File.Exists(targetFile))
                    instance.IconImage = new(Path.Combine(App.ResourcePath, "256x256_instancedefault.png"));
                else
                    instance.IconImage = new(targetFile);

                if (instance.IconImage.Width != 128 || instance.IconImage.Height != 128)
                    instance.IconImage.Resize(128, 128);
            }
            catch
            {
                Debug.Error($"Couldn't load instance from directory: '{directory}'!");
                return null;
            }

            // Assign any properties not loaded from the instance file.
            instance.Root = directory;
            instance.Dirty = false; // Since we just loaded, the instance is not dirty.

            return instance;
        }

        /// <summary>
        /// Deletes an instance from Lawful Blade and the file system
        /// </summary>
        public void Delete()
        {
            // Magick.NET seems to keep a handle, and doing this removes it?..
            IconImage = null;

            // Trigger the recursive delete (this should only operate on "owned" files eventually...)
            new DirectoryInfo(Root).Delete(true);
        }

        /// <summary>
        /// Saves an instance, updating it's data.
        /// </summary>
        public void Save()
        {
            // Exit save if the instance is not dirty...
            if (!Dirty)
                return;

            // Create the directory tree if it does not exist...
            if (!Directory.Exists(Root))
                Directory.CreateDirectory(Root);

            // Save instance.json
            File.WriteAllText(Path.Combine(Root, "instance.json"), JsonSerializer.Serialize(this));

            // Save packages.json
            File.WriteAllText(Path.Combine(Root, "packages.json"), JsonSerializer.Serialize(Packages));

            // Save instance.ico
            IconImage.Write(Path.Combine(Root, "instance.ico"), MagickFormat.Ico);

            // Now we've saved, the instance is no longer dirty.
            Dirty = false;
        }

        public void CreateShortcut()
        {
            // Get the current users desktop path
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Name}.url");
            string batchcutPath = Path.Combine(Root, "shortcut.bat");

            // The URL format we're using doesn't support arguments, so instead we create a bat file to launch the instance...
            using (StreamWriter sw = new (File.Open(batchcutPath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine($"@echo off");
                sw.WriteLine($"start \"\" \"{Path.Combine(App.ProgramPath, "LawfulBlade.exe")}\" --inst {UUID}");
                sw.WriteLine($"exit 0");
            }

            // We now write the shortcut
            using (StreamWriter sw = new (File.Open(shortcutPath, FileMode.Create, FileAccess.Write)))
            {
                // Write our URL manually (very simple format, who cares)
                sw.WriteLine($"[InternetShortcut]");
                sw.WriteLine($"URL=\"file:///{batchcutPath}\"");
                sw.WriteLine($"IconIndex=0");
                sw.WriteLine($"IconFile={Path.Combine(Root, "instance.ico")}");
            }
        }
    }
}
