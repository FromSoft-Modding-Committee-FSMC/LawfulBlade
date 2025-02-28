using ImageMagick;
using LawfulBlade.Core.Extensions;
using LawfulBlade.Dialog;
using System.Buffers;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace LawfulBlade.Core
{
    public class Project
    {
        /// <summary>
        /// The name of the project
        /// </summary>
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// The description of the project
        /// </summary>
        [JsonInclude]
        public string Description { get; private set; }

        /// <summary>
        /// The unique identifer of the project
        /// </summary>
        [JsonInclude]
        public string UUID { get; private set; }

        /// <summary>
        /// The time when the project was last accessed.
        /// </summary>
        [JsonInclude]
        public DateTime LastAccessDateTime;

        /// <summary>
        /// The unique identifier of the instance which owns the project
        /// </summary>
        [JsonInclude]
        public string InstanceUUID { get; private set; }

        /// <summary>
        /// The root data location of the package target
        /// </summary>
        [JsonIgnore]
        public string Root { get; protected set; }

        /// <summary>
        /// The icon data for this project
        /// </summary>
        [JsonIgnore]
        public MagickImage Icon { get; private set; }

        /// <summary>
        /// JSON only constructor
        /// </summary>
        [JsonConstructor]
        Project() { }

        /// <summary>
        /// Creates a new project from creation arguments
        /// </summary>
        /// <param name="createArgs">The project configuration</param>
        /// <returns>A new project</returns>
        public static Project Create(in ProjectCreateArgs createArgs)
        {
            // Show Busy...
            BusyDialog.ShowBusy();

            // Generate a UUID for the project
            Guid uuid = GuidExtensions.GenerateGuid(createArgs.Name, createArgs.Description, DateTime.Now);

            // Create project
            Project project = new Project
            {
                Name               = createArgs.Name,
                Description        = createArgs.Description,
                UUID               = $"{uuid}",
                LastAccessDateTime = DateTime.Now,
                InstanceUUID       = createArgs.Owner.UUID,
                Root               = Path.Combine(App.ProjectPath, $"{uuid}")
            };

            // Render up the project icon
            if (createArgs.IconFile != string.Empty && File.Exists(createArgs.IconFile))
                project.Icon = new(createArgs.IconFile);
            else
                project.Icon = new(Path.Combine(App.ResourcePath, "256x256_projectdefault.png"));

            if (project.Icon.Width != 128 || project.Icon.Height != 128)
                project.Icon.Resize(128, 128);

            // Create the directory tree if it does not exist...
            if (!Directory.Exists(project.Root))
                Directory.CreateDirectory(project.Root);

            // We need to copy the template project from the instance...
            new DirectoryInfo(Path.Combine(createArgs.Owner.Root, "template")).Copy(project.Root);

            // And create the .SOM file
            using StreamWriter projectSom = new(File.Open(Path.Combine(project.Root, $"{project.Name}.som"), FileMode.CreateNew));
            projectSom.WriteLine(project.Name);
            projectSom.WriteLine(0);
            projectSom.Flush();
            projectSom.Close();

            // Save the project.json file now, because we don't really need to save these constantly
            File.WriteAllText(Path.Combine(project.Root, $"project.json"), JsonSerializer.Serialize(project));

            // Hide Busy
            BusyDialog.HideBusy();

            return project;
        }

        /// <summary>
        /// Loads a project from a directory
        /// </summary>
        /// <param name="directory">The directory the project is contained in</param>
        public static Project Load(string directory)
        {
            Project project;

            try
            {
                string targetFile;

                // Load project.json
                targetFile = Path.Combine(directory, "project.json");
                if (!File.Exists(targetFile))
                    throw new Exception($"File does not exist: '{targetFile}'");

                project = JsonSerializer.Deserialize<Project>(File.ReadAllText(targetFile))
                    ?? throw new Exception($"Couldn't deserialize file: '{targetFile}'!");

                // Load project.ico
                targetFile = Path.Combine(directory, "project.ico");
                if (!File.Exists(targetFile))
                    project.Icon = new(Path.Combine(App.ResourcePath, "256x256_projectdefault.png"));
                else
                    project.Icon = new(targetFile);

                if (project.Icon.Width != 128 || project.Icon.Height != 128)
                    project.Icon.Resize(128, 128);

            }
            catch (Exception)
            {
                Debug.Error($"Couldn't load project from directory: '{directory}'!");
                return null;
            }

            project.Root = directory;

            return project;
        }

        /// <summary>
        /// Deletes a project from Lawful Blade and the file system
        /// </summary>
        public void Delete()
        {
            // Magick.NET seems to keep a handle, and doing this removes it?..
            Icon = null;

            // Trigger the recursive delete (this should only operate on "owned" files eventually...)
            new DirectoryInfo(Root).Delete(true);
        }

        /// <summary>
        /// Saves an project, updating it's data.
        /// </summary>
        public void Save()
        {
            // Create the directory tree if it does not exist...
            if (!Directory.Exists(Root))
                Directory.CreateDirectory(Root);

            // Save instance.json
            File.WriteAllText(Path.Combine(Root, "project.json"), JsonSerializer.Serialize(this));

            // Save instance.ico
            Icon.Write(Path.Combine(Root, "project.ico"), MagickFormat.Ico);
        }

        /// <summary>
        /// Creates a shortcut to the project
        /// </summary>
        public void CreateShortcut()
        {
            // Get the current users desktop path
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Name}.url");
            string batchcutPath = Path.Combine(Root, "shortcut.bat");

            // The URL format we're using doesn't support arguments, so instead we create a bat file to launch the project...
            using (StreamWriter sw = new(File.Open(batchcutPath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine($"@echo off");
                sw.WriteLine($"start \"\" \"{Path.Combine(App.ProgramPath, "LawfulBlade.exe")}\" proj {UUID}");
                sw.WriteLine($"exit 0");
            }

            // We now write the shortcut
            using (StreamWriter sw = new(File.Open(shortcutPath, FileMode.Create, FileAccess.Write)))
            {
                // Write our URL manually (very simple format, who cares)
                sw.WriteLine($"[InternetShortcut]");
                sw.WriteLine($"URL=\"file:///{batchcutPath}\"");
                sw.WriteLine($"IconIndex=0");
                sw.WriteLine($"IconFile={Path.Combine(Root, "project.ico")}");
            }

            LastAccessDateTime = DateTime.Now;

            Message.Info("Shortcut Created!", true);
        }

        /// <summary>
        /// Launches a project
        /// </summary>
        public void Launch(bool silent)
        {
            InstanceManager.GetInstanceByUUID(InstanceUUID).Launch(this, silent);
            LastAccessDateTime = DateTime.Now;
        } 
    }
}
