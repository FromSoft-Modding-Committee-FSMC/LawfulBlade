using ImageMagick;
using LawfulBlade.Core.Extensions;
using LawfulBlade.Core.Package;
using LawfulBlade.Dialog;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LawfulBlade.Core
{
    public partial class Instance : PackageTarget
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
        /// The time when the instance was last accessed.
        /// </summary>
        [JsonInclude]
        public DateTime LastAccessDateTime;

        /// <summary>
        /// Dictionary of avaliable commands for the instance (launchInstance, launchProject)
        /// </summary>
        [JsonInclude]
        public Dictionary<string, InstanceCommand> Commands { get; private set; }

        /// <summary>
        /// Dictionary of avaliable variables for the instance (set on any instance execution)
        /// </summary>
        [JsonInclude]
        public List<InstanceVariable> Variables { get; private set; }

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
            // Show Busy...
            BusyDialog.ShowBusy();

            // Generate a UUID for the instance
            Guid instanceGuid = GuidExtensions.GenerateGuid(creationArgs.Name, creationArgs.Description, DateTime.Now);

            // Creating instance... instance.
            Instance instance = new()
            {
                Name = creationArgs.Name,
                Description = creationArgs.Description,
                UUID = instanceGuid.ToString(),
                Root = Path.Combine(App.InstancePath, instanceGuid.ToString()),
                Packages = [],
                Tags = [.. (new string[] { "Managed" }), .. creationArgs.Tags],
                LastAccessDateTime = DateTime.Now,
                Dirty = true // Created instances are always marked dirty...
            };

            // Attempt to create the instance icon, and write it to disc
            if (creationArgs.IconFilePath != string.Empty && File.Exists(creationArgs.IconFilePath))
                instance.IconImage = new(creationArgs.IconFilePath);
            else
                instance.IconImage = new(Path.Combine(App.ResourcePath, "256x256_instancedefault.png"));

            if (instance.IconImage.Width != 128 || instance.IconImage.Height != 128)
                instance.IconImage.Resize(128, 128);

            // Create the directory tree if it does not exist...
            if (!Directory.Exists(instance.Root))
                Directory.CreateDirectory(instance.Root);

            // Store the package UUID because fuck me I guess
            string packageUUID = creationArgs.CorePackageUUID;

            // Now we get + install the package
            instance.InstallPackage(PackageManager.GetPackageByUUID(packageUUID));

            // Locate and "install" var.json
            string varJsonFile = Path.Combine(instance.Root, "var.json");
            if (!File.Exists(varJsonFile))
                Debug.Warn("No 'var.json' file found! You will have to manually create one!");
            else
            {
                instance.Variables = JsonSerializer.Deserialize<List<InstanceVariable>>(File.ReadAllText(varJsonFile));
                File.Delete(varJsonFile);
            }

            // Locate and 'install' cmd.json
            string cmdJsonFile = Path.Combine(instance.Root, "cmd.json");
            if (!File.Exists(cmdJsonFile))
                Debug.Warn("No 'cmd.json' file found! You will have to manually create one!");
            else
            {
                instance.Commands = JsonSerializer.Deserialize<Dictionary<string, InstanceCommand>>(File.ReadAllText(cmdJsonFile));
                File.Delete(cmdJsonFile);
            }

            // After installing the package, hide busy...
            BusyDialog.HideBusy();
               
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
                    instance.LastAccessDateTime = DateTime.Now;
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

        /// <summary>
        /// Creates a shortcut to the instance
        /// </summary>
        public void CreateShortcut()
        {
            // Get the current users desktop path
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Name}.url");
            string batchcutPath = Path.Combine(Root, "shortcut.bat");

            // The URL format we're using doesn't support arguments, so instead we create a bat file to launch the instance...
            using (StreamWriter sw = new (File.Open(batchcutPath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine($"@echo off");
                sw.WriteLine($"start \"\" \"{Path.Combine(App.ProgramPath, "LawfulBlade.exe")}\" inst {UUID}");
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

            LastAccessDateTime = DateTime.Now;
            Dirty = true;

            Message.Info("Shortcut Created!", true);
        }

        /// <summary>
        /// Launches the instance directly...
        /// </summary>
        public void Launch(Project project, bool silent = false)
        {
            // Set each variable to the registry...
            Debug.Warn("Modifying Sword of Moonlight Registry!!!");

            foreach (InstanceVariable variable in Variables)
            {
                // We need to expand any #var tags inside the value field...
                Debug.Info($"[Root = {variable.Root}, Key = {variable.Key}, Type = {variable.Type}, Value = {variable.Value}]");

                RegistryHelper.SetValue(variable.Root, variable.Key, 
                    variable.Type switch
                    {
                        // String type can be expanded
                        InstanceVariableType.String => ExpandVariable(variable.Value, project),

                        // Any other variable type can not.
                        _                           => variable.GetValue(),
                    }
                );
            }

            // Now we can attempt to launch...
            InstanceCommand launchCommand;

            if (project == null)
            {
                // When project is null, we are trying to launch an instance.
                if (!Commands.TryGetValue("launchInstance", out launchCommand))
                {
                    Debug.Error($"Cannot launch instance! no 'launchInstance' command specified!!");
                    return;
                }

                Debug.Info($"Launching Instance: '{Name}'!");
            }
            else
            {
                // If project is not null, we're trying to launch a project
                if (!Commands.TryGetValue("launchProject", out launchCommand))
                {
                    Debug.Error($"Cannot launch project! no 'launchProject' command specified!!");
                    return;
                }

                Debug.Info($"Launching Project: '{project.Name}'!");
            }

            // We now need to expand our command, because some variables might be present...
            launchCommand = new()
            {
                Execute   = ExpandVariable(launchCommand.Execute, project),
                Arguments = launchCommand.Arguments.Select(x => ExpandVariable(x, project)).ToArray()  // CHEEKY CHEEKY CHEEKY HEHEHEHEEHEH
            };

            // Also adding \"\" to any arguments with spaces.
            for (int i = 0; i < launchCommand.Arguments.Length; ++i)
                launchCommand.Arguments[i] =
                    launchCommand.Arguments[i].Contains(' ') ? $"\"{launchCommand.Arguments[i]}\"" : launchCommand.Arguments[i];

            // If the user wants to use Locale Emulator, we have to shift all this shit to the right,
            // and use locale emulator as the main executor
            if (App.Preferences.UseLocaleEmulator)
            {
                // Target executable is shifted into a argument...
                launchCommand.Arguments = (new string[] { launchCommand.Execute }).Concat(launchCommand.Arguments).ToArray();

                // Locale Emulator is loaded into the execute field.
                launchCommand.Execute = Path.Combine(App.Preferences.LocaleEmulatorPath, "LEProc.exe");
            }

            // Set the last access and dirty settings
            LastAccessDateTime = DateTime.Now;
            Dirty = true;

            // Finally we cna create the process start info, and start the process...
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = launchCommand.Execute,
                Arguments = string.Join(' ', launchCommand.Arguments).Trim()
            }) 
                ?? throw new Exception("Couldn't start process!");

            // We're using silent for when we're executing this from shortcuts etc... We don't need to listen in then.
            if (!silent)
            {
                // The management object will look for child processes (SoM is cunt)
                ManagementObjectSearcher processWatcher = new($"Select * From Win32_Process Where ParentProcessID={process.Id}");

                // Hide Lawful Blade...
                App.Current.MainWindow.Hide();

                // Wait for this process before exiting...
                process.WaitForInputIdle();

                do
                {
                    while (!process.HasExited)
                    {
                        //
                        // HERE WE CAN IMPLEMENT SOME WATCHER LOGIC IF WE NEED IT...
                        //
                    }

                    // Also use this to wait, because our wait loop above isn't very precise...
                    process.WaitForExit();

                    // Update our local process to whatever processes that one creates are...
                    foreach (ManagementBaseObject mo in processWatcher.Get())
                    {
                        // Get the new process...
                        process = Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]));

                        // Dispose old process watcher and create new one in the mean time
                        processWatcher.Dispose();
                        processWatcher = new($"Select * From Win32_Process Where ParentProcessID={mo["ProcessID"]}");

                        // Wait for the process to be ready to receive input
                        process.WaitForInputIdle();

                        // We'll also GC here because why not?
                        GC.Collect();

                        Debug.Warn($"Changed active process... {process.ProcessName}");

                        // We only care about the first process, so we'll exit now...
                        break;
                    }

                    // Waiting for process to exit...
                } while (!process.HasExited);

                // When the instance is finally fully closed, we can show LB again...
                App.Current.MainWindow.Show();
            }
        }

        /// <summary>
        /// Expands variables in a given string
        /// </summary>
        string ExpandVariable(string variableSource, Project project)
        {
            // Get N matches for expansion...
            MatchCollection matches = ExpandVariablePattern().Matches(variableSource);

            // We now want to operate on each match.
            foreach (Match match in matches)
            {
                // Second group will be the variable
                string expandedVariable = match.Groups[1].Value switch
                {
                    "programDir"  => App.ProgramPath,
                    "instanceDir" => Root,
                    "projectDir"  => project == null ? throw new NotImplementedException() : project.Root,
                    _ => throw new Exception($"Unknown Variable: '{match.Groups[1].Value}'")
                };

                // Now we've expanded the variable, we can replace it. the first group is the target for replacement.
                variableSource = variableSource.Replace(match.Groups[0].Value, expandedVariable);  
            }

            return variableSource;
        }

        /// <summary>
        /// Used to match instance variable pattern
        /// </summary>
        [GeneratedRegex("#var:([A-Za-z]+[A-Za-z0-9]*);")]
        private static partial Regex ExpandVariablePattern();
    }
}
