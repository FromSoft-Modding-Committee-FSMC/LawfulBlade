using LawfulBlade.Core;
using System.Windows;
using System.IO;
using LawfulBlade.Dialog;
using LawfulBlade.Core.Extensions;
using System.ComponentModel;
using LawfulBlade.Core.Package;
using System.Text.Json;

namespace LawfulBlade
{
    public partial class App : Application
    {
        /// <summary>
        /// 'Global' Constant Data
        /// </summary>
        public static readonly string Version           = @"1.01";
        public static readonly string ProgramPath       = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string AppDataPath       = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FSMC", "LawfulBlade");
        public static readonly string ProjectPath       = Path.Combine(AppDataPath, "projects");
        public static readonly string InstancePath      = Path.Combine(AppDataPath, "instances");
        public static readonly string PackageCachePath  = Path.Combine(AppDataPath, "packageCache");
        public static readonly string TemporaryPath     = Path.Combine(Path.GetTempPath(), "FSMC", "LawfulBlade");
        public static readonly string ResourcePath      = Path.Combine(ProgramPath, "Resource");
        public static readonly string ReleaseNotesFile  = Path.Combine(ProgramPath, $"release-notes-v{Version.Strip('.')}.txt");
        public static readonly string RuntimeGenPath    = Path.Combine(ProgramPath, "Runtime");
        public static readonly string ReportAProblemURL = @"https://github.com/FromSoft-Modding-Committee-FSMC/Lawful-Blade/issues";
        public static readonly string RemoteVersionURL  = @"https://lawful.swordofmoonlight.com/version.php";

        public static Preferences Preferences { get; private set; }

        /// <summary>
        /// Set during the second launch stage, when we have a chance of editing anything...
        /// </summary>
        public static bool FullShutdownEnabled { get; private set; } = false;

        /// <summary>
        /// Event Handler.<br/>
        /// Called when the program is started.
        /// </summary>
        void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // If there's another instance of this process active, fuck off instantly...
            if (Message.Warning("Cannot have more than one Lawful Blade application running at a time!", !CheckExclusiveProcess()))
                Shutdown(0);

            // Initialization Stage #1
            Preferences = Preferences.Load();

            // If there are any arguments, we can kick out...
            if (e.Args.Length > 0)
            {
                switch(e.Args[0])
                {
                    case "inst":
                        // Load all instances, then find the one with the matching UUID - launch it.
                        InstanceManager.Initialize();
                        InstanceManager.GetInstanceByUUID(e.Args[1])?.Launch(null, true);
                        break;

                    case "proj":
                        // If we want to launch a project, we need to initialize instances and projects...
                        InstanceManager.Initialize();
                        ProjectManager.Initialize();
                        ProjectManager.GetProjectByUUID(e.Args[1])?.Launch(true);
                        break;

                    case "mkpk":    // MAKE PACKAGE
                        PackageCreateArgs packageCreationArgs = PackageCreateArgs.FromFile(e.Args[1]);
                        Package.Create(packageCreationArgs).Build(packageCreationArgs);
                        break;

                    case "mkrp":    // MAKE REPOSITORY
                        RepositoryCreateArgs repositoryCreateArgs = RepositoryCreateArgs.FromFile(e.Args[1]);
                        Repository.Create(repositoryCreateArgs).Build(repositoryCreateArgs);
                        break;
                }

                // Since we're using the command line, exit now...
                Current.Shutdown(0);
                return;
            }

            FullShutdownEnabled = true;

            // Initialization Stage #2
            // This is all stuff that only needs to be done if we're going into the program proper...

            // Does our temporary folder already exist? if so, we can nuke it
            if (Directory.Exists(TemporaryPath))
                Directory.Delete(TemporaryPath, true);

            // Create our directories
            Directory.CreateDirectory(TemporaryPath);
            Directory.CreateDirectory(AppDataPath);

            // Fix Section
            Winblows.ApplyFixes();

            // Optional Debugging Console
            if (Preferences.ShowDebugConsole)
                Debug.InitConsole();

            // Load Repositories
            PackageManager.Initialize();
            Debug.Info($"There are {PackageManager.PackageCount} packages avaliable across {PackageManager.RepositoryCount} repositories...");

            // Load Instances, Projects
            InstanceManager.Initialize();
            ProjectManager.Initialize();
            Debug.Info($"Managing {InstanceManager.Count} instances, {ProjectManager.Count} projects...");

            // Load Runtime Generators...
            RuntimeManager.Initialize();
            Debug.Info($"There are {RuntimeManager.Count} runtimes avaliable!");

            // Construct new main window
            MainWindow = new MainWindow();
            MainWindow.Closing += (object sender, CancelEventArgs e) => { Current.Shutdown(); };   // Windows yet again sucking a big cock
            MainWindow.Show();

            // Do we want to check for updates?
            if (Preferences.AutoCheckForUpdates)
            {
                if (CheckForProgramUpdate(out UpdateInfo updateInfo))
                {
                    // Do we want the update?
                    if (MessageBox.Show($"A new version of Lawful Blade is avaliable! (Your version = {Version}, new version = {updateInfo.Version})\nDo you want to update?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        PerformUpdate(updateInfo);
                }
            }
        }

        /// <summary>
        /// Event Handler.<br/>
        /// Called when the program is shut down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnApplicationShutdown(object sender, ExitEventArgs e)
        {
            // Exit early when fullshut down is not enabled
            if (!FullShutdownEnabled)
                return;

            // Shutdown Instance Manager
            InstanceManager.Shutdown();

            // Shutdown Project Manager
            ProjectManager.Shutdown();

            // Shutdown package manager
            PackageManager.Shutdown();

            // Save Preferences
            Preferences.Save();
        }

        /// <summary>
        /// Checks for remote updates
        /// </summary>
        public bool CheckForProgramUpdate(out UpdateInfo versionInfo)
        {
            // The version info file does exist, lets download it into our temporary path
            bool   needsUpdate = false;

            // Start the busy dialog...
            BusyDialog.ShowBusy(
                "Lawful Blade - Checking for Updates...", 
                "So long, fare thee well. Pip pip cheerio - \r\nWe'll be back soon!"
                );

            // Check for the newest version...
            if (DownloadManager.RequestVersion(new Uri(RemoteVersionURL), out versionInfo))
            {
                // We must parse the versions as integers to do comparisons...
                int currentVersion = int.Parse(Version.Strip('.'));
                int newVersion     = int.Parse(versionInfo.Version.Strip('.'));

                needsUpdate = currentVersion < newVersion;

                // If we do need an update, download the payload now...
                if (needsUpdate)
                    DownloadManager.DownloadSync(new Uri(versionInfo.SourceF), Path.Combine(TemporaryPath, "updates.zip"));
            } 
            else
                Debug.Warn($"Cannot retrieve version information from URL: '{RemoteVersionURL}'");

            BusyDialog.HideBusy();

            return true; //needsUpdate;
        }

        /// <summary>
        /// Performs remote updates
        /// </summary>
        public void PerformUpdate(UpdateInfo versionInfo)
        {
            // First copy the updater to our temporary path...
            File.Copy(Path.Combine(ProgramPath, "LawfulBladeUpdater.exe"), Path.Combine(TemporaryPath, "LawfulBladeUpdater.exe"), true);

            // Get some info about our current process...
            string processID   = $"{Environment.ProcessId}";
            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            // Start the updater...
            System.Diagnostics.Process process = 
                System.Diagnostics.Process.Start(Path.Combine(TemporaryPath, "LawfulBladeUpdater.exe"), [processID, processName, ProgramPath]);

            Current.Shutdown(0);
        }

        /// <summary>
        /// Checks if this is the only instance of Lawful Blade running
        /// </summary>
        /// <returns></returns>
        public bool CheckExclusiveProcess()
        {
            // Get current process
            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id == currentProcess.Id)
                    continue;

                return false;
            }

            return true;
        }
    }
}